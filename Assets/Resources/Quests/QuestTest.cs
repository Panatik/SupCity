using UnityEngine;

public class QuestTest : MonoBehaviour
{
    public Sprite Quest_icon2;
    private void Start()
    {
        QuestData campfireQuest = new QuestData
        {
            id = "build_fire",
            title = "Allumer un feu de camp",
            description = "Construis ton premier feu de camp",
            //icon = Quest_icon2,
            goal = 1,
            currentProgress = 0,
        };
        QuestManager.Instance.AddQuest(campfireQuest);
    }

    public void OnCampFireBuilt()
    {
        QuestManager.Instance.ProgressQuest("build_fire");
    }

}
