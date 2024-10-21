using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;
    public enum TowerType { single, multi };

    [SerializeField] private GameObject[] towerPrefabs;
    [SerializeField] private GameObject dronePrefabs;
    [SerializeField] private Transform droneport;
    [SerializeField] private int droneCost = 1000;
    [SerializeField] private GameObject towerDetailUI;
    private int selectedTowerIndex = -1;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnDrone()
    {
        if (CurrencySystem.Instance.GetCurrentMoney() >= droneCost)
        {
            GameObject drone = Instantiate(dronePrefabs, transform.parent);
            drone.transform.position = droneport.position + new Vector3(0, 1, 0);
            drone.GetComponent<Drone>().SetDropPosition(droneport);
            CurrencySystem.Instance.SpendMoney(droneCost);
        }

        
    }

    public void ChangeSelectedTower(int index)
    {
        selectedTowerIndex = index;
    }

    public void OpenTowerDetailUI(GameObject tower, GameObject towerGround)
    {
        towerDetailUI.SetActive(true);
        towerDetailUI.GetComponent<TowerDetailManager>().ChangeSelectedTower(tower);
        towerDetailUI.GetComponent <TowerDetailManager>().ChangeSelectedTowerGround(towerGround);
    }

    public GameObject[] GetAllTowers()
    {
        return towerPrefabs;
    }

    public GameObject GetSelectedTower()
    {
        if (selectedTowerIndex == -1) { return null; }
        return towerPrefabs[selectedTowerIndex];
    }

    public Sprite GetPrebuildingImage()
    {
        if (selectedTowerIndex == -1) { return null; }
        return towerPrefabs[selectedTowerIndex].GetComponent<SpriteRenderer>().sprite;
    }
}
