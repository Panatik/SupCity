using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNJ : MonoBehaviour
{
    public float moveSpeed = 2f;

    public House assignedHouse;
    private List<Node> currentPath = new List<Node>();
    private int currentPathIndex = 0;
    private Node currentNode;

    private bool isIdle = false;

    public static PNJ Instance;

    void Start()
    {
        currentNode = GetClosestNode(transform.position);
    }

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (isIdle || currentPath == null || currentPath.Count == 0 || currentPathIndex >= currentPath.Count)
            return;

        Vector3 targetPos = currentPath[currentPathIndex].transform.position;
        float step = moveSpeed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

        if (Vector3.Distance(transform.position, targetPos) < 0.05f)
        {
            currentPathIndex++;
            if (currentPathIndex >= currentPath.Count)
            {
                // Arrivé
                IdleAction();
            }
        }
    }

    void AssignNearestHouse()
    {
        House[] houses = FindObjectsByType<House>(FindObjectsSortMode.None);
        House closest = null;
        float minDist = Mathf.Infinity;

        foreach (var house in houses)
        {
            if (!house.HasSpace()) continue;

            float dist = Vector2.Distance(transform.position, house.transform.position);
            if (dist < minDist)
            {
                closest = house;
                minDist = dist;
            }
        }

        if (closest != null)
        {
            assignedHouse = closest;
            assignedHouse.AddOccupant(this);
            Node targetNode = GetClosestNode(assignedHouse.GetEntrancePosition());
            currentPath = AStarManager.instance.GeneratePath(currentNode, targetNode);
            currentPathIndex = 0;
        }
    }

    Node GetClosestNode(Vector3 position)
    {
        Node[] allNodes = FindObjectsByType<Node>(FindObjectsSortMode.None);
        Node closest = null;
        float minDist = Mathf.Infinity;

        foreach (var node in allNodes)
        {
            float dist = Vector2.Distance(position, node.transform.position);
            if (dist < minDist && !node.IsBlocked)
            {
                closest = node;
                minDist = dist;
            }
        }

        return closest;
    }

    void IdleAction()
    {
        isIdle = true;
        Debug.Log($"{name} est arrivé à sa maison ({assignedHouse.name}) et est en idle.");
        // Animation, changement de sprite ou autre logique ici
    }

    public void OnHouseBuilt(House newHouse)
    {
        // Recherche tous les citoyens sans maison
        PNJ[] allCitizens = FindObjectsByType<PNJ>(FindObjectsSortMode.None);

        foreach (var citizen in allCitizens)
        {
            if (citizen.assignedHouse == null)
            {
                citizen.AssignNearestHouse();
            }
        }
    }
}
