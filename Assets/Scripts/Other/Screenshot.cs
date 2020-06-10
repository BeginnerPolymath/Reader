using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    public int ID;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F12))
        {
            ID++;
            ScreenCapture.CaptureScreenshot($"Screenshots/SomeLevel{ID}.png");
        }
    }
}
