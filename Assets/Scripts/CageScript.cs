using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageScript : MonoBehaviour
{
    public Animator anim;

    public bool isFull = false;

    public void ChangeAnim(bool state)
    {
        anim.SetBool("SlimeIsInCage", state);
        isFull = state;
    }
}
