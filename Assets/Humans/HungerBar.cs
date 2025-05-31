using UnityEngine;
using UnityEngine.UI;
public class HungerBar : MonoBehaviour
{
    public Slider hungerSlider;
    private HumanNeeds linkedNeeds;

    public void SetNeeds(HumanNeeds needs)
    {
        linkedNeeds = needs;

        if (hungerSlider == null)
            Debug.LogError("HungerSlider is NULL in HungerBar!");
        else
            hungerSlider.maxValue = needs.maxHunger;

        UpdateUI();

}
    public void UpdateUI()
    {
        if (linkedNeeds != null && hungerSlider != null)
        {
            hungerSlider.value = linkedNeeds.hunger;
        }
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