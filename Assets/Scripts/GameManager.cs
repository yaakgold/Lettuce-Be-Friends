using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int nightsAlive;

    public SlimeBoss slimeBoss;

    public GameObject panelNight;

    public List<GameObject> plantSpotHolders;
    public List<GameObject> plantSpots;
    public GameObject tempSeed, player, newDayAnim;

    public List<Sprite> roughSeeds, roughPlants;
    public List<Sprite> smoothSeeds, smoothPlants;

    public List<int> badSeedShape, allSeedShapes;           //3 shapes
    public int numBadSeedShape = 1;

    public List<int> badNumSeeds;                           //5 numbers
    public int numBadNumSeed = 5;

    public List<Color> badSeedColor, allSeedColor;          //5 colors
    public int numBadSeedColor = 3;

    public List<int> badSize, allSize;                      //3 sizes
    public int numBadSize = 2;

    public List<bool> badSmooth;                            //2 smooth
    public int badSmoothNum = 1;
   
    public GameObject wildSlime;

    public List<GameObject> wildSlimes;

    public bool canStartGame = false, canCrossOver = true, canSleep = true, canStick = false, canPlaceSeeds = true, canCrossBack = true, canPlaceTable = true;
    public bool isMenuActive = true;

    public Canvas settings, info;

    public void BadSeeds()
    {
        int currentAddIndex = -1;
        int nextAddIndex;

        //Bad Seed Shapes
        for (int i = 0; i < numBadSeedShape; i++)
        {
            do
            {
                nextAddIndex = Random.Range(0, allSeedShapes.Count);
            } while (nextAddIndex == currentAddIndex);

            badSeedShape.Add(allSeedShapes[nextAddIndex]);
            currentAddIndex = nextAddIndex;
        }

        currentAddIndex = -1;

        //Bad Num Seeds
        numBadNumSeed = 5;
        badNumSeeds.Add(Random.Range(1, numBadNumSeed));

        //Bad Seed Colors
        for (int i = 0; i < numBadSeedColor; i++)
        {
            do
            {
                nextAddIndex = Random.Range(0, allSeedColor.Count);
            } while (nextAddIndex == currentAddIndex);

            badSeedColor.Add(allSeedColor[nextAddIndex]);
            currentAddIndex = nextAddIndex;
        }

        currentAddIndex = -1;

        //Bad Seed Sizes
        for (int i = 0; i < numBadSize; i++)
        {
            do
            {
                nextAddIndex = Random.Range(0, allSize.Count);
            } while (nextAddIndex == currentAddIndex);

            badSize.Add(allSize[nextAddIndex]);
            currentAddIndex = nextAddIndex;
        }

        //Bad Smooth
        badSmooth.Add(Random.Range(0, badSmoothNum) == 1);



        SpawnNewSeeds(12);
    }

    // Start is called before the first frame update
    void Start()
    {
        nightsAlive = 0;

        for(int i = 0; i < Random.Range(0, 4); i++)
            wildSlimes.Add(Instantiate(wildSlime, new Vector3(-3, 10), Quaternion.identity));
    }

    private void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            pauseGame();
        }
    }

    public void pauseGame()
    {
        if(!isMenuActive)
            canStartGame = !canStartGame;
        settings.enabled = !settings.enabled;

        if (info.enabled)
            info.enabled = false;

    }

    public void SpawnNewSeeds(int numMax)
    {
        int numSpawned = 0;
        for (int i = 0; i < plantSpotHolders.Count; i++)
        {
            if(plantSpotHolders[i].transform.childCount == 0 && numSpawned <= numMax)
            {
                GameObject seed = Instantiate(tempSeed);
                seed.transform.SetParent(plantSpotHolders[i].transform);
                seed.transform.localPosition *= 0;
                numSpawned++;
            }
        }

        //FindObjectOfType<AudioManager>().Stop("MusicNight");
        FindObjectOfType<AudioManager>().Play("Music", 1);
    }

    public void PlayerUpdate(bool state)
    {
        player.GetComponent<CharacterMovement>().enabled = state;

        if (state)
        {
            player.transform.position = new Vector3(player.transform.position.x + 1.5f, player.transform.position.y, player.transform.position.z);
            player.GetComponent<CharacterMovement>().movingToNextDay = false;
            player.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    public void MoveToNextDay()
    {
        PlayerUpdate(false);

        int numSlimes = Random.Range(0, 4);
        
        while(wildSlimes.Count < numSlimes)
        {
            wildSlimes.Add(Instantiate(wildSlime, new Vector3(-3, 10), Quaternion.identity));
        }
        
        while(wildSlimes.Count > numSlimes)
        {
            Destroy(wildSlimes[0]);
            wildSlimes.RemoveAt(0);
        }

        slimeBoss.MoveToNextDay();

        newDayAnim.GetComponent<Animator>().SetBool("StartNewDayAnim", true);

        nightsAlive++;
    }

    public void GrowPlants()
    {
        for (int i = 0; i < plantSpots.Count; i++)
        {
            if (plantSpots[i].transform.childCount > 0)
            {
                plantSpots[i].GetComponentsInChildren<SpriteRenderer>()[1].sprite = plantSpots[i].GetComponentInChildren<SeedScript>().plantSprite;
                plantSpots[i].GetComponentsInChildren<Transform>()[1].localScale = new Vector3(.3f, .3f);
                plantSpots[i].GetComponentInChildren<SeedScript>().isPlant = true;
            }
        }
    }

    public void addSlime()
    {
        wildSlimes.Add(Instantiate(wildSlime, new Vector3(-3, 10), Quaternion.identity));
    }
}
