using UnityEngine;

public class MaisonClickHandler : MonoBehaviour
{
    private House maisonParent;

    private void Start()
    {
        maisonParent = GetComponentInParent<House>();
    }

    private void OnMouseDown()
    {
        if (maisonParent != null)
        {
            maisonParent.AfficherInfos();
        }
    }
}
