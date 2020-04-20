using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public GameObject robber;

    public GameManager game;
    public SlimeBoss slimeBoss;

    public void ChangeToNight()
    {
        FindObjectOfType<AudioManager>().Stop("Music");
        FindObjectOfType<AudioManager>().Stop("MusicNight");
        FindObjectOfType<AudioManager>().Play("MusicNight");
        game.panelNight.SetActive(true);
    }

    public void StartRobberAnim()
    {
        if(slimeBoss.health == 2)
        {
            if(slimeBoss.hunger > 2.5)
            {
                robber.GetComponent<Animator>().SetBool("StartNoRob", true);
            }
            else
            {
                robber.GetComponent<Robber>().MoveRobberInit();
            }
        }
        else if(slimeBoss.health == 1)
        {
            if (slimeBoss.hunger >= 5)
            {
                robber.GetComponent<Animator>().SetBool("StartNoRob", true);
            }
            else
            {
                robber.GetComponent<Robber>().MoveRobberInit();
            }
        }
        else if(slimeBoss.health == 0)
        {
            if (slimeBoss.hunger >= 7.5)
            {
                robber.GetComponent<Animator>().SetBool("StartNoRob", true);
            }
            else if(slimeBoss.hunger <= 1)
            {
                robber.GetComponent<Robber>().StealPlayer();
            }
            else
            {
                robber.GetComponent<Robber>().MoveRobberInit();
            }
        }
        GetComponent<Animator>().SetBool("StartNewDayAnim", false);
    }
}
