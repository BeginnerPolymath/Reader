using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveSpritzWindow : MonoBehaviour, IDragHandler
{
    public RectTransform SpritzWindow;

    public RectTransform Canvas;

    public void OnDrag (PointerEventData pointerEvent)
    {
        SpritzWindow.anchoredPosition += new Vector2(0, pointerEvent.delta.y * ScreenSizer.DragShiftCoef);

        Vector2 SpritzWindowClamp = new Vector2(0, Mathf.Clamp(SpritzWindow.anchoredPosition.y, -Canvas.rect.height + (SpritzWindow.sizeDelta.y / 2), 0 - (SpritzWindow.sizeDelta.y / 2)));
        
        SpritzWindow.anchoredPosition = SpritzWindowClamp;
    }
}
