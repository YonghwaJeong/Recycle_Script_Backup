using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;
    private Vector3 dropPosition;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private SpriteRenderer targetSprite;
    [SerializeField] private Sprite plastic;
    [SerializeField] private Sprite glass;

    private bool isWorking = false;
    private Transform target;

    void Update()
    {
        if (!isWorking)
        {
            FindClosestEnemy();
        }
    }

    private void FindClosestEnemy()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, Mathf.Infinity, Vector2.zero, 0, enemyLayer);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (RaycastHit2D hit in hits)
        {
            float distance = Vector2.Distance(transform.position, hit.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = hit.transform;
            }
        }

        if (closestDistance != Mathf.Infinity)
        {
            target = closestEnemy;
            StartCoroutine(WorkRoutine());
        }
        else
        {
            target = null;
        }
    }

    private IEnumerator WorkRoutine()
    {
        isWorking = true;
        while (Vector3.Distance(transform.position, target.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            if (!target.gameObject.activeSelf | (target.GetComponent<EnemyHealth>().getHP() <= 0))
            {
                target = null;
                isWorking = false;
                yield break;
            }
            yield return null;
        }

        if (target.GetComponent<Enemy>().GetRewardType() == CurrencySystem.ResourceType.Plastic)
        {
            targetSprite.sprite = plastic;
        } else if (target.GetComponent<Enemy>().GetRewardType() == CurrencySystem.ResourceType.Glass)
        {
            targetSprite.sprite = glass;
        }
        target.gameObject.SetActive(false);

        while (Vector3.Distance(transform.position, dropPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, dropPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        if (target != null)
        {
            target.GetComponent<EnemyHealth>().ExecutedByDrone();
        }
        targetSprite.sprite = null;
        target = null;
        isWorking = false;
    }

    public void SetDropPosition(Transform transform)
    {
        dropPosition = transform.position;
        dropPosition += new Vector3(0, 1, 0);
    }

    // 예비용으로 넣어둔 시작함수
    public void StartWorking()
    {
        isWorking = true;
    }

    // 예비용으로 넣어둔 스톱퍼
    public void StopWorking()
    {
        isWorking = false;
        StopAllCoroutines();
    }
}

