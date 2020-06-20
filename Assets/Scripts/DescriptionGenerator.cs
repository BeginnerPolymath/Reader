using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using System.IO;
using System;
using System.Data;
using MsgPack;
using MessagePack;

public static class GenresData
{
    public static Dictionary<string, string> Genres = new Dictionary<string, string>
    {
        //Фантастика (Научная фантастика и Фэнтези)
        { "sf_history", "Альтернативная история" }, 
        { "sf_action", "Боевая фантастика" },
        { "sf_epic", "Эпическая фантастика" },
        { "sf_heroic", "Героическая фантастика" },
        { "sf_detective", "Детективная фантастика" },
        { "sf_cyberpunk", "Киберпанк" },
        { "sf_space", "Космическая фантастика" },
        { "sf_social", "Социально-психологическая фантастика" },
        { "sf_horror", "Ужасы и Мистика" },
        { "sf_humor", "Юмористическая фантастика" },
        { "sf_fantasy", "Фэнтези" },
        { "sf", "Научная Фантастика" },

        //Детективы и Триллеры
        { "det_classic", "Классический детектив" },
        { "det_police", "Полицейский детектив" },
        { "det_action", "Боевик" },
        { "det_irony", "Иронический детектив" },
        { "det_history", "Исторический детектив" },
        { "det_espionage", "Шпионский детектив" },
        { "det_crime", "Криминальный детектив" },
        { "det_political", "Политический детектив" },
        { "det_maniac", "Маньяки" },
        { "det_hard", "Крутой детектив" },
        { "thriller", "Триллер" },
        { "detective", "Детектив" },

        //Проза
        { "prose_classic", "Классическая проза" },
        { "prose_history", "Историческая проза" },
        { "prose_contemporary", "Современная проза" },
        { "prose_counter", "Контркультура" },
        { "prose_rus_classic", "Русская классическая проза" },
        { "prose_su_classics", "Советская классическая проза" },

        //Любовные романы
        { "love_contemporary", "Современные любовные романы" },
        { "love_history", "Исторические любовные романы" },
        { "love_detective", "Остросюжетные любовные романы" },
        { "love_short", "Короткие любовные романы" },
        { "love_erotica", "Эротика" },

        //Приключения
        { "adv_western", "Вестерн" },
        { "adv_history", "Исторические приключения" },
        { "adv_indian", "Приключения про индейцев" },
        { "adv_maritime", "Морские приключения" },
        { "adv_geo", "Путешествия и география" },
        { "adv_animal", "Природа и животные" },
        { "adventure", "Прочие приключения" },

        //Детское
        { "child_tale", "Сказка" },
        { "child_verse", "Детские стихи" },
        { "child_prose", "Детскиая проза" },
        { "child_sf", "Детская фантастика" },
        { "child_det", "Детские остросюжетные" },
        { "child_adv", "Детские приключения" },
        { "child_education", "Детская образовательная литература" },
        { "children", "Прочая детская литература" },

        //Поэзия, Драматургия
        { "poetry", "Поэзия" },
        { "dramaturgy", "Драматургия" },

        //Старинное
        { "antique_ant", "Античная литература" },
        { "antique_european", "Европейская старинная литература" },
        { "antique_russian", "Древнерусская литература" },
        { "antique_east", "Древневосточная литература" },
        { "antique_myths", "Мифы. Легенды. Эпос" },
        { "antique", "Прочая старинная литература" },

        //Наука, Образование
        { "sci_history", "История" },
        { "sci_psychology", "Психология" },
        { "sci_culture", "Культурология" },
        { "sci_religion", "Религиоведение" },
        { "sci_philosophy", "Философия" },
        { "sci_politics", "Политика" },
        { "sci_business", "Деловая литература" },
        { "sci_juris", "Юриспруденция" },
        { "sci_linguistic", "Языкознание" },
        { "sci_medicine", "Медицина" },
        { "sci_phys", "Физика" },
        { "sci_math", "Математика" },
        { "sci_chem", "Химия" },
        { "sci_biology", "Биология" },
        { "sci_tech", "Технические науки" },
        { "science", "Прочая научная литература" },

        //Компьютер и интернет
        { "comp_www", "Интернет" },
        { "comp_programming", "Программирование" },
        { "comp_hard", "Компьютерное 'железо'" },
        { "comp_soft", "Программы" },
        { "comp_db", "Базы данных" },
        { "comp_osnet", "ОС и Сети" },
        { "computers", "Прочая околокомпьтерная литература" },

        //Справочная литература
        { "ref_encyc", "Энциклопедии" },
        { "ref_dict", "Словари" },
        { "ref_ref", "Справочники" },
        { "ref_guide", "Руководства" },
        { "reference", "Прочая справочная литература" },

        //Документальная литература
        { "nonf_biography", "Биографии и Мемуары" },
        { "nonf_publicism", "Публицистика" },
        { "nonf_criticism", "Критика" },
        { "design", "Искусство и Дизайн" },
        { "nonfiction", "Прочая документальная литература" },

        //Религия и духовность
        { "religion_rel", "Религия" },
        { "religion_esoterics", "Эзотерика" },
        { "religion_self", "Самосовершенствование" },
        { "religion", "Прочая религионая литература" },

        //Юмор
        { "humor_anecdote", "Анекдоты" },
        { "humor_prose", "Юмористическая проза" },
        { "humor_verse", "Юмористические стихи" },
        { "humor", "Прочий юмор" },

        //Домоводство (Дом и семья)
        { "home_cooking", "Кулинария" },
        { "home_pets", "Домашние животные" },
        { "home_crafts", "Хобби и ремесла" },
        { "home_entertain", "Развлечения" },
        { "home_health", "Здоровье" },
        { "home_garden", "Сад и огород" },
        { "home_diy", "Сделай сам" },
        { "home_sport", "Спорт" },
        { "home_sex", "Эротика, Секс" },
        { "home", "Прочиее домоводство" },
    };
}

[System.Serializable]
[MessagePackObject(keyAsPropertyName: true)]
public class Autor
{
    public List<string> NameParts = new List<string>();

    public string Nickname = string.Empty;

    public string Email = string.Empty;

    public string Homapage = string.Empty;
}

[System.Serializable]
[MessagePackObject(keyAsPropertyName: true)]
public class BookDescription
{
    [Space(5)]
    public List<string> Genres = new List<string>();

    [Space(5)]
    public string BookTitle = string.Empty;

    public string UserBookTitle = string.Empty;

    [Space(5)]

    [TextArea]
    public string Annotation = string.Empty;

    [Space(5)]
    public List<Autor> Authors = new List<Autor>();

    public string Language = string.Empty;

    public string SourceLanguage = string.Empty;

    public int PagesCount;


    public List<Autor> Translators = new List<Autor>();


    public string Data = string.Empty;

    public string Keywords = string.Empty;


    public string Sequence = string.Empty;

    [Space(5)]

    public List<Autor> DIAuthors = new List<Autor>();

    public string ProgramUsed = string.Empty;

    public string DIData = string.Empty;
    
    public string SrcUrl = string.Empty;

    public string SrcOcr = string.Empty;

    public string Version = string.Empty;

    public string History = string.Empty;

    public string DIPublisher = string.Empty;   //Правообладатель

    [Space(5)]

    public string PIBookName = string.Empty;

    public string PIPublisher = string.Empty;

    public string City = string.Empty;

    public string Year = string.Empty;

    public string ISBN = string.Empty;

    public string PISequence = string.Empty;

}

public class DescriptionGenerator : MonoBehaviour
{
    public delegate void Method(XmlNode xmlNode);

    public Dictionary<string, Method> Methods = new Dictionary<string, Method>();

    public BookDescription _BookDescription = new BookDescription();



    void Awake ()
    {
        Methods.Add("description", Description);
        Methods.Add("title-info", TitleInfo);

        Methods.Add("book-title", BookTitle);

        Methods.Add("author", Author);
        Methods.Add("first-name", FirstName);
        Methods.Add("middle-name", MiddleName);
        Methods.Add("last-name", LastName);

        Methods.Add("genre", Genre);
        Methods.Add("annotation", Annotation);
        Methods.Add("coverpage", Coverpage);
        Methods.Add("image", Image);
        Methods.Add("lang", Lang);
        Methods.Add("src-lang", Srclang);
        Methods.Add("keywords", Keywords);
        Methods.Add("date", Data);
        Methods.Add("nickname", Nickname);
        Methods.Add("email", Email);
        Methods.Add("home-page", HomePage);


        Methods.Add("translator", Translator);
        Methods.Add("Tfirst-name", TFirstName);
        Methods.Add("Tmiddle-name", TMiddleName);
        Methods.Add("Tlast-name", TLastName);
        Methods.Add("Tnickname", TNickname);
        Methods.Add("Temail", TEmail);
        Methods.Add("Thome-page", THomePage);
        
        Methods.Add("sequence", Sequence);



        Methods.Add("document-info", DocumentInfo);
        Methods.Add("DIauthor", DIAuthor);

        Methods.Add("DIfirst-name", DIFirstName);
        Methods.Add("DImiddle-name", DIMiddleName);
        Methods.Add("DIlast-name", DILastName);
        Methods.Add("DInickname", DINickname);
        Methods.Add("DIemail", DIEmail);
        Methods.Add("DIhome-page", DIHomePage);
        
        Methods.Add("DIprogram-used", DIProgramUsed);
        Methods.Add("DIdate", DIData);
        Methods.Add("DIsrc-url", DISrcUrl);
        Methods.Add("DIsrc-ocr", DISrcOcr);
        Methods.Add("DIversion", DIVersion);
        Methods.Add("DIhistory", DIHistory);
        Methods.Add("DIpublisher", DIPublisher);

        Methods.Add("DIid", EmptyLine);

        Methods.Add("publish-info", PublishInfo);
        Methods.Add("PIbook-name", BookName);
        Methods.Add("PIpublisher", PIPublisher);
        Methods.Add("PIcity", City);
        Methods.Add("PIyear", Year);
        Methods.Add("PIisbn", ISBN);
        Methods.Add("PIsequence", PISequence);

        
            Methods.Add("Tid", EmptyLine);
            Methods.Add("id", EmptyLine);
            Methods.Add("custom-info", EmptyLine);
            Methods.Add("output", EmptyLine);

            Methods.Add("src-title-info", EmptyLine);
        
    }

    XmlDocument BookFile;

    string BookFolderPath;

    public void CreateDescriptionFile (XmlDocument bookFile, string bookFolderPath, int pagesCount)
    {
        BookFolderPath = bookFolderPath;

        _BookDescription.PagesCount = pagesCount;

        BookFile = bookFile;
        XmlNodeList BookNodes = bookFile.GetElementsByTagName("FictionBook")[0].ChildNodes;

        StartParsing (BookNodes);

        SerializeFile (bookFolderPath);

        _BookDescription = new BookDescription();
    }

    void SerializeFile (string bookFolderPath)
    {
        FileStream steama = new FileStream(bookFolderPath + "/descript.packNew", FileMode.Create, FileAccess.Write);
        MessagePackSerializer.Serialize(steama, _BookDescription);
        steama.Close();
    }

    void EmptyLine (XmlNode node)
    {

    }

    void StartParsing (XmlNodeList BookNodes)
    {
        foreach (XmlNode childNode in BookNodes)
        {
            if(childNode.Name == "description")
            {
                Methods[childNode.Name].Invoke(childNode);
            }
        }
    }

    void Nickname (XmlNode node)
    {
        _BookDescription.Authors[_BookDescription.Authors.Count-1].Nickname = node.InnerText;
    }

    void Email (XmlNode node)
    {
        _BookDescription.Authors[_BookDescription.Authors.Count-1].Email = node.InnerText;
    }

    void HomePage (XmlNode node)
    {
        _BookDescription.Authors[_BookDescription.Authors.Count-1].Homapage = node.InnerText;
    }

    void Description (XmlNode node)
    {
        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods[childNode.Name].Invoke(childNode);
        }
    }

    void TitleInfo (XmlNode node)
    {
        foreach (XmlNode childNode in node.ChildNodes)
        {
            //Debug.Log(childNode.Name);
            Methods[childNode.Name].Invoke(childNode);
        }
    }

    void BookTitle (XmlNode node)
    {
        _BookDescription.BookTitle = node.InnerText;
    }

    void BookName (XmlNode node)
    {
        _BookDescription.PIBookName = node.InnerText;
    }

    void Author (XmlNode node)
    {
        _BookDescription.Authors.Add(new Autor() { NameParts = new List<string>() });
        foreach (XmlNode childNode in node.ChildNodes)
        {
            print(childNode.Name);
            Methods[childNode.Name].Invoke(childNode);
        }
    }

    void FirstName (XmlNode node)
    {
        _BookDescription.Authors[_BookDescription.Authors.Count-1].NameParts.Add(node.InnerText);
    }

    void MiddleName (XmlNode node)
    {
        _BookDescription.Authors[_BookDescription.Authors.Count-1].NameParts.Add(node.InnerText);
    }

    void LastName (XmlNode node)
    {
       _BookDescription.Authors[_BookDescription.Authors.Count-1].NameParts.Add(node.InnerText);
    }

    void Data (XmlNode node)
    {
       _BookDescription.Data = node.InnerText;
    }

    void Keywords (XmlNode node)
    {
       _BookDescription.Keywords = node.InnerText;
    }

    

    void Genre(XmlNode node)
    {
        if(GenresData.Genres.ContainsKey(node.InnerText))
        {
            string match = string.Empty;

            foreach (XmlAttribute attribute in node.Attributes)
            {
                if(attribute.Name == "match")
                {
                    match = " (" + attribute.Value + ")";
                }
            }

            _BookDescription.Genres.Add("<link>" + GenresData.Genres[node.InnerText] + "</link>"  +  match);
        }
            
    }

    void Annotation(XmlNode node)
    {
        foreach (XmlNode childNode in node.ChildNodes)
        {
            _BookDescription.Annotation += childNode.InnerText + '\n';
        }
    }

    void Coverpage(XmlNode node)
    {
        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods[childNode.Name].Invoke(childNode);
        }
    }

    void Image(XmlNode node)
    {
        Texture2D tex = new Texture2D(0, 0);

        string a = "";

        foreach (XmlAttribute item in node.Attributes)
        {
            if(item.Name.Contains(":href"))
            {
                a = node.Attributes[item.Name].Value;
                a = a.Remove(0, 1);
                break;
            }

        }

        // if(node.Attributes["l:href"] != null)
        // {
        //     a = node.Attributes["l:href"].Value;
        //     a = a.Remove(0, 1);
        // }
        // else if(node.Attributes["xlink:href"] != null)
        // {
        //     a = node.Attributes["xlink:href"].Value;
        //     a = a.Remove(0, 1);
        // }

        

        byte[] z = Convert.FromBase64String(BookFile.SelectSingleNode($"//*[@id = '{a}']").InnerText);
        

        tex.LoadImage(z);

        if(tex.width * tex.height > 60000)
        {
            File.WriteAllBytes(BookFolderPath + "/Images/hightCoverImage.jpg", tex.EncodeToJPG());

            TextureScale.Bilinear(tex, 200, 300);

            File.WriteAllBytes(BookFolderPath + "/Images/CoverImage.jpg", tex.EncodeToJPG());
        }
        else
        {
            File.WriteAllBytes(BookFolderPath + "/Images/CoverImage.jpg", tex.EncodeToJPG());
        }

        Resources.UnloadUnusedAssets();
    }

    void Lang(XmlNode node)
    {
        _BookDescription.Language = node.InnerText;
    }

    void Srclang(XmlNode node)
    {
        _BookDescription.SourceLanguage = node.InnerText;
    }


    void Translator (XmlNode node)
    {
        _BookDescription.Translators.Add(new Autor() { NameParts = new List<string>() });
        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods["T" + childNode.Name].Invoke(childNode);
        }
    }

    void TFirstName (XmlNode node)
    {
        _BookDescription.Translators[_BookDescription.Translators.Count-1].NameParts.Add(node.InnerText);
    }

    void TMiddleName (XmlNode node)
    {
        _BookDescription.Translators[_BookDescription.Translators.Count-1].NameParts.Add(node.InnerText);
    }

    void TLastName (XmlNode node)
    {
       _BookDescription.Translators[_BookDescription.Translators.Count-1].NameParts.Add(node.InnerText);
    }

    void TNickname (XmlNode node)
    {
       _BookDescription.Translators[_BookDescription.Translators.Count - 1].Nickname = " (" + node.InnerText + ")";
    }

    void TEmail (XmlNode node)
    {
       _BookDescription.Translators[_BookDescription.Translators.Count - 1].Email = "<link>" + node.InnerText + "</link>";
    }

    
    void THomePage (XmlNode node)
    {
       _BookDescription.Translators[_BookDescription.Translators.Count - 1].Homapage = "<link>" + node.InnerText + "</link>";
    }



    void Sequence (XmlNode node)
    {
        List<string> Sequence = new List<string>();
        foreach (XmlAttribute attribute in node.Attributes)
        {
            if(attribute.Name == "name" )
            {
                Sequence.Add("Серия: " + attribute.Value);
            }
            else if(attribute.Name == "number")
            {
                Sequence.Add("Номер: " + attribute.Value);
            }
        }

        _BookDescription.Sequence = string.Join("\n", Sequence);
    }




    void DocumentInfo (XmlNode node)
    {
        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods["DI" + childNode.Name].Invoke(childNode);
        }
    }

    void DIAuthor (XmlNode node)
    {
        _BookDescription.DIAuthors.Add(new Autor() { NameParts = new List<string>() });
        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods["DI" + childNode.Name].Invoke(childNode);
        }
    }

    void DIFirstName (XmlNode node)
    {
        _BookDescription.DIAuthors[_BookDescription.DIAuthors.Count-1].NameParts.Add(node.InnerText);
    }

    void DIMiddleName (XmlNode node)
    {
        _BookDescription.DIAuthors[_BookDescription.DIAuthors.Count-1].NameParts.Add(node.InnerText);
    }

    void DILastName (XmlNode node)
    {
       _BookDescription.DIAuthors[_BookDescription.DIAuthors.Count-1].NameParts.Add(node.InnerText);
    }

    void DINickname (XmlNode node)
    {
       _BookDescription.DIAuthors[_BookDescription.DIAuthors.Count - 1].Nickname = " (" + node.InnerText + ")";
    }

    void DIEmail (XmlNode node)
    {
       _BookDescription.DIAuthors[_BookDescription.DIAuthors.Count - 1].Email = "<link>" + node.InnerText + "</link>";
    }

    
    void DIHomePage (XmlNode node)
    {
       _BookDescription.DIAuthors[_BookDescription.DIAuthors.Count - 1].Homapage = "<link>" + node.InnerText + "</link>";
    }

    void DIProgramUsed (XmlNode node)
    {
        _BookDescription.ProgramUsed = node.InnerText;
    }

    void DIData (XmlNode node)
    {
        _BookDescription.DIData = node.InnerText;
    }
    
    void DISrcUrl (XmlNode node)
    {
        _BookDescription.SrcUrl = node.InnerText;
    }

    void DISrcOcr (XmlNode node)
    {
        _BookDescription.SrcOcr = node.InnerText;
    }

    void DIVersion (XmlNode node)
    {
        _BookDescription.Version = node.InnerText;
    }

    void DIHistory (XmlNode node)
    {
        foreach (XmlNode childNode in node.ChildNodes)
        {
            _BookDescription.History += childNode.InnerText + '\n';
        }
        
    }

    void DIPublisher (XmlNode node)
    {
        foreach (XmlNode childNode in node.ChildNodes)
        {
            _BookDescription.DIPublisher += childNode.InnerText + '\n';
        }
    }




    void PublishInfo (XmlNode node)
    {
        foreach (XmlNode childNode in node.ChildNodes)
        {
            Methods["PI" + childNode.Name].Invoke(childNode);
        }
    }

    void PIPublisher (XmlNode node)
    {
        _BookDescription.PIPublisher = node.InnerText;
    }

    void City (XmlNode node)
    {
        _BookDescription.City = node.InnerText;
    }

    void Year (XmlNode node)
    {
        _BookDescription.Year = node.InnerText;
    }

    void ISBN (XmlNode node)
    {
        _BookDescription.ISBN = node.InnerText;
    }

    void PISequence (XmlNode node)
    {
        _BookDescription.PISequence = node.InnerText;
    }


}
