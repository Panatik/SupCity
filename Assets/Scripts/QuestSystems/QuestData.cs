using System.Globalization;
using UnityEngine;

public class QuestData
{
    public string id;
    public string title;
    public string description;
    public string rewards;
    //public Sprite icon;
    public int currentProgress;
    public int goal;


    public bool isComplete => currentProgress >= goal;
}
