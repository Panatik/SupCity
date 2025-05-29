using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HouseUi : MonoBehaviour
{
    public static HouseUi Instance;

    public GameObject menuMaison;
    public TMP_Text nomText;
    public TMP_Text descriptionText;
    public Image imageMaison;
    public TMP_Text levelText;
    public TMP_Text occupantsText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        menuMaison.SetActive(false);
    }

    public void AfficherMenuMaison(House maison)
    {
        nomText.text = maison.houseName;
        descriptionText.text = maison.description;
        imageMaison.sprite = maison.house;
        levelText.text = maison.houseLevel.ToString();
        occupantsText.text = maison.maxOccupants.ToString();

        menuMaison.SetActive(true);
    }

    public void FermerMenuMaison()
    {
        menuMaison.SetActive(false);
    }
}