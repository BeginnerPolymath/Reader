using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetColorGradient : MonoBehaviour
{
    public TMP_ColorGradient ColorGradient;


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            ColorGradient.bottomLeft = Color.green;
            ColorGradient.bottomRight = Color.green;

            ColorGradient.topLeft = Color.green;
            ColorGradient.topRight = Color.green;
        }
    }


}
