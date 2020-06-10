using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateTestScript : MonoBehaviour
{
    public Image[] Images;

    public bool Yeah;

    public int i;

    public float aga;

    void FixedUpdate()
    {
        aga = Time.fixedDeltaTime;
        if(Yeah)
        {
            Images[i].color = Color.green;
            if(i < Images.Length - 1)
                i++;
        }
    }
}
