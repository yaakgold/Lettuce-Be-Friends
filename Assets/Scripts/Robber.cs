using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robber : MonoBehaviour
{
    public GameObject slime;

    public Transform initPos;
    public Vector3 posInitial, secondPosition, thirdPosition;

    public GameObject[] seedHolders;

    public float speed = 3;

    private int index = 5;
    private bool canMove = false, doneMoving = false, doneSteal, endGame = false;

    public GameManager game;
    public MainMenu menu;

    private int animIndex = 0;

    private void Start()
    {
        posInitial = initPos.position;

        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    public void StartNoRobSlime()
    {
        slime.GetComponent<Animator>().SetBool("mad", true);
    }

    public void EndSlime()
    {
        slime.GetComponent<Animator>().SetBool("mad", false);

        if (menu.tutorialNum == 10)
            menu.tutorialNum = 11;
    }

    public void MoveRobberInit()
    {
        doneMoving = false;
        GetComponent<Animator>().enabled = false;
        posInitial = initPos.position;
    }

    public void MoveRobber()
    {
        while(!canMove)
        {
            index = Random.Range(0, seedHolders.Length);
            if (seedHolders[index].transform.childCount > 0)
            {
                canMove = true;
            }
        }        
    }

    public void StealPlayer()
    {
        GetComponent<Animator>().enabled = false;
        endGame = true;
    }

    public void GrabPlayer()
    {
        game.player.transform.SetParent(transform);
        game.player.transform.localPosition *= 0;
    }

    public void StealSeed()
    {
        seedHolders[index].GetComponentsInChildren<Transform>()[1].SetParent(transform);
        GetComponentsInChildren<Transform>()[1].localPosition *= 0;
        GetComponentsInChildren<SpriteRenderer>()[1].sortingOrder = 3;
        doneSteal = true;
    }

    private void Update()
    {
        if(endGame)
        {
            if (animIndex == 0 && Mathf.Abs(transform.position.x - initPos.position.x) > .05f)
            {
                transform.position = Vector3.Lerp(transform.position, initPos.position, speed * Time.deltaTime);
            }
            else if (animIndex == 0 && Mathf.Abs(transform.position.x - initPos.position.x) <= .05f)
            {
                animIndex = 1;
            }
            
            if (animIndex == 1 && Mathf.Abs(transform.position.x - secondPosition.x) > .05f)
            {
                transform.position = Vector3.Lerp(transform.position, secondPosition, speed * Time.deltaTime);
            }
            else if (animIndex == 1 && Mathf.Abs(transform.position.x - secondPosition.x) <= .05f)
            {
                GrabPlayer();
                animIndex = 2;
            }

            if (animIndex == 2 && Mathf.Abs(transform.position.x - thirdPosition.x) > 5)
            {
                transform.position = Vector3.Lerp(transform.position, thirdPosition, (speed / 4) * Time.deltaTime);
            }
            else if (animIndex == 2 && Mathf.Abs(transform.position.x - thirdPosition.x) <= 5)
            {
                animIndex = 3;
            }

            if(animIndex == 3)
            {
                game.player.GetComponent<CharacterMovement>().uimgr.endGame.EndGame(game.nightsAlive);
            }
        }
        else if(!doneMoving)
        {
            if (canMove && Mathf.Abs(transform.position.x - seedHolders[index].transform.position.x) > .05f)
            {
                transform.position = Vector3.Lerp(transform.position, seedHolders[index].transform.position, speed * Time.deltaTime);
            }
            else if (!canMove && posInitial != Vector3.zero && Mathf.Abs(transform.position.x - posInitial.x) > .05f)
            {
                transform.position = Vector3.Lerp(transform.position, posInitial, speed * Time.deltaTime);
            }
            else if (Mathf.Abs(transform.position.x - posInitial.x) <= .05f)
            {
                posInitial = Vector3.zero;
                MoveRobber();
            }
            else
            {
                canMove = false;
                doneMoving = true;
                StealSeed();
            }
        }
        else if(doneSteal)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(16, transform.position.y, transform.position.z), speed * Time.deltaTime);
        }
        
        if(doneSteal && Mathf.Abs(transform.position.x - 16) <= .5f)
        {
            doneSteal = false;
            Destroy(GetComponentsInChildren<Transform>()[1].gameObject);
            StartNextDay();
        }
    }
    public void StartNextDay()
    {
        
        game.GrowPlants();
        game.SpawnNewSeeds(3);
        game.PlayerUpdate(true);
        game.panelNight.SetActive(false);

        GetComponent<Animator>().enabled = true;
        GetComponent<Animator>().SetBool("StartNoRob", false);
    }
}
