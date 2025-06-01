using UnityEngine;

public class Human : MonoBehaviour
{
    private HumanNeeds needs;

    public float maxHunger = 100f;
    public float maxThirst = 100f;
    public float maxSleep = 100f;

    private void Start()
    {
        needs = new HumanNeeds(maxHunger, maxThirst, maxSleep, this.gameObject);
    }

    private void Update()
    {
        needs.UpdateNeeds(1f, 1.5f, 0.5f);
    }

    public HumanNeeds GetNeeds()
    {
        return needs;
    }
}
