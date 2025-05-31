using UnityEngine;
using UnityEngine.UI;

public class HumanMenu : MonoBehaviour
{
    public Slider hungerSlider;
    public Slider thirstSlider;
    public Slider sleepSlider;

    private HumanNeeds needs;

    public void SetNeeds(HumanNeeds n)
    {
        needs = n;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (needs == null) return;

        hungerSlider.value = needs.hunger;
        thirstSlider.value = needs.thirst;
        sleepSlider.value = needs.sleep;
    }
}
