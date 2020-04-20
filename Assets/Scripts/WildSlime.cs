using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildSlime : MonoBehaviour
{
    public float speed = 4f;

    private float startTimex = 0f, startTimey = 0f;
    private float endx = 0f, endy = 0f;

    private int directionx = 0, directiony = 0;
    public bool isHit = false;

    public bool canBeCaged = true, canBeTested = true;

    public float timeToLeave = 0, timeBeween = 10f;
    public bool updateTime = false;

    public CharacterMovement player;

    public UIManager uimgr;

    public void FeedMe(GameObject plant)
    {
        SeedScript seed = plant.GetComponent<SeedScript>();
        if(seed.isBad)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();
        uimgr = FindObjectOfType<UIManager>();
    }

    public void TimeUp()
    {
        isHit = false;
        updateTime = false;
        timeToLeave = 0;
        GetComponent<BoxCollider2D>().enabled = true;
        transform.parent = null;
    }    

    private void Update()
    {
        if((player.goingToBackyard || player.movingToNextDay) && isHit)
        {
            TimeUp();
        }

        if(updateTime && !uimgr.canTakeNotes)
        {
            if(timeToLeave < timeBeween)
            {
                timeToLeave += Time.deltaTime;
            }
            else
            {
                TimeUp();
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        startTimex += Time.fixedDeltaTime;
        startTimey += Time.fixedDeltaTime;
        if(!isHit && startTimex >= endx)
        {
            startTimex = 0;
            endx = Random.Range(0f, 4f);
            directionx = Random.Range(-1, 2);
            if(directionx == 0)
                directionx = Random.Range(-1, 2);
        }
        if (!isHit && startTimey >= endy)
        {
            startTimey = 0;
            endy = Random.Range(0f, 4f);
            directiony = Random.Range(-1, 2);
            if (directiony == 0)
                directiony = Random.Range(-1, 2);
        }

        if (!isHit)
        {
            transform.position += new Vector3(directionx * speed * Time.fixedDeltaTime, 0, 0);
            transform.position += new Vector3(0, directiony * speed * Time.fixedDeltaTime, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name.Contains("top") || collision.gameObject.name.Contains("bottom"))
        {
            directionx = -directionx;

            if (player.isInBackyard)
                FindObjectOfType<AudioManager>().Play("SlimeBounce");
        }

        if(collision.gameObject.name == "Fence_right" || collision.gameObject.name == "Fence_left")
        {
            directiony = -directiony;

            if(player.isInBackyard)
                FindObjectOfType<AudioManager>().Play("SlimeBounce");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Stick"))
        {
            FindObjectOfType<AudioManager>().Play("SlimeHit");
            Disabled();
        }
    }

    public void Disabled()
    {
        isHit = true;
        directionx = 0;
        directiony = 0;
    }
}
