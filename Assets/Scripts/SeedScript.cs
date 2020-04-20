using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedScript : MonoBehaviour
{
    public GameManager game;

    public bool canBePickedUp = true, isPlant = false;

    public CharacterMovement player;

    public int seedShape;
    public int numSeeds;
    public Color seedColor;
    public int seedColorNum;
    public int size;
    public bool smooth;
    public int next2;
    public int next3;
    public int next4;

    public bool isBad;
    public bool hasTop = false;

    public float feedAmount;

    public Sprite plantSprite;

    public GameObject emptySeed;
    public Sprite top;

    public UIManager uimgr;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();

        uimgr = GameObject.FindGameObjectWithTag("UIMGR").GetComponent<UIManager>();

        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        seedShape = game.allSeedShapes[Random.Range(0, game.allSeedShapes.Count)];

        numSeeds = Random.Range(1, game.numBadNumSeed);

        seedColorNum = Random.Range(0, game.allSeedColor.Count);
        seedColor = game.allSeedColor[seedColorNum];

        size = game.allSize[Random.Range(0, game.allSize.Count)];
        gameObject.transform.localScale = new Vector3(size * .05f + .05f, size * .05f + .05f, 1);

        smooth = Random.Range(0, 2) == 1;

        feedAmount = Random.Range(.5f, 5f);
        
        isSeedBad();

        GetComponent<SpriteRenderer>().sprite = smooth ? game.smoothSeeds[seedShape] : game.roughSeeds[seedShape];
        plantSprite = smooth ? game.smoothPlants[seedShape] : game.roughPlants[seedShape];
        GetComponent<SpriteRenderer>().color = seedColor;

        for (int i = 0; i < numSeeds - 1; i++)
        {
            GameObject obj = Instantiate(emptySeed, transform);
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.transform.localPosition = new Vector3((i < 2 ? -1.5f : 1.5f), (i % 2 == 1 ? -1.5f : 1.5f));
            obj.GetComponent<SpriteRenderer>().sprite = smooth ? game.smoothSeeds[seedShape] : game.roughSeeds[seedShape];
            obj.GetComponent<SpriteRenderer>().color = seedColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.childCount > 0 && isPlant)
        {
            Transform[] temp = GetComponentsInChildren<Transform>();
            for(int i = 1; i < temp.Length; i++)
            {
                if (!temp[i].CompareTag("TopVeg"))
                {
                    temp[i].gameObject.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
                    temp[i].localPosition = new Vector3((i < 2 ? -1f : 1f), (i % 2 == 1 ? -1f : 1f));
                }
            }
        }
        if(!hasTop && isPlant && seedShape == 0)
        {
            GameObject obj = Instantiate(emptySeed, transform);
            obj.transform.localPosition = Vector3.zero;
            //obj.transform.localScale = new Vector3(1, 1, 1);
            obj.GetComponent<SpriteRenderer>().sprite = top;
            obj.GetComponent<SpriteRenderer>().sortingOrder = 3;
            hasTop = true;
            obj.tag = "TopVeg";
        }
        if (isPlant && ((player.objectHolding != null && player.objectHolding!= this.gameObject) || player.objectHolding == null))
            gameObject.transform.localScale = new Vector3((size+1) * .1f, (size+1) * .1f, 1);
        else if(isPlant && player.objectHolding != null && player.objectHolding == this.gameObject)
            gameObject.transform.localScale = new Vector3(1 * (size + 1)/2f, 1 * (size + 1) / 2f, 1);
    }

    private void isSeedBad()
    {
        float howBad = 0.0f;

        if (game.badSeedShape.Contains(seedShape))
        {
            howBad += 1.6f;
            Debug.Log(howBad);
        }
        if(game.badNumSeeds.Contains(numSeeds))
        {
            howBad += .8f;
        }
        if(game.badSeedColor.Contains(seedColor))
        {
            howBad += 1.4f;
        }
        if(game.badSize.Contains(size))
        {
            howBad += .5f;
        }
        if(game.badSmooth.Contains(smooth))
        {
            howBad += .3f;
        }

        isBad = howBad > 2f;
        Debug.Log(isBad);
    }
}
