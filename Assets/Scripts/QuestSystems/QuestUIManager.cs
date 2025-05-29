using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIManager : MonoBehaviour
{
    public static QuestUIManager Instance;

    public GameObject questCardPrefab;
    public Transform questListParent;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RefreshUI()
    {
        Debug.Log("Instantiatinh QuestCard prefab");
        foreach (Transform child in questListParent)
            Destroy(child.gameObject);
        foreach(QuestData quest in QuestManager.Instance.activeQuests)
        {
            GameObject card = Instantiate(questCardPrefab, questListParent);

            //card.transform.Find("Icon").GetComponent<Image>().sprite = quest.icon;
            card.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = quest.title;
            card.transform.Find("Description").GetComponent<TextMeshProUGUI>().text=quest.description;
            card.transform.Find("Rewards").GetComponent<TextMeshProUGUI>().text = quest.rewards;
        }
    }
}
