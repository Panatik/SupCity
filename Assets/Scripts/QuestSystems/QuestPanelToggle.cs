using UnityEngine;

public class QuestPanelToggle : MonoBehaviour
{
    public GameObject questPanel; // Ton Panel qui contient la liste de qu�tes

    public void TogglePanel()
    {
        bool isActive = questPanel.activeSelf;
        questPanel.SetActive(!isActive);
        if (questPanel.activeSelf)
        {
            QuestUIManager.Instance.RefreshUI(); // Re-g�n�re la liste � chaque ouverture si besoin
        }
        
    }

}


