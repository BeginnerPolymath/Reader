using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.Drawing;

namespace Spritz {

[System.Serializable]
public struct TextLine
{
    public RectTransform transform;
    public List<TextScript> Words;

    public TextLine (RectTransform _transform)
    {
        transform = _transform;
        Words = new List<TextScript>();
    }
}

[System.Serializable]
public struct ContentLines
{
    public RectTransform transform;
    public List<TextLine> Lines;

    public ContentLines (RectTransform _transform)
    {
        transform = _transform;
        Lines = new List<TextLine>();

    }
}


public class FB2Viewer : MonoBehaviour
{
    public delegate void Method(XmlNode xmlNode);

    public Dictionary<string, Method> Methods = new Dictionary<string, Method>();

    public RectTransform PanelTexts;

    public Text SpritzWord;

    public GameObject LinePref, TextPref, ContentLinesPrefab;

    public List<ContentLines> ContentLines = new List<ContentLines>();

    int contentIndex = 0;
    int lineIndex = -1;

    float sizeWight;

    int SpaceID;

    public ScreenSizer screenSizer;

    GameObject zaz;

    void AddText (string _text)
    {
        TextScript kek = TextPref.GetComponent<TextScript>();
        kek.UIText.text = _text;
        sizeWight -= kek.UIText.preferredWidth + 11;

        //print((sizeWight + kek.UIText.preferredWidth + 11) + " => " + sizeWight + " | " + kek.UIText.text);

        if(sizeWight <= 0)
        {
            sizeWight = -(kek.UIText.preferredWidth + 11);
            AddLine (false);
            TextScript newText = Instantiate(TextPref, ContentLines[contentIndex].Lines[lineIndex].transform).GetComponent<TextScript>();
            newText.name = newText.UIText.text;
        
            ContentLines[contentIndex].Lines[lineIndex].Words.Add(newText);
            return;
        }
        else
        {
            kek.UIText.text = " ";

            if(++SpaceID > 0  && ContentLines[contentIndex].Lines[lineIndex].transform.childCount > 0)
            {
                zaz = Instantiate(TextPref, ContentLines[contentIndex].Lines[lineIndex].transform);
                zaz.name = "|Пробел|";
            }

            kek.UIText.text = _text;

            TextScript newText = Instantiate(TextPref, ContentLines[contentIndex].Lines[lineIndex].transform).GetComponent<TextScript>();
            newText.name = newText.UIText.text;
            if(zaz)
            {
                newText.GetComponent<TextScript>().Space = zaz.GetComponent<Image>();
                zaz = null;
            }
                
            
            ContentLines[contentIndex].Lines[lineIndex].Words.Add(newText);

            
        }
    }

    int LinesCount = -1;

    public Transform ContentLinesArray;

    void AddLine (bool NewLine)
    {
        LinesCount++;

        if(NewLine == true)
            sizeWight = screenSizer.Sizer.rect.width - 20;
        else if(NewLine == false)
            sizeWight += screenSizer.Sizer.rect.width - 20;
        
        
        if(LinesCount == 50)
        {
            ContentLines.Add(new ContentLines(Instantiate(ContentLinesPrefab, ContentLinesArray).GetComponent<RectTransform>()));
            LinesCount = 0;
            contentIndex++;
            lineIndex = -1;
        }
        
        RectTransform aza = Instantiate(LinePref, ContentLines[contentIndex].transform).GetComponent<RectTransform>();

        ContentLines[contentIndex].Lines.Add(new TextLine(aza));
        lineIndex++;
        SpaceID = 0;
    }
    
    
    // int TextSizer(Text _text)
    // {
    //     CharacterInfo characterInfo = new CharacterInfo();
    //     font.RequestCharactersInTexture(_text.text, _text.fontSize, _text.fontStyle);

    //     int size = 0;

    //     foreach (char chars in _text.text)
    //     {
    //         _text.font.GetCharacterInfo(chars, out characterInfo, _text.fontSize, _text.fontStyle);
    //         size += characterInfo.advance;
    //     }

    //     return size;
    // }

    public Font font;

    void Start()
    {
        Methods.Add("title", Title);
        Methods.Add("p", P);
        Methods.Add("epigraph", Epigraph);
        Methods.Add("text-author", P);
        Methods.Add("section", Section);
        Methods.Add("empty-line", EmptyLine);

        ParsingStart ();
    }

    void ParsingStart ()
    {
        XmlDocument xmlDoc = new XmlDocument();

        //xmlDoc.Load("Assets/Books/FB2/Уйти красиво. Удивительные похоронные обряды разных стран - (Кейтлин Даути) - 2018.fb2");
        xmlDoc.Load("Assets/Books/FB2/Fresko_Vse-luchshee-chto-ne-kupish-za-dengi.KH_9ew.375942.fb2");
        //xmlDoc.Load("Assets/Books/FB2/Федин С.Н. - Математики тоже шутят - 2009.fb2");
        

        XmlNodeList za = xmlDoc.GetElementsByTagName("body")[0].ChildNodes;

        foreach (XmlNode node in za)
        {
            
            // if(node.Name == "section")
            // {
            //     Methods[node.Name].Invoke(node);
            //     break;
            // }
            
            Methods[node.Name].Invoke(node);
            
        }

        for (int i = 0; i < ContentLines[contentIndex].Lines.Count; i++)
        {
            ContentLines[contentIndex].Lines[i].transform.name = ContentLines[contentIndex].Lines[i].Words[0].UIText.text + "...";
        }
    }

    void EmptyLine (XmlNode xmlNode)
    {
        Instantiate(LinePref, ContentLines[contentIndex].transform);
    }

    void Section (XmlNode xmlNode)
    {
        Instantiate(LinePref, ContentLines[contentIndex].transform);

        LinePref.GetComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.UpperLeft;

        foreach (XmlNode node in xmlNode.ChildNodes)
        {
            Methods[node.Name].Invoke(node);
        }
    }

    void Title (XmlNode xmlNode)
    {
        LinePref.GetComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.UpperCenter;

        foreach (XmlNode node in xmlNode.ChildNodes)
        {
            Methods[node.Name].Invoke(node);
        }

        LinePref.GetComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.UpperLeft;
    }

    void Epigraph (XmlNode xmlNode)
    {
        Instantiate(LinePref, ContentLines[contentIndex].transform);

        LinePref.GetComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.UpperRight;

        foreach (XmlNode node in xmlNode.ChildNodes)
        {
            
            Methods[node.Name].Invoke(node);
        }

        LinePref.GetComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.UpperLeft;
    }

    void P (XmlNode xmlNode)
    {
        if(sizeWight > 0)
        {
            AddLine (true);
        }
        else if(sizeWight <= 0)
        {
            AddLine (false);
        }

        Text TextPrefab = TextPref.GetComponent<TextScript>().UIText;
        

        foreach (XmlNode node in xmlNode.ChildNodes)
        {
            if(node.Name == "#text" || node.Name == "strong" || node.Name == "emphasis")
            {
                if(node.Name == "#text")                                                                        //Обычный текст
                {
                    TextPrefab.fontStyle = FontStyle.Normal;
                    AddStyleText(node);
                }
                else if(node.Name == "strong")
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        if(childNode.Name == "#text")                                                           //Жирный текст
                        {
                            TextPrefab.fontStyle = FontStyle.Bold;
                            AddStyleText(childNode);
                        }
                        else if(childNode.Name == "emphasis")                                                   //Жирный-курсивный текст
                        {
                            TextPrefab.fontStyle = FontStyle.BoldAndItalic;
                            AddStyleText(childNode);
                        }                        
                    }
                }
                else if(node.Name == "emphasis")
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        if(childNode.Name == "#text")                                                           //Курсивный текст
                        {
                            TextPrefab.fontStyle = FontStyle.Italic;
                            AddStyleText(childNode);
                        }
                        else if(childNode.Name == "strong")                                                     //Жирный-курсивный текст
                        {
                            TextPrefab.fontStyle = FontStyle.BoldAndItalic;
                            AddStyleText(childNode);
                        }                        
                    }
                }
            }
            
        }

        TextPref.GetComponent<TextScript>().UIText.fontStyle = FontStyle.Normal;
    }

    void AddStyleText (XmlNode node)
    {
        string a = node.InnerText;

        if(node.InnerText[0] == ' ')
        {
            a = node.InnerText.Remove(0, 1);
        }
        if(node.InnerText[node.InnerText.Length-1] == ' ')
        {
            a = node.InnerText.Remove(node.InnerText.Length-1);
        }

        string[] words = a.Split(' ');

        foreach (string word in words)
        {
            //Wordses.Add(word);
        }

        foreach (string word in words)
        {
            AddText (word);
        }
    }

    public List<string> Wordses = new List<string>();

    public int LineID = 0, TextID = 0;

    public float SpeedWordChange;

    float timer;

    void Update ()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            foreach (var item in Wordses)
            {
                Instantiate(TextPref).GetComponent<TextScript>().UIText.text = item;
            }
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            PanelTexts.gameObject.SetActive(!PanelTexts.gameObject.activeSelf);
            PanelTexts.gameObject.SetActive(!PanelTexts.gameObject.activeSelf);
        }

        if(Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            timer += Time.deltaTime;
            if(timer >= SpeedWordChange)
            {
                timer = 0;

                if(LineID < ContentLines[0].Lines.Count && TextID != ContentLines[0].Lines[LineID].Words.Count)
                {
                    if(++TextID == ContentLines[0].Lines[LineID].Words.Count)
                    {
                        if(++LineID == ContentLines[0].Lines.Count)
                        {
                            return;
                        }

                        TextID = 0;
                    }

                    
                    SpritzWord.text = ContentLines[0].Lines[LineID].Words[TextID].UIText.text;
                    SpritzWord.fontStyle = ContentLines[0].Lines[LineID].Words[TextID].UIText.fontStyle;
                    ContentLines[0].Lines[LineID].Words[TextID].Background.color = new Color32(0, 0, 0, 25);

                    if(ContentLines[0].Lines[LineID].Words[TextID].Space != null)
                        ContentLines[0].Lines[LineID].Words[TextID].Space.color = new Color32(0, 0, 0, 25);

                    if(ContentLines[0].Lines[LineID].Words[TextID].UIText.text[ContentLines[0].Lines[LineID].Words[TextID].UIText.text.Length-1] == ',')
                    {
                        timer -= SpeedWordChange * 2f;
                    }
                    else if(ContentLines[0].Lines[LineID].Words[TextID].UIText.text[ContentLines[0].Lines[LineID].Words[TextID].UIText.text.Length-1] == '.')
                    {
                        timer -= SpeedWordChange * 3f;
                    }
                    else
                    {
                        timer -= SpeedWordChange;
                    }
                    
                }
            }
        }
    }

}
}