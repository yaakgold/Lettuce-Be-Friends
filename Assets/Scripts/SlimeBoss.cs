using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlimeBoss : MonoBehaviour
{
    public int health;
    public float hunger, maxHunger;

    public Image heart1, heart2, handleScroll;
    public Scrollbar hungerBar;
    public Sprite fullHeart, emptyHeart;

    public GameManager game;

    private bool isSad = false;

    public float timeSinceUpdate = 0, timeForUpdate = 2;

    private void Start()
    {
        hunger = maxHunger;
        hungerBar.size = 1;

        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    public void FeedMe(GameObject plant)
    {
        SeedScript currentSeed = plant.GetComponent<SeedScript>();

        if(currentSeed.isBad)
        {
            FindObjectOfType<AudioManager>().Play("LongCrunch");
            isSad = true;
            hunger = hunger - currentSeed.feedAmount > 0 ? hunger - currentSeed.feedAmount : 0;
            health = health - 1 > 0 ? health - 1 : 0;
            UpdateHearts();

            GetComponent<Animator>().SetBool("sad", true);
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("ThreeCrunch");
            hunger = (currentSeed.feedAmount + hunger > maxHunger ? maxHunger : currentSeed.feedAmount + hunger);
        }

        hungerBar.size = hunger / maxHunger;

        CheckLiving();
    }

    private void Update()
    {
        if(isSad)
        {
            if(timeSinceUpdate < timeForUpdate)
            {
                timeSinceUpdate += Time.deltaTime;
            }
            else
            {
                timeSinceUpdate = 0;
                isSad = false;
                GetComponent<Animator>().SetBool("sad", false);
            }
        }
    }

    public void Grr()
    {
        FindObjectOfType<AudioManager>().Play("Grr");
    }

    private void UpdateHearts()
    {
        switch (health)
        {
            case 2:
                heart1.sprite = fullHeart;
                heart2.sprite = fullHeart;
                break;
            case 1:
                heart1.sprite = emptyHeart;
                heart2.sprite = fullHeart;
                break;
            case 0:
                heart1.sprite = emptyHeart;
                heart2.sprite = emptyHeart;
                break;
        }
    }

    public void MoveToNextDay()
    {
        float num = Random.Range(.1f, .7f);

        hunger -= (num * game.nightsAlive > 1 ? 1 : num * game.nightsAlive) * (4 - health);

        if (hunger < 0)
        {
            hunger = 0;
            health = health - 1 > 0 ? health - 1 : 0;
        }

        hungerBar.size = hunger / maxHunger;

        UpdateHearts();
    }

    private void CheckLiving()
    {
        if(hunger <= 0)
        {
            handleScroll.enabled = false;
            health--;
        }
        else
        {
            handleScroll.enabled = true;
        }

        if(health <= 0)
        {
            health = 0;
            UpdateHearts();
        }
    }
}
