using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterMovement : MonoBehaviour
{
    public float speed;
    public GameObject objectHolding;
    public GameManager game;
    public GameObject stick;
    public float stickSpeed = 4f;
    public bool isInteracting = false;
    public Animator anim;
    public bool movingToNextDay = false;
    public MainMenu menu;

    public UIManager uimgr;

    private bool startStick = false;
    private BoxCollider2D bc2;
    public bool isCurrentlySelecting = false, goingToBackyard = false, goingToHouse = false;

    private float timeBetweenSwitchUp = .4f, timeSinceUp = 0;
    private float timeBetweenSwitchDn = .15f, timeSinceDn = 0;

    public bool isInBackyard = false;

    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        bc2 = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!goingToBackyard && !goingToHouse && !movingToNextDay && game.canStartGame)
        {
            if (Input.GetAxis("Horizontal") != 0)
            {
                transform.position += new Vector3(Input.GetAxis("Horizontal") * speed * Time.fixedDeltaTime, 0, 0);
                FindObjectOfType<AudioManager>().Play("Walk");
            }

            if (Input.GetAxis("Vertical") != 0)
            {
                transform.position += new Vector3(0, Input.GetAxis("Vertical") * speed * Time.fixedDeltaTime, 0);
                FindObjectOfType<AudioManager>().Play("Walk");
            }
            
            if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
            {
                FindObjectOfType<AudioManager>().Stop("Walk");
            }
        }

        
    }

    private void Update()
    {
        anim.SetBool("holding", !(objectHolding == null));
        timeSinceUp += Time.deltaTime;
        timeSinceDn += Time.deltaTime;
        if (Input.GetButtonDown("Interact") && timeSinceDn >= timeBetweenSwitchDn)
        {
            isInteracting = true;
            timeSinceDn = 0;
        }
        else if(timeSinceUp >= timeBetweenSwitchUp)
        {
            isInteracting = false;
            timeSinceUp = 0;
        }

        if (goingToBackyard)
        {
            if(Camera.main.transform.position.y <= 9.95)
            {
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(0, 10, -10), 5 * Time.deltaTime);

                bc2.enabled = false;
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 6.5f, 0), 10 * Time.deltaTime);
            }
            else
            {
                Camera.main.transform.position = new Vector3(0, 10, -10);
                bc2.enabled = true;
                goingToBackyard = false;
                isInBackyard = true;
            }
        }

        if (goingToHouse)
        {
            if (Camera.main.transform.position.y >= .05)
            {
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(0, 0, -10), 5 * Time.deltaTime);

                bc2.enabled = false;
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 4f, 0), 5 * Time.deltaTime);
            }
            else
            {
                Camera.main.transform.position = new Vector3(0, 0, -10);
                bc2.enabled = true;
                goingToHouse = false;
                isInBackyard = false;
            }
        }

        if(Input.GetButtonDown("Stick") && game.canStick && game.canStartGame)
        {
            FindObjectOfType<AudioManager>().Play("Swing");
            stick.GetComponent<Renderer>().enabled = true;
            stick.GetComponent<BoxCollider2D>().enabled = true;
            startStick = true;
        }

        if(startStick)
        {
            
            stick.transform.rotation = Quaternion.Euler(0, 0, stick.transform.rotation.eulerAngles.z + (Time.deltaTime * stickSpeed));
            if(stick.transform.rotation.eulerAngles.z >= 200 || stick.transform.rotation.eulerAngles.z <= -200)
            {
                startStick = false;
                stick.transform.rotation = Quaternion.identity;
                stick.GetComponent<Renderer>().enabled = false;
                stick.GetComponent<BoxCollider2D>().enabled = false;
            }
        }

        if(objectHolding != null && objectHolding.CompareTag("WildSlime"))
        {
            objectHolding.transform.localPosition = Vector3.zero;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isInteracting)
        {
            if(collision.gameObject.CompareTag("Cage"))
            {
                if (objectHolding != null && objectHolding.CompareTag("WildSlime") && !collision.gameObject.GetComponent<CageScript>().isFull)
                {
                    if (objectHolding.GetComponent<WildSlime>().canBeCaged)
                    {
                        FindObjectOfType<AudioManager>().Play("PlaceSeed");
                        collision.gameObject.GetComponent<CageScript>().ChangeAnim(true);
                        game.wildSlimes.Remove(objectHolding);
                        Destroy(objectHolding);
                        objectHolding = null;
                        isInteracting = false;
                    }
                    if (menu.tutorialNum == 8)
                        menu.tutorialNum = 9;
                }
                else if(objectHolding == null && collision.gameObject.GetComponent<CageScript>().isFull)
                {
                    FindObjectOfType<AudioManager>().Play("PlaceSeed");
                    collision.gameObject.GetComponent<CageScript>().ChangeAnim(false);

                    objectHolding = Instantiate(game.wildSlime);
                    objectHolding.transform.SetParent(transform);
                    objectHolding.transform.localPosition *= 0;
                    objectHolding.GetComponent<WildSlime>().Disabled();
                    //objectHolding.GetComponent<WildSlime>().canBeCaged = false;
                    objectHolding.GetComponent<WildSlime>().canBeTested = true;
                    objectHolding.GetComponent<SpriteRenderer>().sortingOrder = 3;
                    objectHolding.GetComponent<BoxCollider2D>().enabled = false;
                    isInteracting = false;
                }
            }

            if(collision.gameObject.CompareTag("TestingTable") && game.canPlaceTable)
            {
                if (objectHolding == null && collision.gameObject.transform.childCount > 0)
                {
                    FindObjectOfType<AudioManager>().Play("PlaceSeed");
                    objectHolding = collision.gameObject.GetComponentsInChildren<Transform>()[1].gameObject;
                    objectHolding.transform.SetParent(transform);
                    objectHolding.transform.position *= 0;
                    objectHolding.GetComponent<WildSlime>().canBeTested = false;
                    objectHolding.GetComponent<WildSlime>().updateTime = false;
                    objectHolding.GetComponent<WildSlime>().timeToLeave = 0;
                    isInteracting = false;
                }

                if (objectHolding != null)
                {
                    if(objectHolding.GetComponent<WildSlime>() != null && objectHolding.GetComponent<WildSlime>().canBeTested)
                    {
                        FindObjectOfType<AudioManager>().Play("PlaceSeed");
                        objectHolding.transform.SetParent(collision.gameObject.transform);
                        objectHolding.transform.localPosition *= 0;
                        objectHolding.GetComponent<WildSlime>().canBeCaged = true;
                        objectHolding.GetComponent<WildSlime>().updateTime = true;
                        objectHolding = null;
                        isInteracting = false;
                        if(menu.tutorialNum == 11)
                            menu.tutorialNum = 12;
                    }
                    else if(objectHolding.GetComponent<SeedScript>() != null && objectHolding.GetComponent<SeedScript>().isPlant)
                    {
                        uimgr.CreateNewItem(objectHolding.GetComponent<SeedScript>());
                        collision.gameObject.GetComponentsInChildren<WildSlime>()[0].timeToLeave = 0;
                        collision.gameObject.GetComponentsInChildren<WildSlime>()[0].FeedMe(objectHolding);
                        FindObjectOfType<AudioManager>().Play("SmallCrunch");
                        Destroy(objectHolding);
                        objectHolding = null;
                        isInteracting = false;
                        if (menu.tutorialNum == 12)
                            menu.tutorialNum = 13;
                    }
                }
            }

            if (collision.gameObject.CompareTag("SlimeBoss"))
            {
                if (objectHolding != null && objectHolding.GetComponent<SeedScript>() != null && objectHolding.GetComponent<SeedScript>().isPlant == true)
                {
                    uimgr.CreateNewItem(objectHolding.GetComponent<SeedScript>());
                    collision.gameObject.GetComponent<SlimeBoss>().FeedMe(objectHolding);
                    Destroy(objectHolding);
                    objectHolding = null;
                    isInteracting = false;
                }
            }

            if (collision.gameObject.CompareTag("Bed") && game.canSleep && objectHolding == null)
            {
                if(!movingToNextDay)
                {
                    FindObjectOfType<AudioManager>().Play("PlaceSeed");
                    FindObjectOfType<AudioManager>().Stop("Walk");
                    movingToNextDay = true;
                    game.MoveToNextDay();

                    GetComponent<BoxCollider2D>().enabled = false;
                    transform.position = collision.transform.position;

                    isInteracting = false;
                }

                if (menu.tutorialNum == 9)
                    menu.tutorialNum = 10;
            }

            if (collision.gameObject.CompareTag("TrashBin"))
            {
                if(objectHolding != null)
                {
                    FindObjectOfType<AudioManager>().Play("Trash");
                    Destroy(objectHolding);
                    objectHolding = null;
                    isInteracting = false;
                    if (menu.tutorialNum == 15)
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                        //Application.LoadLevel(Application.loadedLevel);
                }
            }

            if(collision.gameObject.CompareTag("EnterTheBackyard") && game.canCrossOver)
            {
                goingToBackyard = true;
                isInteracting = false;
                if (menu.tutorialNum == 5)
                {
                    menu.tutorialNum = 6;
                    if(game.wildSlimes.Count == 0)
                    {
                        game.addSlime();
                    }
                }

            }
            else if(collision.gameObject.CompareTag("EnterTheHouse") && game.canCrossBack)
            {
                goingToHouse = true;
                isInteracting = false;

                for (int i = 0; i < game.wildSlimes.Count; i++)
                {
                    game.wildSlimes[i].GetComponent<WildSlime>().isHit = false;
                }
                if (menu.tutorialNum == 7)
                    menu.tutorialNum = 8;
            }

            if(collision.gameObject.CompareTag("WildSlime"))
            {
                if(objectHolding == null && collision.gameObject.GetComponent<WildSlime>().isHit)
                {
                    objectHolding = collision.gameObject;
                    game.wildSlimes.Remove(objectHolding);
                    objectHolding.GetComponent<Collider2D>().enabled = false;
                    objectHolding.transform.SetParent(transform);
                    objectHolding.transform.localPosition *= 0;
                    objectHolding.GetComponent<SpriteRenderer>().sortingOrder = 3;
                    isInteracting = false;

                    if (menu.tutorialNum == 6)
                        menu.tutorialNum = 7;
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isInteracting)
        {
            if (collision.gameObject.CompareTag("PlantSpot"))
            {
                if(!isCurrentlySelecting && objectHolding != null && objectHolding.CompareTag("Seed") && collision.gameObject.transform.childCount == 0)
                {
                    FindObjectOfType<AudioManager>().Play("PlaceSeed");
                    objectHolding.transform.SetParent(collision.gameObject.transform);
                    objectHolding.transform.localPosition *= 0;
                    objectHolding.GetComponent<SeedScript>().canBePickedUp = false;
                    objectHolding.GetComponent<SpriteRenderer>().sortingOrder = 1;
                    objectHolding = null;
                    isInteracting = false;
                    if(menu.tutorialNum == 4)
                    {
                        menu.tutorialNum = 5;
                    }
                }
            }
            
            if(objectHolding == null && collision.gameObject.CompareTag("Seed") && game.canPlaceSeeds &&
                (collision.gameObject.GetComponent<SeedScript>().canBePickedUp || collision.gameObject.GetComponent<SeedScript>().isPlant))
            {
                FindObjectOfType<AudioManager>().Play("PlaceSeed");
                objectHolding = collision.gameObject;
                objectHolding.transform.SetParent(transform);
                objectHolding.transform.localPosition *= 0;
                objectHolding.GetComponent<SpriteRenderer>().sortingOrder = 3;
                isInteracting = false;
            }

            if(collision.gameObject.CompareTag("PlantSpotHolder") && objectHolding != null &&
                !objectHolding.GetComponent<SeedScript>().isPlant && collision.gameObject.transform.childCount == 0)
            {
                FindObjectOfType<AudioManager>().Play("PlaceSeed");
                objectHolding.transform.SetParent(collision.gameObject.transform);
                objectHolding.transform.localPosition *= 0;
                objectHolding.GetComponent<SpriteRenderer>().sortingOrder = 1;
                objectHolding = null;
                isInteracting = false;
            }
        }
    }
}
