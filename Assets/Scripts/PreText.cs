using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreText : MonoBehaviour
{

    [HideInInspector]
    public List<string> _PreText = new List<string>();

    public List<Page> Pages = new List<Page>();

    int remStartID = 0;
    int remEndID = -1;

    public void GenerNubmeric ()
    {
        Pages.Add(new Page());

        

        int ReturneINT = 0;

        foreach(string a in _PreText)
        {
            ReturneINT++;

            foreach (char chars in a)
            {
                remEndID++;
            }

            Pages[Pages.Count-1].Steps.Add(new SpritzStep() { First = (ushort)remStartID, Last = (ushort)remEndID });
            
            remStartID = remEndID + 2;
            remEndID++;

            if(ReturneINT > 20)
            {
                break;
            }
        }
    }

    public void AddSpace ()
    {

    }
}
