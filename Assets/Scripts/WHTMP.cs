using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using System.Globalization;


public class WHTMP : MonoBehaviour
{
    
    public TextMeshProUGUI m_TextMeshPro;

    char[] EndPunctuation = new char[] {
        ',', '.', '!', '?', '-', '—'
    };

    public SpritzScript spritzScript;

    void Start ()
    {
        m_TextMeshPro.ForceMeshUpdate();

        for (int i = 0; i < m_TextMeshPro.textInfo.wordCount; i++)
        {
            char putin = '\0';
            int idChar = m_TextMeshPro.textInfo.wordInfo[i].lastCharacterIndex + 1;
            if(m_TextMeshPro.textInfo.characterInfo[idChar].character == '.' || m_TextMeshPro.textInfo.characterInfo[idChar].character == ',')
            {
                putin = m_TextMeshPro.textInfo.characterInfo[m_TextMeshPro.textInfo.wordInfo[i].lastCharacterIndex + 1].character;
            }
        }
    }

    [HideInInspector]
    public int StepIDRem = -1;

    public void ChangeColorChars (int pageID, int stepID, Color32 color)
    {
        for (int i = spritzScript.Pages[pageID].Steps[stepID].First; i <= spritzScript.Pages[pageID].Steps[stepID].Last; ++i)
        {
            if(m_TextMeshPro.textInfo.characterInfo[i].character != ' ' || m_TextMeshPro.textInfo.characterInfo[i].character != '\t' || m_TextMeshPro.textInfo.characterInfo[i].character != '	')
            {
                int meshIndex = m_TextMeshPro.textInfo.characterInfo[i].materialReferenceIndex;
                int vertexIndex = m_TextMeshPro.textInfo.characterInfo[i].vertexIndex;
            
                Color32[] vertexColors = m_TextMeshPro.textInfo.meshInfo[meshIndex].colors32;
                vertexColors[vertexIndex + 0] = color;
                vertexColors[vertexIndex + 1] = color;
                vertexColors[vertexIndex + 2] = color;
                vertexColors[vertexIndex + 3] = color;
            }
            
        }

        m_TextMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int charIndex = TMP_TextUtilities.FindNearestCharacter(m_TextMeshPro, eventData.position, eventData.enterEventCamera, true);
        
        if(charIndex != -1)
        {
            if(StepIDRem != -1)
                ChangeColorChars (spritzScript.PageID, StepIDRem, spritzScript.Colors[1]);


            spritzScript.ClickWordInText(charIndex);
            
            
        }
    }

    public void ZoomWord(PointerEventData eventData)
    {
        int charIndex = TMP_TextUtilities.FindNearestCharacter(m_TextMeshPro, eventData.position, eventData.enterEventCamera, true);

        if(charIndex != -1)
        {
            spritzScript.SetZoomWordInText(charIndex);
        }
        else if(charIndex == -1)
        {
            spritzScript.ClearZoomWord();
        }

    }
}
