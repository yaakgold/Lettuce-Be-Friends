using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Canvas noteCanvas;
    public Canvas seedCanvas;
    public Canvas infoCanvas;

    public bool canTakeNotes = false;
    public bool canSeedInfo = false;

    public List<string> types;
    public List<string> shapes = new List<string> { "Pointy", "Circle", "Lumpy" };
    public List<string> sizes = new List<string> { "Small", "Medium", "Large", "Extra-Large" };
    public List<string> color = new List<string> { "Orange", "Purple", "Green", "Pink", "Yellow" };

    public GameObject checkBox, content, newContent, seed;

    public CharacterMovement player;

    public MainMenu menu;
    public GameOver endGame;

    public GameManager game;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        GameObject panel = noteCanvas.GetComponentInChildren<RawImage>().gameObject;
        GridLayoutGroup grid = panel.GetComponent<GridLayoutGroup>();
        int WIDTH = Camera.main.pixelWidth;

        grid.padding.left = WIDTH / 14;
        grid.padding.top = WIDTH / 28;
        grid.cellSize =  new Vector2(WIDTH / 50, WIDTH / 50);
        grid.spacing = new Vector2(WIDTH / 5, WIDTH / 400);

        grid = seedCanvas.GetComponentInChildren<GridLayoutGroup>();

        grid.padding.left = WIDTH / 13;
        grid.padding.top = WIDTH / 20;
        grid.cellSize = new Vector2(WIDTH / 60, WIDTH / 60);

        for (int i = 0; i < types.Count; i++)
        {
            GameObject obj = Instantiate(checkBox, new Vector3(0, 0), Quaternion.identity, panel.transform);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = types[i].Contains(" ") ? types[i] : "      " + types[i];
            obj.GetComponent<Image>().enabled = !types[i].Contains(" ");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Notes") && !FindObjectOfType<CharacterMovement>().movingToNextDay && FindObjectOfType<GameManager>().canStartGame)
        {
            game.canStick = canTakeNotes;
            canTakeNotes = !canTakeNotes;
            noteCanvas.enabled = canTakeNotes;

            if (menu.tutorialNum == 13)
                menu.tutorialNum = 14;
        }

        if(Input.GetButtonDown("SeedInfo") && player.objectHolding != null)
        {
            canSeedInfo = !canSeedInfo;
            seedCanvas.enabled = canSeedInfo;
            SeedScript seed = player.objectHolding.GetComponent<SeedScript>();
            string seedText = "Seed Shape:  " +     shapes[seed.seedShape];
                   seedText += "\nSeed Size:  " + sizes[seed.size];
                   seedText += "\nSeed Color:  " + color[seed.seedColorNum];
                   seedText += "\nSeed Texture:  " + (seed.smooth ? "Smooth" : "Rough");
                   seedText += "\nSeed Amount:  " + seed.numSeeds;
            seedCanvas.GetComponentInChildren<TextMeshProUGUI>().text = seedText;
            if (menu.tutorialNum == 14)
                menu.tutorialNum = 15;
        }
    }

    public void CreateNewItem(SeedScript plant)
    {
        GameObject nc = Instantiate(newContent, content.transform);
        nc.GetComponent<NewContent>().plant = plant;

        //GameObject ns = Instantiate(seed, nc.GetComponentsInChildren<Image>()[0].transform);
        GameObject ns = Instantiate(seed, nc.transform);
        
    }

    public void goToInfo()
    {
        infoCanvas.enabled = true;
    }

    public void goToSetting()
    {
        infoCanvas.enabled = false;
    }
}
