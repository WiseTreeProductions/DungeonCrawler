using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Page
{
    public int index;
    public int nextPageIndex;
    [TextArea]
    public string text;
    public List<Response> responses;
    public string associatedSpeakerName;
    public Sprite associatedSpeakerImage;

}
