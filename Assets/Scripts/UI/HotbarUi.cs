using UnityEngine;
using UnityEngine.UI;

public class HotbarSwitcher : MonoBehaviour
{
    [Header("Références")]
    public GameObject hotbar1;
    public GameObject hotbar2;

    public Button boutonHotbar1;
    public Button boutonHotbar2;

    private void Start()
    {
        // Active la hotbar 1 au démarrage
        ActiverHotbar(1);

        // Ajoute les listeners
        boutonHotbar1.onClick.AddListener(() => ActiverHotbar(1));
        boutonHotbar2.onClick.AddListener(() => ActiverHotbar(2));
    }

    public void ActiverHotbar(int index)
    {
        bool activer1 = index == 1;

        hotbar1.SetActive(activer1);
        hotbar2.SetActive(!activer1);
    }
}
