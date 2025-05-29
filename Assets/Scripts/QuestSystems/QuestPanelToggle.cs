using UnityEngine;

public class QuestPanelToggle : MonoBehaviour
{
    public GameObject questPanel; // Ton Panel qui contient la liste de quêtes

    public void TogglePanel()
    {
        bool isActive = questPanel.activeSelf;
        questPanel.SetActive(!isActive);
        if (questPanel.activeSelf)
        {
            QuestUIManager.Instance.RefreshUI(); // Re-génère la liste à chaque ouverture si besoin
        }
        
    }

}


