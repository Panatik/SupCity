using UnityEngine;

public class HumanSelector : MonoBehaviour
{
    private Human currentSelected;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider != null)
            {

                Human clickedHuman = hit.collider.GetComponent<Human>();
                if (clickedHuman != null)
                {
                    Debug.Log("NPC cliqué : " + clickedHuman.name); 
                    if (currentSelected != null)
                        currentSelected.OnDeselected();

                    currentSelected = clickedHuman;
                    currentSelected.OnSelected();
                }
            }
        }
    }
}
