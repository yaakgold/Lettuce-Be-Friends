using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CheckScript : MonoBehaviour
{
    public List<Sprite> checks;

    public int checkVal = 1;

    public bool isInMenu = false;

    public TextMeshProUGUI hard;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onPress()
    {
        if(!isInMenu)
        {
            GetComponent<Image>().sprite = checks[checkVal];

            checkVal = ++checkVal < checks.Count ? checkVal++ : 0;
        }
        else
        {
            GetComponent<Image>().sprite = checks[checkVal];

            checkVal = ++checkVal < 2 ? checkVal++ : 0;


        }

    }

    public void ChangeHardMode()
    {
        FindObjectOfType<MainMenu>().hardMode = !FindObjectOfType<MainMenu>().hardMode;

        hard.color = (FindObjectOfType<MainMenu>().hardMode ? Color.red : Color.white);
    }
}
