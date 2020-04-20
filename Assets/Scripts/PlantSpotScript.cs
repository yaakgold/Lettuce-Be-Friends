using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSpotScript : MonoBehaviour
{
    public Color c;

    public bool keepChanging = true;

    public SpriteRenderer sr;

    void Update()
    {
        if(transform.childCount > 0)
        {
            keepChanging = true;
        }

        if(keepChanging)
        {
            sr.color = Color.Lerp(sr.color, c, 5f * Time.deltaTime);
        }
        
        if(transform.childCount == 0 && 
            ((Mathf.Abs(sr.color.r - Color.white.r) <= 5) || (Mathf.Abs(sr.color.g - Color.white.g) <= 5) ||
            (Mathf.Abs(sr.color.b - Color.white.b) <= 5) || (Mathf.Abs(sr.color.a - Color.white.a) <= 5)))
        {
            sr.color = Color.Lerp(sr.color, Color.white, 5f * Time.deltaTime);
        }

        if(keepChanging && 
            (Mathf.Abs(sr.color.r - c.r) <= 5) || (Mathf.Abs(sr.color.g - c.g) <= 5) || 
            (Mathf.Abs(sr.color.b - c.b) <= 5) || (Mathf.Abs(sr.color.a - c.a) <= 5))
        {
            keepChanging = false;
        }
    }
}
