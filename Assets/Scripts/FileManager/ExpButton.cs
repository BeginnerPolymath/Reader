using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpButton : MonoBehaviour
{
    public FileManager _FileManager;

    public Text _Text;

    public int CatalogID;

    public void Click ()
    {
        for (int i = 0; i < _FileManager.ExpButtons.Count; i++)
        {
            if(i > CatalogID)
            {
                
                DestroyImmediate(_FileManager.ExpButtons[i].gameObject);
                _FileManager.ExpButtons.RemoveAt(i);
                _FileManager.PathParts.RemoveAt(i);
                i--;
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(_FileManager.ExpContent.GetComponent<RectTransform>());
        Canvas.ForceUpdateCanvases();
        
        _FileManager.ExpContent.anchoredPosition = new Vector2(_FileManager.ExpContent.rect.width * -0.5f, 0);
        
        _FileManager.FilesShow();
    }
}
