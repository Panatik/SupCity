<<<<<<< HEAD
using System.Collections.Generic;
using UnityEngine;
=======
ï»¿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
>>>>>>> 8634496 (implementation of humans with moving animation and hunger bar)

public class NPC_Controller : MonoBehaviour
{
    public Node currentNode;
    public List<Node> path = new List<Node>();
    private SpriteRenderer spriteRenderer;
<<<<<<< HEAD
    public Node targetNode;

    private int currentPathIndex = 0;
    public float speed = 3f;
=======
>>>>>>> 8634496 (implementation of humans with moving animation and hunger bar)

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
<<<<<<< HEAD

        // Si currentNode n'est pas défini, on le détecte automatiquement
        if (currentNode == null)
        {
            Collider2D hit = Physics2D.OverlapPoint(transform.position, LayerMask.GetMask("Nodes"));
            if (hit != null)
            {
                currentNode = hit.GetComponent<Node>();
                Debug.Log("Current Node auto-detected: " + currentNode.name);
            }
            else
            {
                Debug.LogWarning("No Node found under NPC's position!");
            }
        }

        // Trouve un target aléatoire pour commencer
        Node[] nodes = FindObjectsByType<Node>(FindObjectsSortMode.None);
        if (nodes.Length > 0)
        {
            targetNode = nodes[Random.Range(0, nodes.Length)];
            RecalculatePath();
        }
=======
>>>>>>> 8634496 (implementation of humans with moving animation and hunger bar)
    }

    private void Update()
    {
        FollowPath();
    }

    private void FollowPath()
    {
<<<<<<< HEAD
        if (path == null || path.Count == 0 || currentPathIndex >= path.Count)
            return;
=======
        if (path.Count > 0)
        {
            int x = 0;
            Vector3 targetPos = new Vector3(path[x].transform.position.x, path[x].transform.position.y, -2);
            Vector3 direction = (targetPos - transform.position).normalized;

            transform.position = Vector3.MoveTowards(transform.position, targetPos, 3 * Time.deltaTime);

            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                if (direction.x > 0)
                    spriteRenderer.flipX = true;
                else if (direction.x < 0)
                    spriteRenderer.flipX = false; 
            }
>>>>>>> 8634496 (implementation of humans with moving animation and hunger bar)

        Node nextNode = path[currentPathIndex];
        Vector3 targetPos = new Vector3(nextNode.transform.position.x, nextNode.transform.position.y, 0);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        Vector3 direction = (targetPos - transform.position).normalized;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) 
        {
            if (direction.x > 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (direction.x < 0) 
            {
                spriteRenderer.flipX = false;
            }
        }

        if (Vector2.Distance(transform.position, nextNode.transform.position) < 0.1f)
        {
            currentNode = nextNode;
            currentPathIndex++;

            if (currentPathIndex >= path.Count)
            {
                // Arrivé à destination : choisir une nouvelle destination
                Node[] nodes = FindObjectsByType<Node>(FindObjectsSortMode.None);
                if (nodes.Length > 1)
                {
                    Node newTarget;
                    do
                    {
                        newTarget = nodes[Random.Range(0, nodes.Length)];
                    } while (newTarget == currentNode || newTarget.IsBlocked);

                    targetNode = newTarget;
                    RecalculatePath();
                }
            }
        }
    }

    public void RecalculatePath()
    {
        if (currentNode == null || targetNode == null)
        {
            Debug.LogWarning("Missing start or target node.");
            return;
        }

        //Debug.Log($"Recalculating path from {currentNode.name} to {targetNode.name}");

        List<Node> newPath = AStarManager.instance.GeneratePath(currentNode, targetNode);
        if (newPath == null || newPath.Count == 0)
        {
            Debug.LogWarning("No valid path found for " + gameObject.name);
            return;
        }

        //Debug.Log("Path successfully calculated with " + newPath.Count + " nodes.");

        path = newPath;
        currentPathIndex = 0;
    }

    private void OnDrawGizmos()
    {
<<<<<<< HEAD
        if (path != null && path.Count > 1)
=======
        if (path.Count > 0)
>>>>>>> 8634496 (implementation of humans with moving animation and hunger bar)
        {
            Gizmos.color = Color.blue;
            for (int i = 1; i < path.Count; i++)
            {
                Gizmos.DrawLine(path[i - 1].transform.position, path[i].transform.position);
            }
        }
    }
}
