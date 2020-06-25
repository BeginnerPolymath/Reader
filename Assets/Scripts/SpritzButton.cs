using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class SpritzButton : MonoBehaviour, IDragHandler
{
    public RectTransform rectTransform;

    public RectTransform Canvas;

    public void OnDrag (PointerEventData eventDataZ)
    {

        rectTransform.anchoredPosition += eventDataZ.delta * ScreenSizer.DragShiftCoef;

        rectTransform.anchoredPosition = new Vector2
        (

            Mathf.Clamp(rectTransform.anchoredPosition.x, -(Canvas.rect.width - rectTransform.rect.width) / 2, (Canvas.rect.width - rectTransform.rect.width) / 2)
            , 
            Mathf.Clamp(rectTransform.anchoredPosition.y, -(Canvas.rect.height - rectTransform.rect.height) / 2, (Canvas.rect.height - rectTransform.rect.height) / 2)

        );
    }
}
