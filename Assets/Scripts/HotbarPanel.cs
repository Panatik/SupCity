using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HotbarManager : MonoBehaviour
{
    public static HotbarManager Instance;

    [SerializeField] private List<GameObject> prefabsQuest1;
    [SerializeField] private List<GameObject> prefabsQuest2;
    [SerializeField] private List <GameObject> prefabsQuest3;
    [SerializeField] private List<GameObject> prefabsQuest5;
    [SerializeField] private List<GameObject> prefabsQuest9;
    [SerializeField] private List<GameObject> prefabsQuest11;
    [SerializeField] private List<GameObject> prefabsQuest16;

    private int numberHouse = 0;
    private int numberRoad = 0;
    private int berryPicker = 0;
    private int woodBuilding = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }



    private int currentIndex = 0;

    void Start()
    {
        foreach (var prefab in prefabsQuest1)
        {
            prefab.SetActive(false);
        }
        foreach (var prefab in prefabsQuest2)
        {
            prefab.SetActive(false);
        }
        foreach (var prefab in prefabsQuest3)
        {
            prefab.SetActive(false);
        }
        foreach (var prefab in prefabsQuest5)
        {
            prefab.SetActive(false);
        }
        foreach (var prefab in prefabsQuest9)
        {
            prefab.SetActive(false);
        }
        foreach (var prefab in prefabsQuest11)
        {
            prefab.SetActive(false);
        }
        foreach (var prefab in prefabsQuest16)
        {
            prefab.SetActive(false);
        }

    }

    public static void ShowNextPrefab(int questId)
    {
        if(questId == 27)
        {
            foreach (var prefab in Instance.prefabsQuest1)
            {
                prefab.SetActive(true);
            }
        }

        if (questId >=4 && questId <= 7)
        {
            Instance.numberHouse++;
        }
        if(Instance.numberHouse == 8)
        {
            foreach (var prefab in Instance.prefabsQuest2)
            {
                prefab.SetActive(true);
            }
        }

        if (questId >= 1 && questId <= 3)
        {
            Instance.numberRoad++;
        }
        if (Instance.numberRoad == 4)
        {
            foreach (var prefab in Instance.prefabsQuest3)
            {
                prefab.SetActive(true);
            }
        }

        if (questId == 9) {
            print("quest 5 validée");
            foreach (var prefab in Instance.prefabsQuest5) {
                prefab.SetActive(true);
            }
        }

        if (questId == 7) {
            print("quest 9 validée");
            foreach (var prefab in Instance.prefabsQuest9) {
                prefab.SetActive(true);
            }
        }

        if (questId == 8) {
            print("quest 11 validée");
            foreach (var prefab in Instance.prefabsQuest11) {
                prefab.SetActive(true);
            }
        }

        if (questId == 10) {
            print("quest 16 validée");
            foreach (var prefab in Instance.prefabsQuest16) {
                prefab.SetActive(true);
            }
        }
    }
}
