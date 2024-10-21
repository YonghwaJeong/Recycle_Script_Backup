using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Turret;

public class HumanAttack : MonoBehaviour
{
    [SerializeField] private float detectionRange = 2f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float attackPerSec;
    [SerializeField] private float damage = 2f;
    private float attackCooldown = 0f;
    private Transform target;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        DetectAndAttackEnemy();
    }

    private void DetectAndAttackEnemy()
    {
        if (target == null)
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, detectionRange, Vector2.zero, 0f, enemyLayer);
            if (hits.Length > 0)
            {
                target = hits[^1].transform;
            }
            return;
        }

        if (target.IsDestroyed())
        {
            target = null;
            return;
        } 
        else if (Vector2.Distance(target.position, transform.position) > detectionRange)
        {
            target = null;
        }
        else
        {
            attackCooldown += Time.deltaTime;

            if (attackCooldown >= 1f / attackPerSec)
            {
                animator.SetTrigger("Attack");
                target.GetComponent<EnemyHealth>().TakeDamage(damage, TowerType.human);
                attackCooldown = 0f;
            }
        }
    }

    public void ReduceAttackSpeed()
    {
        attackPerSec /= 2;
    }

    public void ResetAttackSpeed()
    {
        attackPerSec *= 2;
    }
}
