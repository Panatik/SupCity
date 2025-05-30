using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    public List<QuestData> allQuests = new List<QuestData>();
    public List<QuestData> activeQuests = new List<QuestData>();
    private int currentQuestIndex = 0;
    private int maxVisibleQuests = 1;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {

        ShowNextQuests();
    }

    public void ShowNextQuests()
    {
        int count = 0;
        while (currentQuestIndex < allQuests.Count && activeQuests.Count < maxVisibleQuests && count < maxVisibleQuests)
        {
            QuestData nextQuest = allQuests[currentQuestIndex];
            activeQuests.Add(nextQuest);
            currentQuestIndex++;
            count++;
        }
        QuestUIManager.Instance?.RefreshUI();
    }

    public void ProgressQuest(string questId, int amount = 1)
    {
        QuestData quest = activeQuests.Find(q => q.id == questId);
        if (quest != null && !quest.isComplete)
        {
            quest.currentProgress += amount;
            if (quest.isComplete)
                CompleteQuest(questId);
            else
                QuestUIManager.Instance?.RefreshUI();
        }
    }

    public void CompleteQuest(string questId) 
    {
        Debug.Log($"Quête {questId} complétée");

        QuestData quest = activeQuests.Find(q => q.id == questId);
        if (quest != null)
        {
            activeQuests.Remove(quest);
            ShowNextQuests();
            QuestUIManager.Instance?.RefreshUI();
        }
    }

    public void ClearQuests()
    {
        activeQuests.Clear();
        currentQuestIndex = 0;
    }

}
