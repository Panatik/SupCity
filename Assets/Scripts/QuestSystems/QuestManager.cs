using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    public List<QuestData> activeQuests = new List<QuestData>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddQuest(QuestData quest)
    {
        activeQuests.Add(quest);
        QuestUIManager.Instance?.RefreshUI();
    }

    public void ProgressQuest(string questId, int amount = 1)
    {
        QuestData quest = activeQuests.Find(q => q.id == questId);
        if (quest != null && !quest.isComplete)
        {
            quest.currentProgress += amount;
            QuestUIManager.Instance?.RefreshUI();
        }
    }

    public void CompleteQuest(string questId) 
    {
        Debug.Log($"Quête {questId} complétée");
    }

    public void ClearQuests()
    {
        activeQuests.Clear();
    }

}
