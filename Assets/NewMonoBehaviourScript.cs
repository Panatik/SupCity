using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public float speed = 80f;
    public float zoom_speed = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 5, 30);

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right  * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.up * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.down * speed * Time.deltaTime;
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            Camera.main.orthographicSize -= zoom_speed;
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            Camera.main.orthographicSize += zoom_speed;
        }
    }
}
