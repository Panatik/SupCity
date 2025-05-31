using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanNeeds
{
    public float hunger;
    public float maxHunger;

    public float thirst;
    public float maxThirst;

    public float sleep;
    public float maxSleep;

    public HumanNeeds(float maxHunger, float maxThirst, float maxSleep)
    {
        this.maxHunger = maxHunger;
        this.hunger = maxHunger;

        this.maxThirst = maxThirst;
        this.thirst = maxThirst;

        this.maxSleep = maxSleep;
        this.sleep = maxSleep;
    }

    public void UpdateNeeds(float rate, float thirstRate, float sleepRate)
    {
        hunger -= rate * Time.deltaTime;
        thirst -= thirstRate * Time.deltaTime;
        sleep -= sleepRate * Time.deltaTime;

        hunger = Mathf.Clamp(hunger, 0f, maxHunger);
        thirst = Mathf.Clamp(thirst, 0f, maxThirst);
        sleep = Mathf.Clamp(sleep, 0f, maxSleep);
    }
    public void Eat(float amount)
    {
        hunger += amount;
        hunger = Mathf.Clamp(hunger, 0f, maxHunger);
    }

    public void Drink(float amount)
    {
        thirst += amount;
        thirst = Mathf.Clamp(thirst + amount, 0f, maxThirst);
    }

    public void Rest(float amount)
    {
        sleep += amount;
        sleep = Mathf.Clamp(sleep + amount, 0f, maxSleep);
    }
}