using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class FileManager : MonoBehaviour
{
    public Library _Library;
    public GeneratorBook _GeneratorBook;

    public GameObject FilePrefab;

    public Transform Content;

    public List<string> PathParts = new List<string>();

    List<GameObject> Files = new List<GameObject>();

    public GameObject LoadWindow;

    public List<RectTransform> ExpButtons;

    void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !LoadWindow.activeSelf && gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    public void CloseWindow ()
    {
        gameObject.SetActive(false);
    }

    string GetPath ()
    {
        string path = string.Empty;

        foreach (string part in PathParts)
        {
            path += part + '/';
        }

        return path;
    }

    void Start ()
    {
        ExpMoveUp ("storage");
        ExpMoveUp ("emulated");
        ExpMoveUp ("0");

        Debug.Log(Application.consoleLogPath);

        LayoutRebuilder.ForceRebuildLayoutImmediate(ExpContent.GetComponent<RectTransform>());

        FilesShow ();
    }

    public void StartPath ()
    {
        PathParts.RemoveRange(PathParts.Count - 1, 1);

        float width = ExpButtons[ExpButtons.Count-1].rect.width;
        ExpContent.anchoredPosition = new Vector2(((ExpContent.rect.width - width - 10) * -0.5f) , 0);

        Destroy(ExpContent.GetChild(ExpContent.childCount-1).gameObject);
        ExpButtons.RemoveAt(ExpContent.childCount-1);
        FilesShow ();
    }

    public Button BackButton;

    public void FilesShow ()
    {
        foreach (GameObject file in Files)
        {
            Destroy(file);
        }

        if(PathParts.Count == 1)
        {
            BackButton.interactable = false;
        }
        else
        {
            BackButton.interactable = true;
        }

        string path = GetPath();

        foreach (string pathToFolder in Directory.EnumerateDirectories(path))
        {
            FileScript file = Instantiate(FilePrefab, Content).GetComponent<FileScript>();
            file._FileManager = this;
            file.File = false;
            Files.Add(file.gameObject);
            file.ButtonImage.color = Color.gray;
            file.Times = 0;

            string[] nameFolder = pathToFolder.Split('/');
            file.SetText(pathToFolder, nameFolder[nameFolder.Length-1]);
        }

        foreach (string pathToFolder in Directory.GetFiles(path, "*.fb2"))
        {
            FileScript file = Instantiate(FilePrefab, Content).GetComponent<FileScript>();
            file._FileManager = this;
            file.File = true;
            file.ButtonImage.color = new Color32(81, 136, 224, 255);

            Files.Add(file.gameObject);

            string[] nameFolder = pathToFolder.Split('/');
            file.SetText(pathToFolder, nameFolder[nameFolder.Length-1]);
        }

        foreach (string pathToFolder in Directory.GetFiles(path, "*.txt"))
        {
            FileScript file = Instantiate(FilePrefab, Content).GetComponent<FileScript>();
            file._FileManager = this;
            file.File = true;
            file.ButtonImage.color = new Color32(79, 196, 163, 255);

            Files.Add(file.gameObject);

            string[] nameFolder = pathToFolder.Split('/');
            file.SetText(pathToFolder, nameFolder[nameFolder.Length-1]);
        }

        foreach (string pathToFolder in Directory.GetFiles(path, "*.epub"))
        {
            FileScript file = Instantiate(FilePrefab, Content).GetComponent<FileScript>();
            file._FileManager = this;
            file.File = true;
            file.ButtonImage.color = new Color32(204, 143, 63, 255);

            Files.Add(file.gameObject);

            string[] nameFolder = pathToFolder.Split('/');
            file.SetText(pathToFolder, nameFolder[nameFolder.Length-1]);
        }
    }

    public RectTransform ExpContent;
    public RectTransform ExpContentView;
    public GameObject ExpButtonPrefab;

    public void ExpMoveUp (string pathPart)
    {
        PathParts.Add(pathPart);

        ExpButton expButton = Instantiate(ExpButtonPrefab, ExpContent).GetComponent<ExpButton>();
        ExpButtons.Add(expButton.GetComponent<RectTransform>());

        expButton._FileManager = this;

        expButton._Text.text = pathPart + " /";
        expButton.CatalogID = PathParts.Count-1;

        LayoutRebuilder.ForceRebuildLayoutImmediate(ExpContent.GetComponent<RectTransform>());
        Canvas.ForceUpdateCanvases();

        ExpContent.anchoredPosition = new Vector2(ExpContent.rect.width * -0.5f, 0);
    }
}
