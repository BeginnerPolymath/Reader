using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollView : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform BookTextView;
    public RectTransform SpritzWindow;

    public SpritzScript Spritz;

    public bool ChangePage = true;
    public bool OnScroll;
    public bool BeginScroll;

    public RectTransform canvas;

    public void OnDrag (PointerEventData eventDataZ)
    {
        if(ChangePage && eventDataZ.button == PointerEventData.InputButton.Left)
        {
            Vector2 eventData = eventDataZ.delta * ScreenSizer.DragShiftCoef;

            if(!ZoomStart)
            {
                BeginScroll = true;
                BookTextView.anchoredPosition += new Vector2(0, eventData.y);

                // BookTextView.anchoredPosition = new Vector2(0, Mathf.Clamp(BookTextView.anchoredPosition.y, -(SpritzWindow.rect.height / 2) + SpritzWindow.anchoredPosition.y, BookTextView.rect.height - Screen.height + SpritzWindow.rect.height));

                BookTextView.anchoredPosition = new Vector2(0, Mathf.Clamp(BookTextView.anchoredPosition.y, -canvas.rect.height + 70, BookTextView.sizeDelta.y - 130));
            }

            if(eventData.y > 20)
            {
                OnScroll = true;
            }
            else if(eventData.y < -20)
            {
                OnScroll = true;
            }


            if(eventData.x > 50 && !OnScroll && !ZoomStart)
            {
                if(Spritz.PageID != 0)
                {  
                    Spritz.SetPage(Spritz.PageID - 1);
                    ChangePage = false;
                }
                    
            }
            else if(eventData.x < -50 && !OnScroll && !ZoomStart)
            {
                if(Spritz.PageID != Spritz.Pages.Count - 1)
                {  
                    Spritz.SetPage(Spritz.PageID + 1);
                    ChangePage = false;
                }
            }

            if(ZoomStart)
            {
                if(!BeginScroll)
                {
                    wHTMP.ZoomWord(eventDataZ);
                }
            }
        }
    }

    public WHTMP wHTMP;

    public GameObject WordZoomGO;

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            ChangePage = true;

            ZoomBegin = true;
        }
    }


    void Update ()
    {
        if(!BeginScroll && ZoomBegin && !ZoomStart)
        {
            ZoomTimer ();
        }
        else
        {

        }

        

    }

    public bool ZoomBegin;

    public bool ZoomStart;

    public float TimerDelta;
    public float Timer = 2;

    void ZoomTimer ()
    {
        TimerDelta += Time.deltaTime;

        if(TimerDelta >= Timer)
        {
            WordZoomGO.SetActive(true);
            ZoomStart = true;
            TimerDelta = 0;
        }
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if(!BeginScroll)
                wHTMP.OnPointerClick(eventData);
            
            BeginScroll = false;
            OnScroll = false;   

            WordZoomGO.SetActive(false);

            ZoomBegin = false;
            ZoomStart = false;
            TimerDelta = 0;
        }
    }
}
