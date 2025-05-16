using UnityEngine;
using UnityEngine.Tilemaps;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField]
    private Tilemap map_renderer;
    
    private float mapMinX, mapMinY, mapMaxX, mapMaxY;

    public float speed = 5f;
    public float zoom_speed = 5f;

    public int min_zoom = 5;
    public int max_zoom = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        BoundsInt bounds = map_renderer.cellBounds;

        Vector3 min = map_renderer.CellToWorld(bounds.min);
        Vector3 max = map_renderer.CellToWorld(bounds.max);

        mapMinX = min.x;
        mapMaxX = max.x;
        mapMinY = min.y;
        mapMaxY = max.y;
    }
    void Update()
    {
        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movement += Vector3.left;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            movement += Vector3.right;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            movement += Vector3.up;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            movement += Vector3.down;
        }

        transform.position += movement * speed * Time.deltaTime;
        transform.position = ClampCamera(transform.position);



        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            Camera.main.orthographicSize -= zoom_speed;
            Camera.main.transform.position = ClampCamera(Camera.main.transform.position);
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            Camera.main.orthographicSize += zoom_speed;
            Camera.main.transform.position = ClampCamera(Camera.main.transform.position);
        }
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, min_zoom, max_zoom);
        Camera.main.transform.position = ClampCamera(Camera.main.transform.position);
    }

    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        float camHeight = Camera.main.orthographicSize;
        float camWidth = camHeight * Camera.main.aspect;

        float minX = mapMinX + camWidth;
        float maxX = mapMaxX - camWidth;
        float minY = mapMinY + camHeight;
        float maxY = mapMaxY - camHeight;

        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

        return new Vector3(newX, newY, targetPosition.z);
    }
}