using UnityEngine;
using UnityEngine.UI;
public class HungerBar : MonoBehaviour
{
    public Slider hungerSlider;
    private HumanNeeds linkedNeeds;

    public void SetNeeds(HumanNeeds needs)
    {
        linkedNeeds = needs;
        hungerSlider.maxValue = needs.maxHunger;
        UpdateUI();
    }
    public void UpdateUI()
    {
        if (linkedNeeds != null)
            hungerSlider.value = linkedNeeds.hunger;
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public bool IsVisible()
    {
        return gameObject.activeSelf;
    }
}