using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildManagerUI : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    void Start()
    {
        GameObject[] towers = BuildManager.Instance.GetAllTowers();
        foreach (GameObject tower in towers)
        {
            GameObject newSlotObject = Instantiate(slotPrefab, transform);
            newSlotObject.transform.GetChild(1).GetComponent<Image>().sprite = tower.GetComponent<SpriteRenderer>().sprite;
            newSlotObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = tower.GetComponent<Turret>().GetTowerCost().ToString();
        }
    }

    private void Update()
    {
    }

    public void ResetOutline()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetChild(0).GetComponent<Outline>().enabled = false;
        }
    }
}
