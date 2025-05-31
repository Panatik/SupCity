using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class AStarManager : MonoBehaviour
{
    public static AStarManager instance;

    public void Awake()
    {
        instance = this;
    }
    void Start()
    {
        RebuildAllConnections(); // <<< INDISPENSABLE
    }

    public List<Node> GeneratePath(Node start, Node end, bool ignoreEndBlock)
    {
        List<Node> openSet = new List<Node>();

        foreach (Node n in FindObjectsByType<Node>(FindObjectsSortMode.None))
        {
            n.gScore = float.MaxValue;
        }

        start.gScore = 0;
        start.hScore = Vector2.Distance(start.transform.position, end.transform.position);

        openSet.Add(start);

        while (openSet.Count > 0)
        {
            int lowestF = default;

            for (int i = 0; i < openSet.Count; i++)
            {
                if (openSet[i].FScore() < openSet[lowestF].FScore())
                {
                    lowestF = i;
                }
            }

            Node currentNode = openSet[lowestF];
            openSet.Remove(currentNode);

            if (currentNode == end)
            {
                List<Node> path = new List<Node>();

                path.Insert(0, end);

                while (currentNode != start)
                {
                    currentNode = currentNode.cameFrom;
                    path.Add(currentNode);
                }

                path.Reverse();
                return path;
            }

            foreach (Node connectedNode in currentNode.connections)
            {
                if (connectedNode == null)
                    continue;

                if (connectedNode.IsBlocked && !connectedNode.isRoute && !(connectedNode == end && ignoreEndBlock))
                {
                    continue;
                }

                float heldGScore = currentNode.gScore + Vector2.Distance(currentNode.transform.position, connectedNode.transform.position);

                if (heldGScore < connectedNode.gScore)
                {
                    connectedNode.cameFrom = currentNode;
                    connectedNode.gScore = heldGScore;
                    connectedNode.hScore = Vector2.Distance(connectedNode.transform.position, end.transform.position);

                    if (!openSet.Contains(connectedNode))
                    {
                        openSet.Add(connectedNode);
                    }
                }
            }
        }

        return null;
    }

    public void RebuildAllConnections()
    {
        Node[] allNodes = FindObjectsByType<Node>(FindObjectsSortMode.None);

        foreach (Node node in allNodes)
        {
            node.connections.Clear();

            foreach (Node other in allNodes)
            {
                if (node == other || (other.IsBlocked && !other.isRoute))
                    continue;

                Vector2 dir = other.transform.position - node.transform.position;

                // Connexions orthogonales uniquement (1 unité de distance verticale ou horizontale, pas les deux)
                if ((Mathf.Abs(dir.x) == 1 && dir.y == 0) || (Mathf.Abs(dir.y) == 1 && dir.x == 0))
                {
                    node.connections.Add(other);
                }
            }
        }

        Debug.Log("Orthogonal connections rebuilt.");
    }
}
