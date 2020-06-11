using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class GetLink : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI m_TextMeshPro;

    public bool TextColor;

    int linkIndex;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(linkIndex != -1)
        {
            ChangeColorChars(TextColorz.bottomLeft);
        }

        Fade = 0;

        linkIndex = TMP_TextUtilities.FindNearestLink(m_TextMeshPro, eventData.position, eventData.enterEventCamera);
        
        if(linkIndex != -1)
        {
            GUIUtility.systemCopyBuffer = m_TextMeshPro.textInfo.linkInfo[linkIndex].GetLinkText();

            ChangeColorChars(Color.grey);

            TextColor = true;
        }
    }

    public void ChangeColorChars (Color32 color)
    {
        for (int i = m_TextMeshPro.textInfo.linkInfo[linkIndex].linkTextfirstCharacterIndex; i <= m_TextMeshPro.textInfo.linkInfo[linkIndex].linkTextfirstCharacterIndex + m_TextMeshPro.textInfo.linkInfo[linkIndex].linkTextLength - 1; ++i)
        {
            if(char.IsWhiteSpace(m_TextMeshPro.textInfo.characterInfo[i].character) != true && m_TextMeshPro.textInfo.characterInfo[i].character != '\t' && m_TextMeshPro.textInfo.characterInfo[i].character != '	' &&  m_TextMeshPro.textInfo.characterInfo[i].character != ' ')
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

    float Fade;

    public TMP_ColorGradient TextColorz;

    void Update ()
    {
        if(TextColor)
        {
            Fade += Time.deltaTime;

            ChangeColorChars(new Color(Mathf.Lerp(Color.grey.r, TextColorz.bottomLeft.r, Fade), Mathf.Lerp(Color.grey.g, TextColorz.bottomLeft.g, Fade), Mathf.Lerp(Color.grey.b, TextColorz.bottomLeft.b, Fade), 1));

            if(Fade >= 1)
            {
                TextColor = false;
                Fade = 0;
            }
        }
    }
}
