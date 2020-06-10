using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DescriptScript : MonoBehaviour
{
    public TMP_InputField Name;
    public TextMeshProUGUI Description, Annotation;

    [Space(10)]

    public ImageScript CoverImage;

    public Book _Book;

    public ScreenImage _ScreenImage;

    void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !_ScreenImage.gameObject.activeSelf)
        {
            CloseWindow ();
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && _ScreenImage.gameObject.activeSelf)
        {
            _ScreenImage.gameObject.SetActive(false);
        }
    }

    public void CloseWindow ()
    {
        gameObject.SetActive(false);

        Name.text = string.Empty;
        Description.text = string.Empty;
        Annotation.text = string.Empty;
    }


    public void OpenBook ()
    {
        _Book.OpenBooks = true;
    }
}
