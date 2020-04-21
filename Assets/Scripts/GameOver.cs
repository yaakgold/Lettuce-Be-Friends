using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI daysSurviveText, badAspects;

    public List<Color> colors;

    public void EndGame(int daysSurvived, GameManager game)
    {
        GetComponentInChildren<Canvas>().enabled = true;

        badAspects.text = "Bad Aspects:\n" + GetBadAspectsText(game);

        daysSurviveText.text = "Nights Survived: " + daysSurvived;
    }

    public string GetBadAspectsText(GameManager game)
    {
        for (int i = 0; i < game.allSeedColor.Count; i++)
        {
            colors.Add(game.allSeedColor[i]);

        }

        string retString = "Shape(s): ";
        for (int i = 0; i < game.badSeedShape.Count; i++)
        {
            switch (game.allSeedShapes[game.badSeedShape[i]])
            {
                case 0:
                    retString += "Pointy";
                    break;
                case 1:
                    retString += "Circle";
                    break;
                case 2:
                    retString += "Lumpy";
                    break;
            }
            retString += ", ";
        }

        retString += "; Color(s): ";

        for (int i = 0; i < game.badSeedColor.Count; i++)
        {
            if (game.badSeedColor[i].CompareRGB(colors[0]))
            {
                retString += "Orange";
            }
            else if (game.badSeedColor[i].CompareRGB(colors[1]))
            {
                retString += "Purple";
            }
            else if (game.badSeedColor[i].CompareRGB(colors[2]))
            {
                retString += "Green";
            }
            else if (game.badSeedColor[i].CompareRGB(colors[3]))
            {
                retString += "Pink";
            }
            else if (game.badSeedColor[i].CompareRGB(colors[4]))
            {
                retString += "Yellow";
            }
            
            retString += ", ";
        }

        retString += "; Amount: " + game.badNumSeeds[0] + ", ";


        retString += "; Size(s): ";
        for (int i = 0; i < game.badSize.Count; i++)
        {
            switch (game.allSize[game.badSize[i]])
            {
                case 0:
                    retString += "Small";
                    break;
                case 1:
                    retString += "Medium";
                    break;
                case 2:
                    retString += "Large";
                    break;
                case 3:
                    retString += "Extra-Large";
                    break;
            }
            retString += ", ";
        }

        retString += "; Texture(s): ";
        for (int i = 0; i < game.badSmooth.Count; i++)
        {
            switch (game.badSmooth[i])
            {
                case true:
                    retString += "Smooth";
                    break;
                case false:
                    retString += "Rough";
                    break;
            }
            retString += ", ";
        }

        return retString;
    }

    public void OnStartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnExitGame()
    {
        Application.Quit();
    }
}
