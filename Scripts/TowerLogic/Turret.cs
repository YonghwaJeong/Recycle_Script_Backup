using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
using UnityEngine.UIElements;
using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using JetBrains.Annotations;

public class Turret : MonoBehaviour
{
    public enum TowerAttackType { single, multi };

    // 예시로 만든 타입
    // 순서대로 압축기, 절단기, 소각기, 세척기
    public enum TowerType { press, cutter, incinerator, washer, drone, pig, human, all, None };
    public Animator animator;

    [SerializeField] private TowerAttackType towerAttackType;
    [SerializeField] private TowerType towerType;

    [SerializeField] private int towerCost = 100;
    [SerializeField] private int upgradeCost = 200;
    public int UpgradeCost => upgradeCost;
    [SerializeField] private float attackDamage = 0f;
    [SerializeField] private float attackPerSec = 1f;
    [SerializeField] private int attackcount = 1;

    // 타워의 회전하는 부분
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private float shootingRange = 3f;
    [SerializeField] private float rotationSpeed = 5f;

    private float initialAttackDamage;
    private float initialAttackPerSec;
    private float initialShootingRange;
    private int initialTowerCost;

    // 타워가 인식할 대상
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    
    [SerializeField] private GameObject nextTier;

    private Transform target;
    private float attackCooldown = 0f;
    private TowerGround ground;

    private AugmentSystem augmentSystem;

    private void Awake()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.build);
        initialAttackDamage = attackDamage;
        initialShootingRange = shootingRange;
        initialAttackPerSec = attackPerSec;
        initialTowerCost = towerCost;

        TowerManager.Instance.towers.Add(this);
        //TowerManager.Instance.CheckAugmentationAndApply(this);
    }

    private void LateUpdate()
    {
        
    }

    private void OnDestroy()
    {
        TowerManager.Instance.towers.Remove(this);
    }

    private void AugUpdate()
    {
        augmentSystem = AugmentSystem.instance;

        switch (towerType)
        {
            case TowerType.press:
                attackDamage = initialAttackDamage *
                    (1f + augmentSystem.PressADBuff + augmentSystem.ADBuff);
                attackPerSec = initialAttackPerSec *
                    (1f + augmentSystem.PressASBuff + augmentSystem.ASBuff);
                shootingRange = initialShootingRange *
                    (1f + augmentSystem.PressRBuff + augmentSystem.RBuff);
                towerCost = (int)((float)initialTowerCost *
                    (1f - augmentSystem.PressDC - augmentSystem.DC));
                break;

            case TowerType.cutter:
                attackDamage = initialAttackDamage *
                    (1f + augmentSystem.CutterADBuff + augmentSystem.ADBuff);
                attackPerSec = initialAttackPerSec *
                    (1f + augmentSystem.CutterASBuff + augmentSystem.ASBuff);
                shootingRange = initialShootingRange *
                    (1f + augmentSystem.CutterRBuff + augmentSystem.RBuff);
                towerCost = (int)((float)initialTowerCost *
                    (1f - augmentSystem.CutterDC - augmentSystem.DC));
                break;

            case TowerType.incinerator:
                attackDamage = initialAttackDamage *
                    (1f + augmentSystem.IncinADBuff + augmentSystem.ADBuff);
                attackPerSec = initialAttackPerSec *
                    (1f + augmentSystem.IncinASBuff + augmentSystem.ASBuff);
                shootingRange = initialShootingRange *
                    (1f + augmentSystem.IncinRBuff + augmentSystem.RBuff);
                towerCost = (int)((float)initialTowerCost *
                    (1f - augmentSystem.IncinDC - augmentSystem.DC));
                break;

            case TowerType.washer:
                attackDamage = initialAttackDamage *
                    (1f + augmentSystem.WasherADBuff + augmentSystem.ADBuff);
                attackPerSec = initialAttackPerSec *
                    (1f + augmentSystem.WasherASBuff + augmentSystem.ASBuff);
                shootingRange = initialShootingRange *
                    (1f + augmentSystem.WasherRBuff + augmentSystem.RBuff);
                towerCost = (int)((float)initialTowerCost *
                    (1f - augmentSystem.WasherDC - augmentSystem.DC));
                break;

            case TowerType.drone:
                attackDamage = initialAttackDamage *
                    (1f + augmentSystem.DroneADBuff + augmentSystem.ADBuff);
                attackPerSec = initialAttackPerSec *
                    (1f + augmentSystem.DroneASBuff + augmentSystem.ASBuff);
                shootingRange = initialShootingRange *
                    (1f + augmentSystem.DroneRBuff + augmentSystem.RBuff);
                towerCost = (int)((float)initialTowerCost *
                    (1f - augmentSystem.DroneDC - augmentSystem.DC));
                break;

            case TowerType.pig:
                attackDamage = initialAttackDamage *
                    (1f + augmentSystem.PigADBuff + augmentSystem.ADBuff);
                attackPerSec = initialAttackPerSec *
                    (1f + augmentSystem.PigASBuff + augmentSystem.ASBuff);
                shootingRange = initialShootingRange *
                    (1f + augmentSystem.PigRBuff + augmentSystem.RBuff);
                towerCost = (int)((float)initialTowerCost *
                    (1f - augmentSystem.PigDC - augmentSystem.DC));
                break;

            case TowerType.human:
                attackDamage = initialAttackDamage *
                    (1f + augmentSystem.HumanADBuff + augmentSystem.ADBuff);
                attackPerSec = initialAttackPerSec *
                    (1f + augmentSystem.HumanASBuff + augmentSystem.ASBuff);
                shootingRange = initialShootingRange *
                    (1f + augmentSystem.HumanRBuff + augmentSystem.RBuff);
                towerCost = (int)((float)initialTowerCost *
                    (1f - augmentSystem.HumanDC - augmentSystem.DC));
                break;

            default:
                attackDamage = initialAttackDamage *
                    (1f + augmentSystem.ADBuff);
                attackPerSec = initialAttackPerSec *
                    (1f + augmentSystem.ASBuff);
                shootingRange = initialShootingRange *
                    (1f + augmentSystem.RBuff);
                towerCost = (int)((float)initialTowerCost *
                    (1f - augmentSystem.DC));
                break;
        }
    }
    private void Update()
    {
        AugUpdate();

        if (target == null)
        {
            FindTarget();
            return;
        }

        // target이 있다면 지속적으로 target을 향해 rotate, attack
        if (towerAttackType == TowerAttackType.single)
        {
            //RotateTowardTarget();
        }

        if (Vector2.Distance(target.position, transform.position) > shootingRange)
        {
            target = null;
        } else
        {
            attackCooldown += Time.deltaTime;

            if (attackCooldown >= 1f / attackPerSec)
            {
                if (towerAttackType == TowerAttackType.single)
                {
                    Attack();
                }
                else if (towerAttackType == TowerAttackType.multi) 
                {
                    animator.SetTrigger("AttackTrigger");
                    RangeAttack();
                }
                attackCooldown = 0f;
            }
        }
    }

    public void SetTowerGround(TowerGround towerGround)
    {
        ground = towerGround;
    }

    public void UpdateTowerGround()
    {
        ground.UpdateTowerRange();
    }

    public int GetTowerCost()
    {
        return towerCost;
    }

    public GameObject GetNextTier()
    {
        return nextTier;
    }

    public float GetTowerRange()
    {
        return shootingRange;
    }

    public TowerAttackType GetTowerAttackType() 
    {
        return towerAttackType;
    }

    public TowerType GetTowerType()
    {
        return towerType;
    }

    public void ApplyAugmentation()
    {
        /*
        attackDamage = initialAttackDamage *
                    (1f + augmentSystem.ADBuff);
        attackPerSec = initialAttackPerSec *
            (1f + augmentSystem.ASBuff);
        shootingRange = initialShootingRange *
            (1f + augmentSystem.RBuff);
        towerCost = (int)((float)initialTowerCost *
            (1f - augmentSystem.DC));

        switch (towerType)
        {
            case TowerType.press:
                attackDamage = initialAttackDamage * 
                    (1f + augmentSystem.PressADBuff + augmentSystem.ADBuff);
                attackPerSec = initialAttackPerSec * 
                    (1f + augmentSystem.PressASBuff + augmentSystem.ASBuff);
                shootingRange = initialShootingRange * 
                    (1f + augmentSystem.PressRBuff + augmentSystem.RBuff);
                towerCost = (int)((float)initialTowerCost * 
                    (1f - augmentSystem.PressDC - augmentSystem.DC));
                break;

            case TowerType.cutter:
                attackDamage = initialAttackDamage * 
                    (1f + augmentSystem.CutterADBuff + augmentSystem.ADBuff);
                attackPerSec = initialAttackPerSec * 
                    (1f + augmentSystem.CutterASBuff + augmentSystem.ASBuff);
                shootingRange = initialShootingRange * 
                    (1f + augmentSystem.CutterRBuff + augmentSystem.RBuff);
                towerCost = (int)((float)initialTowerCost * 
                    (1f - augmentSystem.CutterDC - augmentSystem.DC));
                break;

            case TowerType.incinerator:
                attackDamage = initialAttackDamage * 
                    (1f + augmentSystem.IncinADBuff + augmentSystem.ADBuff);
                attackPerSec = initialAttackPerSec * 
                    (1f + augmentSystem.IncinASBuff + augmentSystem.ASBuff);
                shootingRange = initialShootingRange * 
                    (1f + augmentSystem.IncinRBuff + augmentSystem.RBuff);
                towerCost = (int)((float)initialTowerCost * 
                    (1f - augmentSystem.IncinDC - augmentSystem.DC));
                break;

            case TowerType.washer:
                attackDamage = initialAttackDamage * 
                    (1f + augmentSystem.WasherADBuff + augmentSystem.ADBuff);
                attackPerSec = initialAttackPerSec * 
                    (1f + augmentSystem.WasherASBuff + augmentSystem.ASBuff);
                shootingRange = initialShootingRange * 
                    (1f + augmentSystem.WasherRBuff + augmentSystem.RBuff);
                towerCost = (int)((float)initialTowerCost * 
                    (1f - augmentSystem.WasherDC - augmentSystem.DC));
                break;

            case TowerType.drone:
                attackDamage = initialAttackDamage *
                    (1f + augmentSystem.DroneADBuff + augmentSystem.ADBuff);
                attackPerSec = initialAttackPerSec *
                    (1f + augmentSystem.DroneASBuff + augmentSystem.ASBuff);
                shootingRange = initialShootingRange *
                    (1f + augmentSystem.DroneRBuff + augmentSystem.RBuff);
                towerCost = (int)((float)initialTowerCost *
                    (1f - augmentSystem.DroneDC - augmentSystem.DC));
                break;

            case TowerType.pig:
                attackDamage = initialAttackDamage *
                    (1f + augmentSystem.PigADBuff + augmentSystem.ADBuff);
                attackPerSec = initialAttackPerSec *
                    (1f + augmentSystem.PigASBuff + augmentSystem.ASBuff);
                shootingRange = initialShootingRange *
                    (1f + augmentSystem.PigRBuff + augmentSystem.RBuff);
                towerCost = (int)((float)initialTowerCost *
                    (1f - augmentSystem.PigDC - augmentSystem.DC));
                break;

            case TowerType.human:
                attackDamage = initialAttackDamage *
                    (1f + augmentSystem.HumanADBuff + augmentSystem.ADBuff);
                attackPerSec = initialAttackPerSec *
                    (1f + augmentSystem.HumanASBuff + augmentSystem.ASBuff);
                shootingRange = initialShootingRange *
                    (1f + augmentSystem.HumanRBuff + augmentSystem.RBuff);
                towerCost = (int)((float)initialTowerCost *
                    (1f - augmentSystem.HumanDC - augmentSystem.DC));
                break;

            default:
                attackDamage = initialAttackDamage *
                    (1f + augmentSystem.ADBuff);
                attackPerSec = initialAttackPerSec *
                    (1f + augmentSystem.ASBuff);
                shootingRange = initialShootingRange *
                    (1f + augmentSystem.RBuff);
                towerCost = (int)((float)initialTowerCost *
                    (1f - augmentSystem.DC));
                break;
        }
        */

        //attackPerSec =  initialAttackPerSec *(1f + augmentation.additionalAttackSpeed*0.05f);
        //shootingRange = initialShootingRange * (1f + augmentation.additionalAttackRange*0.05f);
    }

    public void ChangeAttackSpeed(float value)
    {
        attackPerSec = Mathf.Clamp(attackPerSec + value, 0.1f, attackPerSec + value);
    }

    public void ResetToNormalSpeed()
    {
        attackPerSec = initialAttackPerSec;
    }

    public void ResetToNormalRange()
    {
        shootingRange = initialShootingRange;
    }

    private void Attack()
    {
        GameObject bulletObject = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        bulletObject.GetComponent<Bullet>().SetDamage(attackDamage);
        bulletObject.GetComponent<Bullet>().SetTarget(target);
    }

    private void RangeAttack()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, new Vector2(shootingRange, shootingRange), 0f, Vector2.zero, Mathf.Infinity, enemyLayer);
        foreach (RaycastHit2D hit in hits) 
        { 
            hit.transform.GetComponent<EnemyHealth>().TakeDamage(attackDamage, towerType);
        }
    }


    private void FindTarget()
    {
        // 원형으로 캐스트를 수행하여, 충돌되는 대상을 hits에 배열로 저장
        if (towerAttackType == TowerAttackType.single)
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, shootingRange, Vector2.zero, 0f, enemyLayer);
            // 감지된 타격 대상이 있다면, 첫번째 대상을 타겟으로 지정
            if (hits.Length > 0)
            {
                target = hits[^1].transform;
            }
        }
        else if (towerAttackType == TowerAttackType.multi)
        {
            // 범위 + 다중타겟 공격은 일단 사격형만 존재한다고 가정
            RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, new Vector2(shootingRange, shootingRange), 0f, Vector2.zero, Mathf.Infinity, enemyLayer);
            if (hits.Length > 0)
            {
                target = hits[^1].transform;
            }
        }
    }

    private void RotateTowardTarget()
    {   
        // 위치 차를 이용해 각도를 계산하여 rad로 반환하고 deg 단위로 전환
        float angle = Mathf.Atan2(target.position.y - transform.position.y, 
            target.position.x - transform.position.x)* Mathf.Rad2Deg;
        Quaternion turretRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, turretRotation, 
            rotationSpeed * Time.deltaTime);
    }

    /*
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.forward, shootingRange);
    }
    */
}
