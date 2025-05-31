using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNJ : MonoBehaviour
{
    public float moveSpeed = 2f;
    public List<Node> currentPath = new List<Node>();
    private int currentPathIndex = 0;
    public Node currentNode;
    public Node targetNode;
    private SpriteRenderer spriteRenderer;

    // Gestion de la maison
    public House assignedHouse;
    public float idleCheckCooldown = 30f;
    private float idleCheckTimer = 0f;
    private bool isIdle = false;

    public Batiment assignedWork;

    // Pour savoir si le PNJ a été "forcé" de rentrer à la maison récemment
    private bool returningHomeByTimer = false;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Détection automatique du node de départ
        if (currentNode == null)
        {
            Collider2D hit = Physics2D.OverlapPoint(transform.position, LayerMask.GetMask("Nodes"));
            if (hit != null)
            {
                currentNode = hit.GetComponent<Node>();
            }
            else
            {
                Debug.LogWarning("No Node found under NPC's position!");
            }
        }

        // Si aucune maison assignée, on en cherche une
        if (assignedHouse == null)
        {
            TryAssignHouse();
        }

        // Si une maison a été trouvée, on commence par aller à la maison, sinon on se déplace aléatoirement
        if (assignedHouse != null)
        {
            GoToHouse();
        }
        else
        {
            PickRandomTargetAndGo();
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
            GoToHouse();
        }
    }

    void Update()
    {
        idleCheckTimer += Time.deltaTime;

        if (assignedHouse == null && isIdle)
        {
            Debug.Log($"{name} est idle mais n’a plus de maison. Il reprend son chemin.");
            isIdle = false;
            PickRandomTargetAndGo();
        }

        if (idleCheckTimer >= idleCheckCooldown)
        {
            idleCheckTimer = 0f;
            if (assignedHouse != null)
            {
                returningHomeByTimer = true;
                GoToHouse();
            }
            else
            {
                TryAssignHouse();
            }
        }

        // Déplacement sur le chemin si non idle
        if (!isIdle && currentPath != null && currentPath.Count > 0 && currentPathIndex < currentPath.Count)
        {
            FollowPath();
        }
    }

    void FollowPath()
    {
        if (currentPath == null || currentPath.Count == 0 || currentPathIndex >= currentPath.Count)
            return;

        Node nextNode = currentPath[currentPathIndex];
        Vector3 targetPos = nextNode.transform.position;
        float step = moveSpeed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

        // Flip du sprite selon la direction
        if (spriteRenderer != null)
        {
            Vector3 direction = (targetPos - transform.position).normalized;
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                spriteRenderer.flipX = (direction.x > 0);
            }
        }

        if (Vector2.Distance(transform.position, targetPos) < 0.05f)
        {
            currentNode = nextNode;
            currentPathIndex++;

            if (currentPathIndex >= currentPath.Count)
            {
                // Arrivé à destination...
                if (assignedHouse != null && assignedHouse.GetOccupiedNodes().Contains(targetNode))
                {
                    IdleAction();
                }
                else
                {
                    PickRandomTargetAndGo();
                }
            }
        }
    }

    void TryAssignHouse()
    {
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
                GoToHouse();
            }
        }
    }

    void GoToHouse()
    {
        // On choisit un node occupé de la maison comme destination
        if (assignedHouse == null) return;

        Node bestTargetNode = GetClosestNodeAmong(assignedHouse.GetOccupiedNodes());
        Node startNode = GetClosestNode(transform.position);

        if (bestTargetNode == null || startNode == null)
        {
            Debug.LogWarning($"[PNJ] Impossible de générer un chemin : Node de départ ou d’arrivée null. {bestTargetNode}  {startNode} {transform.position}");
            isIdle = true;
            return;
        }

        currentPath = AStarManager.instance.GeneratePath(startNode, bestTargetNode,true);
        targetNode = bestTargetNode;
        currentPathIndex = 0;
        isIdle = false;
    }

    void IdleAction()
    {
        isIdle = true;
        Debug.Log($"{name} est arrivé à sa maison ({assignedHouse.name}) et est en idle.");
        // Animation, etc. possible ici

        // Après un petit délai, le PNJ repart en déplacement aléatoire
        // Si retour forcé par timer, repartir en errance après un délai
        if (returningHomeByTimer)
        {
            returningHomeByTimer = false;
            StartCoroutine(ResumeRandomMoveAfterIdle(2f));
        }
        // Sinon, repartir en errance comme d’habitude
        else
        {
            StartCoroutine(ResumeRandomMoveAfterIdle(2f));
        }
    }

    System.Collections.IEnumerator ResumeRandomMoveAfterIdle(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        isIdle = false;
        PickRandomTargetAndGo();
    }

    void PickRandomTargetAndGo()
    {
        Node[] nodes = FindObjectsByType<Node>(FindObjectsSortMode.None);
        if (nodes.Length > 1)
        {
            Node newTarget;
            int tries = 0;
            do
            {
                newTarget = nodes[Random.Range(0, nodes.Length)];
                tries++;
            } while ((newTarget == currentNode || (newTarget.IsBlocked)) && tries < 50);

            if (newTarget != null && newTarget != currentNode && !newTarget.IsBlocked)
            {
                targetNode = newTarget;
                RecalculatePath();
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

        List<Node> newPath = AStarManager.instance.GeneratePath(currentNode, targetNode, true);
        if (newPath == null || newPath.Count == 0)
        {
            Debug.LogWarning("No valid path found for " + gameObject.name);
            return;
        }

        currentPath = newPath;
        currentPathIndex = 0;
        isIdle = false;
    }

    Node GetClosestNode(Vector2 position)
    {
        Node[] allNodes = FindObjectsByType<Node>(FindObjectsSortMode.None);

        Node closest = null;
        float minDist = Mathf.Infinity;

        foreach (Node node in allNodes)
        {
            if (node == null)
                continue;

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
        return closest;
    }

    public void ForceLeaveHouse()
    {
        if (assignedHouse != null)
        {
            Debug.Log($"{name} a été expulsé de {assignedHouse.name}");
            assignedHouse = null;
        }

        isIdle = false;
        returningHomeByTimer = false;
        PickRandomTargetAndGo();
    }

    public void AssignerTravail(Batiment batiment)
    {
        assignedWork = batiment;
        batiment.AjouterOuvrier(this);
    }

    // Debug pour voir le chemin dans l'éditeur
    private void OnDrawGizmos()
    {
        if (currentPath != null && currentPath.Count > 1)
        {
            Gizmos.color = Color.blue;
            for (int i = 1; i < currentPath.Count; i++)
            {
                Gizmos.DrawLine(currentPath[i - 1].transform.position, currentPath[i].transform.position);
            }
        }
    }
}