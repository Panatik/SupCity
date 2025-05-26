using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNJ : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float idleCheckCooldown = 30f;

    public House assignedHouse;
    private List<Node> currentPath = new List<Node>();
    private int currentPathIndex = 0;
    private Node currentNode;

    private bool isIdle = false;
    private float idleCheckTimer = 0f;

    void Start()
    {

    }

    void Update()
    {
        idleCheckTimer += Time.deltaTime;

        // Répète toutes les 30s
        if (idleCheckTimer >= idleCheckCooldown)
        {
            idleCheckTimer = 0f;
            TryAssignOrGoIdle();
        }

        // Si pas en idle, on suit le chemin
        if (!isIdle && currentPath != null && currentPath.Count > 0 && currentPathIndex < currentPath.Count)
        {
            Vector3 targetPos = currentPath[currentPathIndex].transform.position;
            float step = moveSpeed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

            if (Vector3.Distance(transform.position, targetPos) < 0.05f)
            {
                currentPathIndex++;
                if (currentPathIndex >= currentPath.Count)
                {
                    IdleAction();
                }
            }
        }
    }

    void TryAssignOrGoIdle()
    {
        // Si pas encore de maison : on en cherche une libre
        if (assignedHouse == null)
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
            }
        }

        // Si on a une maison, on retourne y faire une action
        if (assignedHouse != null)
        {
            Node bestTargetNode = GetClosestNodeAmong(assignedHouse.GetOccupiedNodes());
            Node startNode = GetClosestNode(transform.position);

            if (bestTargetNode == null || startNode == null)
            {
                Debug.LogWarning($"[PNJ] Impossible de générer un chemin : Node de départ ou d’arrivée null. { bestTargetNode}  { startNode} {transform.position}");
                return;
            }

            currentPath = AStarManager.instance.GeneratePathTEST(startNode, bestTargetNode);
            currentPathIndex = 0;
            isIdle = false;
        }
    }

    Node GetClosestNode(Vector2 position)
    {
        Node[] allNodes = FindObjectsByType<Node>(FindObjectsSortMode.None);
        //Debug.Log($"[GetClosestNode] Il y a {allNodes.Length} nodes dans la scène.");

        Node closest = null;
        float minDist = Mathf.Infinity;

        foreach (Node node in allNodes)
        {
            if (node == null)
            {
                //Debug.LogWarning("[GetClosestNode] Un node dans la liste est null !");
                continue;
            }

            float dist = Vector2.Distance(position, node.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = node;
            }
        }

        if (closest == null)
        {
            Debug.LogWarning($"[GetClosestNode] Aucun node trouvé autour de {position}");
        }

        return closest;
    }

    Node GetClosestNodeAmong(List<Node> nodes)
    {
        Node closest = null;
        float minDist = Mathf.Infinity;

        foreach (Node node in nodes)
        {
            float dist = Vector2.Distance(transform.position, node.transform.position);
            if (dist < minDist)
            {
                closest = node;
                minDist = dist;
            }
        }
        //Debug.Log($"[GetClosestNode] Node maison : {closest.transform.position}");
        return closest;
    }

    void IdleAction()
    {
        isIdle = true;
        Debug.Log($"{name} est arrivé à sa maison ({assignedHouse.name}) et est en idle.");
        // Ici tu pourrais lancer une animation, etc.
    }
}
