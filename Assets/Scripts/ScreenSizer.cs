using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSizer : MonoBehaviour
{
    public RectTransform Sizer;

    float sizeScreen;

    public Dictionary<float, Vector2> ScreenRatio = new Dictionary<float, Vector2>();

    public static float DragShiftCoef;

    void Start()
    {
        // ScreenRatio.Add((float)16/9, new Vector2(1080, 1920));
        // ScreenRatio.Add((float)18/9, new Vector2(1080, 2160));
        // ScreenRatio.Add((float)18.5f/9, new Vector2(1080, 2220));
        // ScreenRatio.Add((float)19/9, new Vector2(1080, 2280));
        // ScreenRatio.Add((float)19.5f/9, new Vector2(1080, 2340));

        // //print((float)Screen.height / (float)Screen.width);
        // Sizer.sizeDelta = ScreenRatio[(float)Screen.height / (float)Screen.width];

        // sizeScreen = Screen.width / Sizer.rect.width;
        // //print(sizeScreen);

        DragShiftCoef = Sizer.rect.width / (float)Screen.width;

        // //print(Screen.height / 1920);
        // //Sizer.localScale = new Vector2(sizeScreen, (float)Screen.height / (float)1920);

        // Sizer.localScale = new Vector2(sizeScreen, sizeScreen);

        // Application.targetFrameRate = 60;

        
        //     Sizer.localScale = new Vector2(sizeScreen, sizeScreen);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
