using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class TilePlacerUi : MonoBehaviour
{
    public enum ToolMode { Place, Delete, Move }
    public ToolMode currentMode = ToolMode.Place;

    public Tilemap targetTilemap;
    public List<TileBase> placeableTiles;
    public List<TileBase> wallTiles;

    private TileBase selectedTile = null;
    private GameObject selectedObject = null;

    public GameObject[] placeableObjects;
    public GameObject[] previewObjects;
    private GameObject currentPreview;

    private GameObject draggedObject;
    private Vector3 originalPosition;
    private bool isDragging = false;

    private int selectedIndex = -1;

    public void SetToolMode(int mode)
    {
        currentMode = (ToolMode)mode;

        // Supprime la preview quand on passe en Delete ou Move
        if (currentMode == ToolMode.Delete || currentMode == ToolMode.Move)
        {
            if (currentPreview)
            {
                Destroy(currentPreview);
                currentPreview = null;
            }
        }
    }

    public void SelectObject(int index)
    {
        selectedTile = null;

        // Active le mode Place automatiquement
        currentMode = ToolMode.Place;

        if (currentPreview) Destroy(currentPreview);

        selectedIndex = index;

        if (index >= 0 && index < placeableObjects.Length)
        {
            selectedObject = placeableObjects[index];
            currentPreview = Instantiate(previewObjects[index]);
            MakePreview(currentPreview);
        }
    }

    public void SelectTile(int index)
    {
        selectedObject = null;
        if (index >= 0 && index < placeableTiles.Count)
            selectedTile = placeableTiles[index];
    }

    void MakePreview(GameObject obj)
    {
        foreach (var sr in obj.GetComponentsInChildren<SpriteRenderer>())
            sr.color = new Color(0f, 0.5f, 1f, 0.5f); // bleu transparent

        Collider2D col = obj.GetComponent<Collider2D>();
        if (col) col.enabled = false;
    }

    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        Vector3 intersectionPos = SnapToGrid(mouseWorldPos);
        float cellSize = targetTilemap.cellSize.x;

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            switch (currentMode)
            {
                case ToolMode.Place:
                    if (selectedTile != null) PlaceTile(intersectionPos);
                    else if (selectedObject != null) PlaceObject(intersectionPos);
                    break;

                case ToolMode.Delete:
                    if (currentPreview)
                        Destroy(currentPreview);
                    HandleDelete();
                    break;

                case ToolMode.Move:
                    StartMove(intersectionPos);
                    break;
            }
        }

        if (isDragging)
        {
            currentPreview.transform.position = intersectionPos;

            if (Input.GetMouseButtonUp(0))
                EndMove(intersectionPos);
        }

        // La preview suit la souris si pas en déplacement
        if (!isDragging && currentPreview)
            currentPreview.transform.position = intersectionPos;
    }

    Vector3 SnapToGrid(Vector3 pos)
    {
        float cellSize = targetTilemap.cellSize.x;
        float x = Mathf.Round(pos.x / cellSize) * cellSize;
        float y = Mathf.Round(pos.y / cellSize) * cellSize;
        return new Vector3(x, y, 0f);
    }

    void PlaceTile(Vector3 worldPos)
    {
        Vector3Int cellPos = targetTilemap.WorldToCell(worldPos);
        TileBase currentTile = targetTilemap.GetTile(cellPos);

        if (!wallTiles.Contains(currentTile) && currentTile != null && !placeableTiles.Contains(currentTile))
            targetTilemap.SetTile(cellPos, selectedTile);
    }

    void PlaceObject(Vector3 worldPos)
    {
        float cellSize = targetTilemap.cellSize.x;
        Vector3Int baseCell = targetTilemap.WorldToCell(worldPos);
        baseCell.x -= 1;
        baseCell.y -= 1;

        Vector2 size = new Vector2(2f * cellSize, 2f * cellSize);
        if (CanPlaceAt(baseCell) && IsAreaClear(worldPos, size))
        {
            //Instantiate(selectedObject, worldPos, Quaternion.identity);
            GameObject placed = Instantiate(selectedObject, worldPos, Quaternion.identity);

            // Ajoute un identifiant à l'objet
            PlaceableObject id = placed.AddComponent<PlaceableObject>();
            id.objectIndex = selectedIndex;

            // Détruit l'ancienne preview
            if (currentPreview)
                Destroy(currentPreview);

            // Recrée la preview à la souris
            currentPreview = Instantiate(previewObjects[selectedIndex]);
            MakePreview(currentPreview);
        }
        else
        {
            Debug.Log("Impossible de placer ici !");
        }
    }

    void HandleDelete()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mousePos, LayerMask.GetMask("Buildings"));
        if (hit)
        {
            Debug.Log("On détruit: " + hit.gameObject.name);
            Destroy(hit.transform.root.gameObject);
        }

    }

    void StartMove(Vector3 worldPos)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mousePos, LayerMask.GetMask("Buildings"));

        if (hit)
        {
            if (currentPreview) Destroy(currentPreview);

            draggedObject = hit.transform.root.gameObject;
            originalPosition = draggedObject.transform.position;
            PlaceableObject id = draggedObject.GetComponent<PlaceableObject>();
            if (id != null && id.objectIndex >= 0 && id.objectIndex < previewObjects.Length)
            {
                currentPreview = Instantiate(previewObjects[id.objectIndex]);
            }
            currentPreview.transform.position = SnapToGrid(originalPosition);

            draggedObject.SetActive(false);
            isDragging = true;
        }
    }

    void EndMove(Vector3 worldPos)
    {
        float cellSize = targetTilemap.cellSize.x;
        Vector3Int baseCell = targetTilemap.WorldToCell(worldPos);
        baseCell.x -= 1;
        baseCell.y -= 1;

        Vector2 size = new Vector2(2f * cellSize, 2f * cellSize);

        bool canPlace = CanPlaceAt(baseCell);
        bool isClear = IsAreaClear(worldPos, size);

        Debug.Log($"CanPlaceAt = {canPlace}, IsAreaClear = {isClear}");

        if (CanPlaceAt(baseCell) && IsAreaClear(worldPos, size))
        {
            draggedObject.transform.position = worldPos;
            draggedObject.SetActive(true);
        }
        else
        {
            draggedObject.transform.position = originalPosition;
            draggedObject.SetActive(true);
            Debug.Log("Déplacement invalide, retour à l'origine");
        }

        Destroy(currentPreview);
        isDragging = false;
    }

    bool CanPlaceAt(Vector3Int cellPos)
    {
        BoundsInt bounds = new BoundsInt(cellPos.x, cellPos.y, 0, 2, 2, 1);
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            TileBase tile = targetTilemap.GetTile(pos);
            if (tile == null || wallTiles.Contains(tile))
                return false;
        }
        return true;
    }

    bool IsAreaClear(Vector3 center, Vector2 size)
    {
        if (currentPreview && currentPreview.GetComponent<Collider2D>())
            currentPreview.GetComponent<Collider2D>().enabled = false;

        Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, 0f, LayerMask.GetMask("Buildings"));
        foreach (var hit in hits)
        {
            if (hit.transform.root.gameObject != currentPreview)
                return false;
        }
        return true;
    }
}
