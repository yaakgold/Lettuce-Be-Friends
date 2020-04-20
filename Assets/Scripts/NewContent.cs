using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewContent : MonoBehaviour
{
    public SeedScript plant;
    public TextMeshProUGUI text;

    public string[] seedShapes = {"Pointy", "Circle", "Lumpy" }, seedSize = { "Small", "Medium", "Large", "Extra-Large" }, feel = {"Rough", "Smooth"},
                        colorNames = {"Orange", "Purple", "Green", "Pink", "Yellow"};

    private void Start()
    {
        text.text = "Shape:\t" + seedShapes[plant.seedShape] + "\t" +
                    "Number of Seeds: " + plant.numSeeds + "\n" +
                    "Color:\t" + colorNames[plant.seedColorNum] + "\t" +
                    "Size:\t" + seedSize[plant.size] + "\n" +
                    "Feel:\t" + feel[plant.smooth == true ? 1 : 0] + "\t" +
                    "Is Poison:\t" + (plant.isBad ? "Yes" : "No") + "\n";

    }

}
