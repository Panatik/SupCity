using UnityEngine;

public class Human : MonoBehaviour
{
    public HungerBar hungerBarPrefab;
    private HungerBar hungerBarUI;
    private HumanNeeds needs;

    public float maxHunger = 100f;

    private void Start()
    {
        var canvas = FindObjectOfType<Canvas>().transform;
        hungerBarUI = Instantiate(hungerBarPrefab, canvas);

        needs = new HumanNeeds(maxHunger);
        hungerBarUI.SetNeeds(needs);
        hungerBarUI.Hide();
    }

    private void Update()
    {
        needs.UpdateHunger(1f);
        if (hungerBarUI.IsVisible())
            hungerBarUI.UpdateUI();
    }

    public void OnSelected()
    {
        hungerBarUI.Show();
    }

    public void OnDeselected()
    {
        hungerBarUI.Hide();
    }
}
