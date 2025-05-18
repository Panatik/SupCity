using UnityEngine;
using UnityEngine.UI;
public class HungerBar : MonoBehaviour
{
    public Slider hungerSlider;
    public float maxHunger = 100f;

    private HumanNeeds humanNeeds;
    void Start()
    {
        humanNeeds = new HumanNeeds(maxHunger);
    }

    void Update()
    {
        humanNeeds.UpdateHunger(1f);
        hungerSlider.value = humanNeeds.hunger;

    }

    public void Eat(float amount)
    {
        humanNeeds.Eat(amount);
    }

}
