using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISeed : MonoBehaviour
{
    public GameManager game;

    public bool canBePickedUp = true, isPlant = false;

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



    public SeedScript seed;

    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        seed = GetComponentInParent<NewContent>().plant;

        seedShape = seed.seedShape;

        numSeeds = seed.numSeeds;

        seedColorNum = seed.seedColorNum;
        seedColor = seed.seedColor;

        size = seed.size;

        gameObject.transform.localScale = Vector3.one;

        smooth = seed.smooth;

        GetComponent<Image>().sprite = smooth ? game.smoothSeeds[seedShape] : game.roughSeeds[seedShape];

        GetComponent<Image>().color = seedColor;
    }
}
