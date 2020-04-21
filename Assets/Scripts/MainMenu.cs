using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameManager game;
    public Canvas titleScreen;
    public Canvas tutorial;

    public Image panel1, panel2, panel3, panel4;
    public Text text5, text6, text7, text8, text9, text10, text11, text12, text13, text14, text15;
    
    private TextMeshProUGUI[] tempTText;
    private Image[] tempImage;

    public int tutorialNum = 0;

    public Image background1, background2;

    public bool hardMode = false, gameHasStared = false;

    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    public void Exit()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Stick"))
        {
            if(tutorialNum == 1)
            {
                tempImage = panel1.GetComponentsInChildren<Image>();
                for (int i = 0; i < tempImage.Length; i++)
                    tempImage[i].enabled = false;
                tempTText = panel1.GetComponentsInChildren<TextMeshProUGUI>();
                for (int i = 0; i < tempTText.Length; i++)
                    tempTText[i].enabled = false;

                tempImage = panel2.GetComponentsInChildren<Image>();
                for (int i = 0; i < tempImage.Length; i++)
                    tempImage[i].enabled = true;
                tempTText = panel2.GetComponentsInChildren<TextMeshProUGUI>();
                for (int i = 0; i < tempTText.Length; i++)
                    tempTText[i].enabled = true;

                tutorialNum++;
            }
            else if(tutorialNum == 2)
            {
                tempImage = panel2.GetComponentsInChildren<Image>();
                for (int i = 0; i < tempImage.Length; i++)
                    tempImage[i].enabled = false;
                tempTText = panel2.GetComponentsInChildren<TextMeshProUGUI>();
                for (int i = 0; i < tempTText.Length; i++)
                    tempTText[i].enabled = false;

                tempImage = panel3.GetComponentsInChildren<Image>();
                for (int i = 0; i < tempImage.Length; i++)
                    tempImage[i].enabled = true;
                tempTText = panel3.GetComponentsInChildren<TextMeshProUGUI>();
                for (int i = 0; i < tempTText.Length; i++)
                    tempTText[i].enabled = true;

                tutorialNum++;
            }
            else if (tutorialNum == 3)
            {
                tempImage = panel3.GetComponentsInChildren<Image>();
                for (int i = 0; i < tempImage.Length; i++)
                    tempImage[i].enabled = false;
                tempTText = panel3.GetComponentsInChildren<TextMeshProUGUI>();
                for (int i = 0; i < tempTText.Length; i++)
                    tempTText[i].enabled = false;

                tempImage = panel4.GetComponentsInChildren<Image>();
                for (int i = 0; i < tempImage.Length; i++)
                    tempImage[i].enabled = true;
                tempTText = panel4.GetComponentsInChildren<TextMeshProUGUI>();
                for (int i = 0; i < tempTText.Length; i++)
                    tempTText[i].enabled = true;

                tutorialNum++;
            }
            else if (tutorialNum == 4)
            {
                tempImage = panel4.GetComponentsInChildren<Image>();
                for (int i = 0; i < tempImage.Length; i++)
                    tempImage[i].enabled = false;
                tempTText = panel4.GetComponentsInChildren<TextMeshProUGUI>();
                for (int i = 0; i < tempTText.Length; i++)
                    tempTText[i].enabled = false;

                text5.enabled = true;
                game.canStartGame = true;
                background1.enabled = true;
            }
        }
        if(tutorialNum == 5)
        {
            text5.enabled = false;
            text6.enabled = true;

            background1.enabled = false;
            background2.enabled = true;

            game.canPlaceSeeds = false;
            game.canCrossOver = true;
        }
        else if (tutorialNum == 6)
        {
            text6.enabled = false;
            text7.enabled = true;

            background2.enabled = false;
            background1.enabled = true;
            //background1.color = new Color(255, 255, 255, 200);

            game.canPlaceSeeds = false;
            game.canCrossBack = false;
            game.canStick = true;
        }
        else if (tutorialNum == 7)
        {
            text7.enabled = false;
            text8.enabled = true;

            //background1.color = new Color(255, 255, 255, 255);

            game.canCrossBack = true;
            //game.canPlaceTable = false;
        }
        else if (tutorialNum == 8)
        {
            text8.enabled = false;
            text9.enabled = true;

            background1.enabled = false;
            background2.enabled = true;

            game.canCrossOver = false;
            game.canCrossBack = true;
        }
        else if (tutorialNum == 9)
        {
            text9.enabled = false;
            text10.enabled = true;

            background2.enabled = false;
            background1.enabled = true;

            game.canSleep = true;
        }
        else if (tutorialNum == 10)
        {
            text10.enabled = false;

            background1.enabled = false;
        }
        else if (tutorialNum == 11)
        {
            text11.enabled = true;

            background2.enabled = true;

            game.canPlaceTable = true;
            game.canSleep = false;
        }
        else if (tutorialNum == 12)
        {
            text11.enabled = false;
            text12.enabled = true;

            game.canPlaceSeeds = true;
        }
        else if (tutorialNum == 13)
        {
            text12.enabled = false;
            text13.enabled = true;
        }
        else if (tutorialNum == 14)
        {
            text13.enabled = false;
            text14.enabled = true;
        }
        else if (tutorialNum == 15)
        {
            text14.enabled = false;
            text15.enabled = true;

            background1.enabled = true;
            background2.enabled = false;
        }
    }

    public void startGame()
    {
        gameHasStared = true;

        titleScreen.enabled = false;
        game.canStartGame = true;
        game.canCrossOver = true;
        game.canSleep = true;
        game.canStick = true;
        game.canPlaceSeeds = true;
        game.canCrossBack = true;
        game.canPlaceTable = true;
        game.isMenuActive = false;

        if (hardMode)
        {
            int number = -1;// Random.Range(0, 5);

            game.numBadSeedShape = (number == 0 ? 3 : Random.Range(1, 3));

            //game.numBadNumSeed = (number == 1 ? 5 : Random.Range(2, 5));

            game.numBadSeedColor = (number == 2 ? 5 : Random.Range(2, 5));

            game.numBadSize = (number == 3 ? 3 : Random.Range(2, 4));

            game.badSmoothNum = (number == 4 ? 3 : Random.Range(1, 3));

            game.BadSeeds();
        }
        else
            game.BadSeeds();
    }

    public void startTutorial()
    {
        game.canCrossOver = false;
        game.canSleep = false;
        game.canStick = false;
        game.isMenuActive = false;
        game.SpawnNewSeeds(12);

        titleScreen.enabled = false;
        tutorial.enabled = true;
        tutorialNum = 1;
        tempImage = panel1.GetComponentsInChildren<Image>();
        for (int i = 0; i < tempImage.Length; i++)
            tempImage[i].enabled = true;
        tempTText = panel1.GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < tempTText.Length; i++)
            tempTText[i].enabled = true;
    }
}
