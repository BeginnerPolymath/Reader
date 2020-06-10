using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class FileScript : MonoBehaviour
{
    public string FilePath;
    public TextMeshProUGUI FileName;

    public RectTransform MoveTextMask;

    public Image ButtonImage;
    public bool File;

    [Space(10)]
    public FileManager _FileManager;

    public void SetText (string filePath, string fileName)
    {
        FileName.text = fileName;
        FilePath = filePath;
    }

    public void Click ()
    {
        if(TimerStart && FileName.rectTransform.rect.width > MoveTextMask.rect.width)
        {
            Move = true;

            TimerStart = false;
            Timer = 0;
            return;
        }
        else if(TimerStart && FileName.rectTransform.rect.width <= MoveTextMask.rect.width)
        {
            FolderClick ();

            TimerStart = false;
            Timer = 0;
            return;
        }

        TimerStart = true;
    }



    public void FolderClick ()
    {
        if(!File)
        {
            _FileManager.ExpMoveUp(FileName.text);
            _FileManager.FilesShow ();
        }
        else
        {
            //_FileManager._Library.AddBookLibrary(FilePath);
            OpenBook = true;
        }
    }

    bool OpenBook = false;

    public bool A;



    bool TimerStart;

    float Timer;

    [HideInInspector]
    public float Times = 0.2f;

    bool Move;
    bool Direction = true;

    public float SpeedText = 2;

    void MoveText ()
    {
        if(Move == true)
        {
            float a = MoveTextMask.rect.width-FileName.rectTransform.rect.width;

            if(Direction)
            {
                FileName.rectTransform.anchoredPosition -= new Vector2(SpeedText, 0);

                if(FileName.rectTransform.anchoredPosition.x < a - 1)
                {
                    Direction = false;
                }
            }
            else
            {
                FileName.rectTransform.anchoredPosition += new Vector2(SpeedText, 0);

                if(FileName.rectTransform.anchoredPosition.x > -1)
                {
                    Move = false;
                    Direction = true;
                    FileName.rectTransform.anchoredPosition = Vector2.zero;
                }
            }

            FileName.rectTransform.anchoredPosition = new Vector2(Mathf.Clamp(FileName.rectTransform.anchoredPosition.x, a, 0), 0);
        }
    }

    void Update ()
    {
        MoveText ();

        if(TimerStart)
        {
            Timer += Time.deltaTime;

            if(Timer >= Times)
            {
                FolderClick ();

                Timer = 0;
                TimerStart = false;
            }
        }

        if(OpenBook)
        {
            _FileManager.LoadWindow.SetActive(true);

            if(A == true)
            {
                Timer = 0;
                TimerStart = false;
                Move = false;
                FileName.rectTransform.anchoredPosition = Vector2.zero;
                Direction = true;

                _FileManager._Library.AddBookLibrary(FilePath);
                OpenBook = false;
                A = false;
                return;
            }
            A = true;
        }
    }
}
