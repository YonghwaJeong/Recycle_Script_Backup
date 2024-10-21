using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnhandledInputManager : MonoBehaviour
{
    [SerializeField] private TowerDetailManager towerManager;
    [SerializeField] private BuildManagerUI buildManagerUI;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            // TowerGround �ܿ� collider�� ������ object�� ����ٸ� ������ ��
            if (hit.collider == null)
            {
                towerManager.ResetAllFocus();
                buildManagerUI.ResetOutline();
                BuildManager.Instance.ChangeSelectedTower(-1);
            }
        }
    }
}
