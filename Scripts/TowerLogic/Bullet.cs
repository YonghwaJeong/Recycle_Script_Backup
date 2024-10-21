using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] public float bulletDamage = 1f;
    [SerializeField] private Turret.TowerType towerType;

    // 충돌하지 않은 bullet이 맵에서 사라지게 함
    private float bulletLifeTime = 0f;
    private float bulletLifeTimeMax = 3f;

    [SerializeField] private Transform target;

    public void SetTarget(Transform givenTarget)
    {
        target = givenTarget;
    }

    public void SetDamage(float attackDamage)
    {
        bulletDamage = attackDamage;
    }

    private void FixedUpdate()
    {
        bulletLifeTime += Time.fixedDeltaTime;
        if (bulletLifeTime > bulletLifeTimeMax)
        {
            Destroy(gameObject);
        }
        if (!target) return;
        Vector2 bulletDirection = (target.position - transform.position).normalized;
        rb.velocity = bulletDirection * bulletSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        try
        {
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(bulletDamage, towerType);
        }
        catch (NullReferenceException)
        {
        }
        finally 
        {
            Destroy(gameObject);
        }
    }
}
