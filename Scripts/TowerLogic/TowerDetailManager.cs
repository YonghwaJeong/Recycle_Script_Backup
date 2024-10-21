using System.Collections;
using System.Collections.Generic;
using TMPro;

//using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class TowerDetailManager : MonoBehaviour
{
    [SerializeField] float resellRatio = .5f;
    [SerializeField] private Image currentTierImage;
    [SerializeField] private Image nextTierImage;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI upgradeCostText;
    private GameObject selectedTower;
    private GameObject selectedTowerground;
    private GameObject nextTierTower;
    private int upgradeCost;

    public void ChangeSelectedTower(GameObject tower)
    {
        selectedTower = tower;
        currentTierImage.sprite = selectedTower.GetComponent<SpriteRenderer>().sprite;
        nextTierTower = selectedTower.GetComponent<Turret>().GetNextTier();
        if (nextTierTower != null )
        {
            upgradeButton.interactable = true;
            upgradeCost = tower.GetComponent<Turret>().UpgradeCost;
            upgradeCostText.text = "Cost : " + upgradeCost.ToString();
            nextTierImage.sprite = nextTierTower.GetComponent<SpriteRenderer>().sprite;
        } else
        {
            upgradeButton.interactable = false;
            nextTierImage.sprite = null;
            nextTierImage.color = new Color(0,0,0,0);
        }
    }

    public void ChangeSelectedTowerGround(GameObject towerGround)
    {
        if (selectedTowerground != null)
        {
            selectedTowerground.GetComponent<TowerGround>().DisableTowerRange();
        }
        selectedTowerground = towerGround;
        selectedTowerground.GetComponent<TowerGround>().EnableTowerRange();
    }

    public void OnCloseButtonPushed()
    {
        gameObject.SetActive(false);
        selectedTowerground.GetComponent<TowerGround>().DisableTowerRange();
    }

    public void ResetAllFocus() 
    {
        gameObject.SetActive(false);
        if (selectedTowerground)
        {
            selectedTowerground.GetComponent<TowerGround>().DisableTowerRange();
        }
        selectedTower = null;
        selectedTowerground = null;
    }

    public void SellTower()
    {
        int towerCost = selectedTower.GetComponent<Turret>().GetTowerCost();
        CurrencySystem.Instance.EarnMoney(Mathf.RoundToInt(towerCost * resellRatio));
        Destroy(selectedTower);
        selectedTowerground.GetComponent<TowerGround>().DisableTowerRange();
        gameObject.SetActive(false);
    }

    public void UpgradeTower()
    {
        
        if (CurrencySystem.Instance.GetCurrentMoney() >= upgradeCost)
        {
            CurrencySystem.Instance.SpendMoney(upgradeCost);
            Transform towerTransform = selectedTower.transform;
            Destroy(selectedTower);
            GameObject newTower = Instantiate(nextTierTower, towerTransform.parent);
            newTower.transform.position = towerTransform.position;
            selectedTowerground.GetComponent<TowerGround>().ChangeTower(newTower);
            ChangeSelectedTower(newTower);
        }
    }
}
