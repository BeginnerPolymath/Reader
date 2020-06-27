using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;
using System.Threading.Tasks;
using MessagePack;

[MessagePackObject]
public struct ReadInfo
{
    [Key(0)]
    public int PageID;
    [Key(1)]
    public int WordID;
}

[MessagePackObject(keyAsPropertyName: true)]
public class Settings
{
    public bool SpritZActivated = false;

    public float SpritZSize = 60;
    public float TextBookSize = 36;
    public float DescriptSize = 25;

    public float CharCoeffTime = 0.015f;
    public float EndPunctCoeffTime = 0.150f;
    public float PunctCoeffTime = 0.070f;
    public float HighcapCoeffTime = 0.070f;

    public float PauseBeginReadTime = 0.3f;

    public int UnCoefWordLength = 6;

    public bool SpritzMode = false;

    public Vector2 SpritzWindowPosition = new Vector2(0, -200);

    public Vector2 SpritzStartButtonPosition = new Vector2(356, -377);


    public List<string> Books = new List<string>();

    public int ColorSetupID = 0;
}

public class SpritzScript : MonoBehaviour
{
        public float timeWord;

        public int WordID = 0;

        public WHTMP wHTMP;

        public TextMeshProUGUI ViweportText;


        public List<Page> Pages = new List<Page>(1);

        public int PageID = 0;

        public List<PageTextC> PagesText = new List<PageTextC>();

        public string ReadBookFolder;

        public TextMeshProUGUI PageInfo;


        public TextMeshProUGUI WordZoomText;

        public Library _Library;

        public ReadInfo _ReadInfo;

        public DescriptScript Descript;

        public RectTransform SpritzStartButton;

    void Awake ()
    {
        Input.multiTouchEnabled = false;

        CreateReadSettingFile ();
    }

    void Start ()
    {
        

        LoadReadSettingFile ();

        
    }

    void LoadDEFAULTReadSetting ()
    {
        //
    }

    void LoadReadSettingFile ()
    {
        FileStream filez = File.Open(Application.persistentDataPath + "/ReadSetting.newPack", FileMode.Open);
        Settings settings = MessagePackSerializer.Deserialize<Settings>(filez);
        filez.Close();

        SpritzContent.SetActive(settings.SpritZActivated);
        SprtiZActivatedToggle.isOn = SpritzContent.activeSelf;
    

        SizeSpritzWordIF.text = settings.SpritZSize.ToString();
        PrefabSpritZChar.GetComponent<TextMeshProUGUI>().fontSize = settings.SpritZSize;

        SizeTextBookIF.text = settings.TextBookSize.ToString();
        ViweportText.fontSize = settings.TextBookSize;

        SizeDescriptIF.text = settings.DescriptSize.ToString();
        Descript.Annotation.fontSize = settings.DescriptSize;


        CoefCharIF.text = (settings.CharCoeffTime*1000).ToString();
        CoeffChar = settings.CharCoeffTime;

        EndPunctuationIF.text = (settings.EndPunctCoeffTime*1000).ToString();
        EndPunctuationCoeff = settings.EndPunctCoeffTime;

        PunctuationIF.text = (settings.PunctCoeffTime*1000).ToString();
        PunctuationCoeff = settings.PunctCoeffTime;

        HighcapIF.text = (settings.HighcapCoeffTime*1000).ToString();
        HighcapCoeff = settings.HighcapCoeffTime;

        PauseBeginReadTimeIF.text = (settings.PauseBeginReadTime*1000).ToString();
        PauseBeginReadTime = settings.PauseBeginReadTime;

        //Сохранение длины отображаемого слова, которое не считается через коэффициент

        UnCoefWordLength = settings.UnCoefWordLength;

        //Сохранение режима активации SpritZ

        SpritzMode = settings.SpritzMode;

        if(SpritzMode)
            SpritzModeText.text = NameModes[1];
        else
            SpritzModeText.text = NameModes[0];

        //Сохранение позиций панели со SpritZ словом и кнопки SpritZ

        SpritzWindow.anchoredPosition  = settings.SpritzWindowPosition;
        SpritzStartButton.anchoredPosition = settings.SpritzStartButtonPosition;
    }

    public void SaveReadSettingFile ()
    {
        FileStream filez = File.Open(Application.persistentDataPath + "/ReadSetting.newPack", FileMode.Open);
        Settings settings = MessagePackSerializer.Deserialize<Settings>(filez);
        filez.Close();

        settings.SpritZActivated = SpritzContent.activeSelf;

        settings.SpritZSize = PrefabSpritZChar.GetComponent<TextMeshProUGUI>().fontSize;
        settings.TextBookSize = ViweportText.fontSize;
        settings.DescriptSize = Descript.Annotation.fontSize;

        settings.CharCoeffTime = CoeffChar;
        settings.EndPunctCoeffTime = EndPunctuationCoeff;
        settings.PunctCoeffTime = PunctuationCoeff;
        settings.HighcapCoeffTime = HighcapCoeff;
        settings.PauseBeginReadTime = PauseBeginReadTime;

        settings.SpritzWindowPosition = SpritzWindow.anchoredPosition;
        settings.SpritzStartButtonPosition = SpritzStartButton.anchoredPosition;

        settings.UnCoefWordLength = UnCoefWordLength;

        settings.SpritzMode = SpritzMode;

        FileStream steams = new FileStream(Application.persistentDataPath + "/ReadSetting.newPack", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        MessagePackSerializer.Serialize(steams, settings);
        steams.Close();
        
    }

    void CreateReadSettingFile ()
    {
        if(!File.Exists(Application.persistentDataPath + "/ReadSetting.newPack"))
        {
            FileStream steams = new FileStream(Application.persistentDataPath + "/ReadSetting.newPack", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            MessagePackSerializer.Serialize(steams, new Settings());
            steams.Close();
        }

        LoadReadSettingFile ();
    }

    public TMP_InputField SizeSpritzWordIF;

    public void SetSizeSpritzWord (string value)
    {
        float size = float.Parse(value);

        size = Mathf.Clamp(size, 30, 100);

        PrefabSpritZChar.GetComponent<TextMeshProUGUI>().fontSize = size;
        SizeSpritzWordIF.text = size.ToString();

        if(!_Library.gameObject.activeSelf && SpritzContent.activeSelf)
            SetSpritzWord(WordID, WordInTextColor);

        SaveReadSettingFile ();
    }

    public TMP_InputField SizeTextBookIF;

    public void SetSizeTextBookIF (string value)
    {
        float size = float.Parse(value);

        size = Mathf.Clamp(size, 30, 50);

        ViweportText.fontSize = size;
        SizeTextBookIF.text = size.ToString();

        
        
        if(!_Library.gameObject.activeSelf)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(ViweportText.rectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(ContentText);
            TextForceUpdate();
        }
            

        SaveReadSettingFile ();

    }

    public TMP_InputField SizeDescriptIF;

    public void SetDescriptSizeText (string value)
    {
        float size = float.Parse(value);

        size = Mathf.Clamp(size, 20, 40);

        Descript.Annotation.fontSize = size;
        Descript.Description.fontSize = size;
        SizeDescriptIF.text = size.ToString();

        LayoutRebuilder.ForceRebuildLayoutImmediate(Descript.Annotation.rectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(Descript.Description.rectTransform);
        
        SaveReadSettingFile ();
    }

    public TMP_InputField PauseBeginReadTimeIF;

    public float PauseBeginReadTime = 0.3f;

    public void SetPauseBeginReadTime (string value)
    {
        PauseBeginReadTime = float.Parse(value) / 1000;

        if(PauseBeginReadTime > 0.800f || PauseBeginReadTime < 0.000f)
        {
            PauseBeginReadTime = Mathf.Clamp(PauseBeginReadTime, 0.000f, 0.800f);
            PauseBeginReadTimeIF.text = (PauseBeginReadTime*1000).ToString();
        }
        
        SaveReadSettingFile ();
    }

    public TMP_InputField UnCoefWordLengthIF;

    public int UnCoefWordLength = 6;

    public void UnCoefWordLengthV (string value)
    {
        UnCoefWordLength = int.Parse(value);

        if(UnCoefWordLength > 10 || UnCoefWordLength < 0)
        {
            UnCoefWordLength = (int)Mathf.Clamp(PauseBeginReadTime, 0, 10);
            UnCoefWordLengthIF.text = UnCoefWordLength.ToString();
        }
        
        SaveReadSettingFile ();
    }

    public Toggle SprtiZActivatedToggle;

    public void SpritZMethodActivated (bool enable)
    {
        if(enable && !_Library.gameObject.activeSelf)
        {
            SetPage(PageID);

            ViweportText.transform.parent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            ViweportText.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -(SpritzWindow.rect.height/2) + SpritzWindow.anchoredPosition.y);
        }
        else if(!enable && !_Library.gameObject.activeSelf)
        {
            ViweportText.transform.parent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            ViweportText.gameObject.SetActive(false);
            ViweportText.gameObject.SetActive(true);
        }

        SaveReadSettingFile ();
    }


    public void SpritzDown ()
    {
        timeWord = SpritZChars.Count * CoeffChar;
        isSpritz = true;

        StartReadTime = false;
    }

    public float PRTime;

    public float PauseReadTimer = 0.5f;

    public bool StartReadTime;

    [Space(10)]

    public GameObject ReadFade;
    public bool SpritzMode;

    public TextMeshProUGUI SpritzModeText;

    string[] NameModes =
    {
        "Удержание",
        "Нажатие"
    };

    public Button OpenFindWindowButton, OpenSettingsWindowButton;

    public void SetSpritzMode ()
    {
        SpritzMode = !SpritzMode;

        if(SpritzMode)
            SpritzModeText.text = NameModes[1];
        else
            SpritzModeText.text = NameModes[0];

        SaveReadSettingFile ();
    }

    public void StartRead ()
    {
        if(!SpritzMode)
        {
            ReadFade.SetActive(true);
            StartReadTime = true;


        }
        else
        {
            ReadFade.SetActive(!ReadFade.activeSelf);
            isSpritz = !isSpritz;

            OpenFindWindowButton.interactable = !OpenFindWindowButton.interactable;
            OpenSettingsWindowButton.interactable = !OpenSettingsWindowButton.interactable;
        }
    }  

    void PauseReadStart ()
    {
        if(StartReadTime)
        {
            PRTime += Time.deltaTime;

            if(PRTime >= PauseReadTimer)
            {
                PRTime = 0;
                SpritzDown ();
            }
        }
    }

    public void SpritzUp ()
    {
        if(!SpritzMode)
        {
            ReadFade.SetActive(false);

            timeWord = SpritZChars.Count * CoeffChar;
            isSpritz = false;

            PRTime = 0;
            StartReadTime = false;
        }
    }

    public bool isSpritz;

    public SettingsScript _SettingsScript;

    public FindTextScript _FindTextScript;

    void Update()
    {
        PauseReadStart ();

        if(Input.GetMouseButtonDown(1))
        {
            SpritzDown ();
        }
        if(Input.GetMouseButton(1) || isSpritz)
        {
            GoSpritz ();
        }
        if(Input.GetMouseButtonUp(1))
        {
            SpritzUp ();
        }

        if(Input.GetKeyDown(KeyCode.Escape) && !_Library.gameObject.activeSelf && !isSpritz && !WordZoomText.transform.parent.gameObject.activeSelf)
        {
            if(_FindTextScript.gameObject.activeSelf)
            {
                _FindTextScript.ClearAll();
                _FindTextScript.gameObject.SetActive(false);
            }
            else if(_SettingsScript.OpenWindow == false)  
            {
                _SettingsScript.Close();
            }
            else if(_ScreenImage.gameObject.activeSelf)
            {
                _ScreenImage.gameObject.SetActive(false);
            }
            else if(!_Library.gameObject.activeSelf)
            {
                ExitBook ();
            }
        }
    }

    public Book _Book;

    void ExitBook ()
    {
        Pages = new List<Page>(1);;
        PagesText = new List<PageTextC>();
        ViweportText.text = string.Empty;
        PageID = 0;

        _Library.gameObject.SetActive(true);
    }

    [Space(10)]

    public TMP_InputField CoefCharIF;

    public float CoeffChar = 0.015f;

    public void SetCoefChar (string value)
    {
        CoeffChar = float.Parse(value) / 1000;

        if(CoeffChar > 0.100f || CoeffChar < 0.005f)
        {
            CoeffChar = Mathf.Clamp(CoeffChar, 0.005f, 0.100f);
            CoefCharIF.text = (CoeffChar*1000).ToString();
        }
        
        SaveReadSettingFile ();
    }

    public TMP_InputField EndPunctuationIF;

    public float EndPunctuationCoeff = 0.02f;

    string[] EndPunctuation = new string[]
    {
        ".", "!", "?"
    };

    public void SetEndPunctuationCoeff (string value)
    {
        EndPunctuationCoeff = float.Parse(value) / 1000;

        if(EndPunctuationCoeff > 1.000f || EndPunctuationCoeff < 0.000f)
        {
            EndPunctuationCoeff = Mathf.Clamp(EndPunctuationCoeff, 0.000f, 1.000f);
            EndPunctuationIF.text = (EndPunctuationCoeff*1000).ToString();
        }
        
        SaveReadSettingFile ();
    }

    public TMP_InputField PunctuationIF;

    public float PunctuationCoeff = 0.01f;

    string[] Punctuation = new string[]
    {
        ",", ":", ";", ")", "]", "}", "\"", ">", "»", "-", "\n"
    };

    public void SetPunctuationCoeff (string value)
    {
        PunctuationCoeff = float.Parse(value) / 1000;

        if(PunctuationCoeff > 1.000f || PunctuationCoeff < 0.000f)
        {
            PunctuationCoeff = Mathf.Clamp(PunctuationCoeff, 0.000f, 1.000f);
            PunctuationIF.text = (PunctuationCoeff*1000).ToString();
        }
        
        SaveReadSettingFile ();
    }

    public TMP_InputField HighcapIF;

    public float HighcapCoeff;

    public void SetHighcapCoeff (string value)
    {
        HighcapCoeff = float.Parse(value) / 1000;

        if(HighcapCoeff > 1.000f || HighcapCoeff < 0.000f)
        {
            HighcapCoeff = Mathf.Clamp(HighcapCoeff, 0.000f, 1.000f);
            HighcapIF.text = (HighcapCoeff*1000).ToString();
        }
        
        SaveReadSettingFile ();
    }

    void GoSpritz ()
    {
        if(PageID != Pages.Count-1 || WordID != Pages[PageID].Steps.Count-1)
        {
            timeWord -= Time.deltaTime;

            if(timeWord <= 0)
            {
                timeWord = 0;

                NextSpritzWord ();

                SavePages();

                CalculateTime ();
            }
        }
    }

    public bool NewLine;

    public bool DifWord;

    void CalculateTime ()
    {
        if(DifWord)
        {
            timeWord += 0.2f;
            DifWord = false;
        }

        if(SpritZChars.Count == 0 && !NewLine)
        {
            timeWord += 0.25f;
            NewLine = true;
        }
        else if(SpritZChars.Count == 0 && NewLine)
        {
            NewLine = false;
            timeWord += 0;
        }
        else 
        {
            NewLine = false;

            char lastChar = SpritZChars[SpritZChars.Count-1].text[0];
            char firstChar = SpritZChars[0].text[0];

            if(char.IsUpper(firstChar))
            {
                timeWord += HighcapCoeff;
            }

            if(lastChar == '-')
            {
                DifWord = true;
            }

            foreach (string chars in Punctuation)
            {
                if(lastChar.ToString() == chars)
                {
                    timeWord += PunctuationCoeff;
                }
            }

            foreach (string chars in EndPunctuation)
            {
                if(lastChar.ToString() == chars)
                {
                    timeWord += EndPunctuationCoeff;
                }
            }

            if(SpritZChars.Count > UnCoefWordLength)
                timeWord += SpritZChars.Count * CoeffChar;
            else
                timeWord += 0.2f;
        }
    }

    bool CanSave = true;

    FileStream steams;

    async void SavePages()
    {
        _ReadInfo.PageID = PageID;
        _ReadInfo.WordID = WordID;

        _Book.PageCounter.text = (PageID + 1).ToString() + "/" + PagesText.Count.ToString();

        if(CanSave)
        {
            CanSave = false;
            await Task.Run(() =>
            {
                FileStream steams = new FileStream(Library.pathToBooksFolder + '/' + ReadBookFolder + "/pages.packNew", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                MessagePackSerializer.Serialize(steams, Pages);
                steams.Close();

                steams = new FileStream(Library.pathToBooksFolder + '/' + ReadBookFolder + "/readinfo.packNew", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                MessagePackSerializer.Serialize(steams, _ReadInfo);
                steams.Close();

                CanSave = true;
            });
        }

        
    }

    void NextSpritzWord ()
    {
        WordID++;

        if(WordID == Pages[PageID].Steps.Count)
        {
            PageID++;
            if(PageID != Pages.Count)
            {
                SetPage (PageID);
                return;
            }
            else if(PageID == Pages.Count)
            {
                return;
            }
        }

        SetSpritzWord(WordID, WordInTextColor, true);

        if(WordID != 0)
            wHTMP.ChangeColorChars(PageID, WordID-1, Colors[1]);
    }

    [Space(10)]
    public RectTransform SpritzWindow;

    public ImageScript ImagePrefab;

    public RectTransform ImagesViewContent;

    public RectTransform ImagesContent;

    public List<GameObject> ImagesArray = new List<GameObject>();

    public ScreenImage _ScreenImage;

    public static Color32 WordInTextColor = new Color32(112, 197, 82, 255);

    public GameObject SpritzContent;

    public void SetPage (int pageID, bool LoadReadInfo = false)
    {
        ClearImages ();

        ImagesContent.anchoredPosition = Vector2.zero;

        if(SpritzContent.activeSelf)
            timeWord += 0.3f;

        PageID = pageID;
        PageInfo.text = (PageID + 1).ToString() + '/' + Pages.Count;

        ViweportText.text = PagesText[PageID].PageText;
        //ViweportText.ForceMeshUpdate();

        if(PagesText[PageID].NameImages.Count > 0)
        {
            int a = 0;
            foreach (string nameImage in PagesText[PageID].NameImages)
            {
                a++;
                ImageScript imageScript = Instantiate(ImagePrefab, ImagesContent).GetComponent<ImageScript>();

                imageScript._ScreenImage = _ScreenImage;

                imageScript.ReadBookFolder = ReadBookFolder;

                imageScript.Number.text = a.ToString();

                ImagesArray.Add(imageScript.gameObject);

                imageScript.ImageName = nameImage;

                byte[] z = File.ReadAllBytes(Library.pathToBooksFolder + "/" + ReadBookFolder + "/Images/" + nameImage);

                Texture2D tex = new Texture2D(0, 0);

                tex.LoadImage(z);

                Sprite coverImageSprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);

                imageScript._Image.sprite = coverImageSprite;
                imageScript._Image.SetNativeSize();
            }
        }

        if(SpritzContent.activeSelf)
        {
            if(SpritzWindow.anchoredPosition.y > Screen.height / -2)
            {
                ViweportText.transform.parent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                ViweportText.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -(SpritzWindow.rect.height/2) + SpritzWindow.anchoredPosition.y);
            }
            else if(SpritzWindow.anchoredPosition.y < Screen.height / -2)
            {
                ViweportText.transform.parent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
        }
        else
        {
            ViweportText.transform.parent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            //ViweportText.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -(SpritzWindow.rect.height/2) + SpritzWindow.anchoredPosition.y);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(ViweportText.rectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(ContentText);
        Canvas.ForceUpdateCanvases();

        ImagesViewContent.sizeDelta = new Vector2(ImagesViewContent.sizeDelta.x, ImagesContent.sizeDelta.y);

        LayoutRebuilder.ForceRebuildLayoutImmediate(ViweportText.rectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(ContentText);
        Canvas.ForceUpdateCanvases();

        WordID = 0;

        if(SpritzContent.activeSelf)
            LoadColorSpritzStep ();

        if(!LoadReadInfo && SpritzContent.activeSelf)
            SetSpritzWord(0, WordInTextColor);

        SavePages();
    }

    public void SetNumberPage (string number)
    {
        if(number != string.Empty)
        {
            int id = int.Parse(number) - 1;

            if(id >= 0 && id < Pages.Count)
            {
                SetPage(id);
            }
        }
    }

    void ClearImages ()
    {
        foreach (GameObject img in ImagesArray)
        {
            DestroyImmediate(img);
        }

        ImagesArray.Clear();
    }

    public int RemWordIDLine;

    public RectTransform ContentText;



    public RectTransform ContainerSpritZChar;

    public GameObject PrefabSpritZChar;

    public List<TextMeshProUGUI> SpritZChars;

    public RectTransform Canvass;

    public void SetSpritzWord (int ID, Color32 color, bool Read = false)
    {
        Pages[PageID].Steps[ID].ColorID = 1;     //Запоминаем цвет прочитанного текста
        
        for (int i = 0; i < SpritZChars.Count; i++)
        {
            DestroyImmediate(SpritZChars[i].gameObject);
        }

        SpritZChars.Clear();

        for (int z = Pages[PageID].Steps[ID].First; z <= Pages[PageID].Steps[ID].Last; z++)
        {

            SpritZChars.Add(Instantiate(PrefabSpritZChar, ContainerSpritZChar).GetComponent<TextMeshProUGUI>());

            SpritZChars[SpritZChars.Count-1].text += wHTMP.m_TextMeshPro.textInfo.characterInfo[z].character;
            SpritZChars[SpritZChars.Count-1].color = wHTMP.m_TextMeshPro.textInfo.characterInfo[z].color;
            //SpritZChars[SpritZChars.Count-1].fontStyle = wHTMP.m_TextMeshPro.textInfo.characterInfo[z].style;

            int idLine = wHTMP.m_TextMeshPro.textInfo.characterInfo[z].lineNumber;

            if(RemWordIDLine != idLine && Read)
            {
                ContentText.anchoredPosition += new Vector2(0, wHTMP.m_TextMeshPro.textInfo.lineInfo[idLine].lineHeight);

                ContentText.anchoredPosition = new Vector2(0, Mathf.Clamp(ContentText.anchoredPosition.y, -Canvass.rect.height + 70, ContentText.sizeDelta.y - 130));

                RemWordIDLine = idLine;
            }
            else if(RemWordIDLine != idLine && !Read)
            {
                RemWordIDLine = idLine;
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(ContainerSpritZChar);

        Canvas.ForceUpdateCanvases();

        wHTMP.ChangeColorChars(PageID, ID, color);
        wHTMP.StepIDRem = ID;

        //Центрирование слова в спритзе
        ContainerSpritZChar.anchoredPosition = Vector2.zero;

        int charShifts = Mathf.RoundToInt(SpritZChars.Count / 4f);

        if(SpritZChars.Count != 0)
        {
            float contentWordShift = ContainerSpritZChar.position.x - SpritZChars[charShifts].rectTransform.position.x;

            int meshIndex = SpritZChars[charShifts].textInfo.characterInfo[0].materialReferenceIndex;
            int vertexIndex = SpritZChars[charShifts].textInfo.characterInfo[0].vertexIndex;
            
            Color32[] vertexColors = SpritZChars[charShifts].textInfo.meshInfo[meshIndex].colors32;
            vertexColors[vertexIndex + 0] = new Color(1, 0.5f, 0.5f, 1);;
            vertexColors[vertexIndex + 1] = new Color(1, 0.5f, 0.5f, 1);;
            vertexColors[vertexIndex + 2] = new Color(1, 0.5f, 0.5f, 1);;
            vertexColors[vertexIndex + 3] = new Color(1, 0.5f, 0.5f, 1);;

            SpritZChars[charShifts].UpdateVertexData(TMP_VertexDataUpdateFlags.All);

            ContainerSpritZChar.anchoredPosition += new Vector2(contentWordShift * ScreenSizer.DragShiftCoef, 0);
        }

        SavePages();
    }

    //Функция выбора слова из текста - назначение на спритз и выделение зеленым цветом/прошлое слово серым (если есть)
    public void ClickWordInText (int ID)        
    {
        for (int i = 0; i < Pages[PageID].Steps.Count; i++)
        {
            for (int a = Pages[PageID].Steps[i].First; a <= Pages[PageID].Steps[i].Last; a++)
            {
                if(a == ID)
                {
                    WordID = i;
                    SetSpritzWord (i, WordInTextColor);
                    return;
                }
            }
        }
    }

    //Функция выбора слова из текста - назначение на спритз и выделение зеленым цветом/прошлое слово серым (если есть)
    public void SetZoomWordInText (int ID)        
    {
        for (int i = 0; i < Pages[PageID].Steps.Count; i++)
        {
            for (int a = Pages[PageID].Steps[i].First; a <= Pages[PageID].Steps[i].Last; a++)
            {
                if(a == ID)
                {
                    WordID = i;
                    SetZoomWord (i);
                    return;
                }
            }
        }
    }

    void SetZoomWord (int ID)
    {
        WordZoomText.text = string.Empty;
        
        for (int z = Pages[PageID].Steps[ID].First; z <= Pages[PageID].Steps[ID].Last; z++)
        {
            WordZoomText.text += wHTMP.m_TextMeshPro.textInfo.characterInfo[z].character;
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(WordZoomText.transform.parent.GetComponent<RectTransform>());
        Canvas.ForceUpdateCanvases();
    }

    public void ClearZoomWord ()
    {
        WordZoomText.text = string.Empty;

        LayoutRebuilder.ForceRebuildLayoutImmediate(WordZoomText.transform.parent.GetComponent<RectTransform>());
        Canvas.ForceUpdateCanvases();
    }

    
    public Color32[] Colors = new Color32[] { Color.black, new Color(0.9f, 0.9f, 0.9f, 1) };

    void LoadColorSpritzStep ()
    {
        for (int stepIDa = 0; stepIDa < Pages[PageID].Steps.Count; stepIDa++)
        {
            if(Pages[PageID].Steps[stepIDa].ColorID != 0)
                wHTMP.ChangeColorChars(PageID, stepIDa, Colors[Pages[PageID].Steps[stepIDa].ColorID]);
        }
    }

    public void TextForceUpdate ()
    {
        if(SpritzContent.activeSelf)
        {
            Canvas.ForceUpdateCanvases();
    
            LoadColorSpritzStep();
            SetSpritzWord(WordID, WordInTextColor);
        }
    }
}
