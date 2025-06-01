using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    public Vector2Int size = new Vector2Int(2, 2);
    public int objectIndex; // l’index dans placeableObjects

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnPlaced(int selectedIndex)
    {
        Debug.Log($"{selectedIndex} a été placé !");
        QuestManager.Instance.CompleteQuest(selectedIndex);
    }
}
