using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;

public class SettingsScript : MonoBehaviour, IEndDragHandler, IDragHandler
{
    public RectTransform Window;

    public RectTransform Canvas;

    public CanvasScaler canvasss;


    public void OnDrag (PointerEventData eventDataZ)
    {
        OnDrags = true;

        Window.anchoredPosition = new Vector2(eventDataZ.position.x * ScreenSizer.DragShiftCoef, 0);

        Window.anchoredPosition = new Vector2(Mathf.Clamp(Window.anchoredPosition.x, 0, Canvas.rect.width), 0);
    }

    public void OnEndDrag (PointerEventData eventData)
    {
        if(eventData.delta.x < -10 || Window.anchoredPosition.x < Canvas.rect.width / 2)
        {
            if(Window.anchoredPosition.x < Canvas.rect.width / 2 && eventData.delta.x > 10)
            {
                OpenWindow = true;

                time = 0;

                LerpEnd = false;
                OnDrags = false;
            }
            else
            {
                OpenWindow = false;

                time = 0;

                LerpEnd = false;
                OnDrags = false;
            }
        }
        else
        {
            OpenWindow = true;

            time = 0;

            LerpEnd = false;
            OnDrags = false;
        }
        
    }

    float time;

    public bool OnDrags;

    public bool OpenWindow;

    public bool LerpEnd;

    public void Open ()
    {
        OpenWindow = false;

        time = 0;

        LerpEnd = false;
        OnDrags = false;
    }

    public void Close ()
    {
        if(OpenWindow != true)
        {
            LerpEnd = false;
            OpenWindow = true;

            time = 0;
        }
    }

    void Update ()
    {
        if(!OnDrags && !LerpEnd)
        {
            if(OpenWindow)
            {
                time += Time.deltaTime * 5;

                Window.anchoredPosition = new Vector2(Mathf.Lerp(Window.anchoredPosition.x, Canvas.rect.width, time), 0);
            }
            else
            {
                time += Time.deltaTime * 5;

                Window.anchoredPosition = new Vector2(Mathf.Lerp(Window.anchoredPosition.x, 0, time), 0);
            }

            if(time >= 1)
            {
                time = 0;
                LerpEnd = true;
            }

        }
        
    }
}
