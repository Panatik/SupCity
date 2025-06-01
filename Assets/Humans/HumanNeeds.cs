using System;
using UnityEngine;

public class HumanNeeds
{
    public float hunger;
    public float maxHunger;

    public float thirst;
    public float maxThirst;

    public float sleep;
    public float maxSleep;

    private GameObject human;

    // ? Nouvel événement
    public event Action OnHumanDestroyed;

    public HumanNeeds(float maxHunger, float maxThirst, float maxSleep, GameObject human)
    {
        this.maxHunger = maxHunger;
        this.hunger = maxHunger;

        this.maxThirst = maxThirst;
        this.thirst = maxThirst;

        this.maxSleep = maxSleep;
        this.sleep = maxSleep;

        this.human = human;
    }

    public void UpdateNeeds(float rate, float thirstRate, float sleepRate)
    {
        hunger -= rate * Time.deltaTime;
        thirst -= thirstRate * Time.deltaTime;
        sleep -= sleepRate * Time.deltaTime;

        hunger = Mathf.Clamp(hunger, 0f, maxHunger);
        thirst = Mathf.Clamp(thirst, 0f, maxThirst);
        sleep = Mathf.Clamp(sleep, 0f, maxSleep);

        if (hunger <= 0 || thirst <= 0 || sleep <= 0)
        {
            if (human != null)
            {
                GameObject.Destroy(human);
                OnHumanDestroyed?.Invoke(); // ? Déclenche l'événement
            }
        }
    }

    public void Eat(float amount)
    {
        hunger = Mathf.Clamp(hunger + amount, 0f, maxHunger);
    }

    public void Drink(float amount)
    {
        thirst = Mathf.Clamp(thirst + amount, 0f, maxThirst);
    }

    public void Rest(float amount)
    {
        sleep = Mathf.Clamp(sleep + amount, 0f, maxSleep);
    }
}
