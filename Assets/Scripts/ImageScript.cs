using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ImageScript : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Image _Image;

    public RectTransform RectTransform;

    public TextMeshProUGUI Number;

    public string ReadBookFolder;

    public string ImageName;

    public ScreenImage _ScreenImage;

    public void ClickOnImage ()
    {
        byte[] z;

        if(File.Exists(Library.pathToBooksFolder + "/" + ReadBookFolder + "/Images/" + "hight" + ImageName))
        {
            z = File.ReadAllBytes(Library.pathToBooksFolder + "/" + ReadBookFolder + "/Images/" + "hight" + ImageName);
        }
        else if(File.Exists(Library.pathToBooksFolder + "/" + ReadBookFolder + "/Images/" + ImageName))
        {
            z = File.ReadAllBytes(Library.pathToBooksFolder + "/" + ReadBookFolder + "/Images/" + ImageName);
        }
        else
        {
            return;
        }

        _ScreenImage.gameObject.SetActive(true);
        
        Texture2D tex = new Texture2D(0, 0);

        tex.LoadImage(z);

        Sprite coverImageSprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);

        _ScreenImage.LargeImage.sprite = coverImageSprite;

        _ScreenImage.LargeImage.SetNativeSize();

        if(_ScreenImage.LargeImage.rectTransform.sizeDelta.x > 1080 || _ScreenImage.LargeImage.rectTransform.sizeDelta.y > 1920)
        {
            _ScreenImage.LargeImage.rectTransform.sizeDelta /= 2;
        }
    }

    bool Drag;

    public void OnBeginDrag (PointerEventData eventDataZ)
    {
        Drag = true;
    }

    public void OnEndDrag (PointerEventData eventDataZ)
    {
        Drag = false;
    }

    public void OnDrag (PointerEventData eventDataZ)
    {
        RectTransform rectParent = transform.parent.GetComponent<RectTransform>();
        RectTransform rectParentParent = rectParent.parent.GetComponent<RectTransform>();

        if(rectParent.rect.width > rectParentParent.rect.width)
        {
            rectParent.anchoredPosition += new Vector2(eventDataZ.delta.x * ScreenSizer.DragShiftCoef, 0);

            rectParent.anchoredPosition = new Vector2(Mathf.Clamp(rectParent.anchoredPosition.x, rectParentParent.rect.width - rectParent.rect.width, 0), 0);
        }
    }

    public void OnPointerClick (PointerEventData eventDataZ)
    {
        if(!Drag)
        {
            ClickOnImage ();
        }
    }
}
