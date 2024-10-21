using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TowerGround : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private SpriteRenderer prebuildSr;
    [SerializeField] private Color hoverColor;
    [SerializeField] private GameObject towerRange;
    [SerializeField] private Sprite circle;
    [SerializeField] private Sprite square;

    [SerializeField] private GameObject tower;
    private Sprite prebuildingImage;
    private SpriteRenderer towerRangeRenderer;
    private Color originalColor;

    private void Start()
    {
        originalColor = sr.color;
        towerRangeRenderer = towerRange.GetComponent<SpriteRenderer>();
    }

    public void ChangeTower(GameObject newTower)
    {
        tower = newTower;
        SetTowerRange(tower);
    }

    private void SetTowerRange(GameObject tower)
    {
        if (tower == null) 
        {
            towerRange.transform.localScale = Vector3.zero;
            return; 
        }
        float attackRange = tower.GetComponent<Turret>().GetTowerRange();
        
        Turret.TowerAttackType towerType = tower.GetComponent<Turret>().GetTowerAttackType();
        switch (towerType)
        {
            case Turret.TowerAttackType.single:
                towerRangeRenderer.sprite = circle;
                towerRange.transform.localScale = new Vector3(attackRange * 2, attackRange * 2, 1);
                break;

            case Turret.TowerAttackType.multi:
                towerRangeRenderer.sprite = square;
                towerRange.transform.localScale = new Vector3(attackRange, attackRange, 1);
                break;
        }
    }

    public void UpdateTowerRange()
    {
        SetTowerRange(tower);
    }

    public void EnableTowerRange()
    {
        towerRangeRenderer.enabled = true;
    }

    public void DisableTowerRange()
    {
        towerRangeRenderer.enabled = false;
    }

    private void OnMouseEnter()
    {
        if (tower != null) { return; }

        prebuildingImage = BuildManager.Instance.GetPrebuildingImage();
        prebuildSr.sprite = prebuildingImage;
        SetTowerRange(BuildManager.Instance.GetSelectedTower());
        EnableTowerRange();
        sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        prebuildSr.sprite = null;
        sr.color = originalColor;
        if (tower == null) 
        {
            DisableTowerRange();
        }
    }

    private void OnMouseDown()
    {
        if (tower != null)
        {
            BuildManager.Instance.OpenTowerDetailUI(tower, gameObject);
            return;
        }

        GameObject towerPrefab = BuildManager.Instance.GetSelectedTower();
        if ( towerPrefab != null )
        {
            int cost = towerPrefab.GetComponent<Turret>().GetTowerCost();
            // 현재 가진 돈이 모자란 경우
            if (CurrencySystem.Instance.GetCurrentMoney() < cost)
            {
                // 나중에 UI 차원의 예외처리 바람!
                Debug.LogError("타워를 건설하기 위한 돈이 부족합니다!");
                return;
            }
            tower = Instantiate(towerPrefab, transform.position, Quaternion.identity);
            tower.GetComponent<Turret>().SetTowerGround(this);
            SetTowerRange(tower);
            BuildManager.Instance.OpenTowerDetailUI(tower, gameObject);
            CurrencySystem.Instance.SpendMoney(cost);
        }
    }
}
