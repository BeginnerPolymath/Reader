using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HorizontalScrollView : MonoBehaviour, IDragHandler
{
    public RectTransform ExpContent;
    public RectTransform ContentView;

    public void OnDrag (PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if(ExpContent.rect.width > ContentView.rect.width)
            {
                ExpContent.anchoredPosition += new Vector2(eventData.delta.x * ScreenSizer.DragShiftCoef, 0);

                ExpContent.anchoredPosition = new Vector2(Mathf.Clamp(ExpContent.anchoredPosition.x, ExpContent.rect.width * -0.5f, (ExpContent.rect.width * -0.5f)+ (ExpContent.rect.width - ContentView.rect.width)) , 0);
            }
        }
    }
}
