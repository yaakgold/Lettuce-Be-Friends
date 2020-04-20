using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public GameObject[] gos;

    public bool disabled = false;

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Update()
    {
        if (FindObjectOfType<MainMenu>().gameHasStared && !disabled)
        {
            for (int i = 0; i < gos.Length; i++)
            {
                gos[i].SetActive(false);
            }
            disabled = true;
        }
        else if(!FindObjectOfType<MainMenu>().gameHasStared)
        {
            for (int i = 0; i < gos.Length; i++)
            {
                gos[i].SetActive(true);
            }
            disabled = false;
        }
    }
}
