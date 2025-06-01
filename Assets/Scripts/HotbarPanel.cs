using UnityEngine;
using System.Collections.Generic;

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
    private int numerRoad = 0;
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
        Debug.Log("Show next prefab");
        if(questId == 3)
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

        if (questId == 1)
        {
            Instance.numerRoad++;
        }
        if (Instance.numerRoad == 4)
        {
            foreach (var prefab in Instance.prefabsQuest3)
            {
                prefab.SetActive(true);
            }
        }

        if (questId == 10)
        {
            Instance.berryPicker++;
        }
        if (Instance.berryPicker == 1)
        {
            foreach (var prefab in Instance.prefabsQuest5)
            {
                prefab.SetActive(true);
            }
        }

        if (questId == 1)
        {
            Instance.berryPicker++;
        }
        if (Instance.berryPicker == 1)
        {
            foreach (var prefab in Instance.prefabsQuest5)
            {
                prefab.SetActive(true);
            }
        }
    }
}
