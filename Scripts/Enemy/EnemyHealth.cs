using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private float maxHP;
    private float currentHP;
    public bool isDie = false; //적이 사망 상태 -> isDie를 true
    private Enemy enemy;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private Movement2D m2D;
    public CurrencySystem.ResourceType resourceType;
    private Turret.TowerType lastHitTowerType;


    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    private void Awake()
    {
        currentHP = maxHP;
        enemy = GetComponent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        m2D = GetComponent<Movement2D>();
        resourceType = enemy.resourceType;
    }

    public void TakeDamage(float damage, Turret.TowerType towerType)
    {
        //적의 체력이 damage 만큼 감소해서 죽을 상황 때 여러타워 공격 동시에 받으면
        //enemy.OnDie() 함수가 여러번 실행될 수 있다.

        //현재 적의 상태가 사망 상태이면 아래 코드를 실행 x
        if (isDie == true) return;

        //현재 체력을 damage만큼 감소
        currentHP -= damage;

        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        if (currentHP <= 0)
        {
            GetComponent<Collider2D>().enabled = false;
            isDie = true;
            lastHitTowerType = towerType;
            m2D.moveSpeed = 0.0f;
            anim.SetTrigger("IsDead");
        }
    }
    public float getHP()
    {
        return currentHP;
    }

    public bool IsDead()
    {
        return isDie;
    }

    public void Dead()
    {
        switch (resourceType)
        {
            case CurrencySystem.ResourceType.Metal:
                AudioManager.instance.PlaySfx(AudioManager.Sfx.alumDead);
                break;
            case CurrencySystem.ResourceType.Paper:
                AudioManager.instance.PlaySfx(AudioManager.Sfx.paperDead);
                break;
            case CurrencySystem.ResourceType.Plastic:
                AudioManager.instance.PlaySfx(AudioManager.Sfx.plaDead);
                break;
            case CurrencySystem.ResourceType.Glass:
                AudioManager.instance.PlaySfx(AudioManager.Sfx.glaDead);
                break;
            case CurrencySystem.ResourceType.Food:
                AudioManager.instance.PlaySfx(AudioManager.Sfx.foodDead);
                break;
            default:
                break; 
        }
        enemy.OnDie(EnemyDestroyType.Kill, lastHitTowerType);
    }

    public void ExecutedByDrone()
    {
        enemy.OnDie(EnemyDestroyType.Kill, Turret.TowerType.drone);
    }

    public void GetAdditionalHealth(float value)
    {
        currentHP = Mathf.Clamp(currentHP + value, 0, maxHP);
    }

    private IEnumerator HitAlphaAnimation()
    {
        switch (resourceType)
        {
            case CurrencySystem.ResourceType.Metal:
                AudioManager.instance.PlaySfx(AudioManager.Sfx.alumHit);
                break;
            case CurrencySystem.ResourceType.Paper:
                AudioManager.instance.PlaySfx(AudioManager.Sfx.paperHit);
                break;
            case CurrencySystem.ResourceType.Plastic:
                AudioManager.instance.PlaySfx(AudioManager.Sfx.plaHit);
                break;
            case CurrencySystem.ResourceType.Glass:
                AudioManager.instance.PlaySfx(AudioManager.Sfx.glaHit);
                break;
            case CurrencySystem.ResourceType.Food:
                AudioManager.instance.PlaySfx(AudioManager.Sfx.foodHit);
                break;
            default:
                break;
        }
        // 현재 색 저장
        Color color = spriteRenderer.color;

        // 투명도 40%
        color.a = 0.4f;
        spriteRenderer.color = color;

        //0.05초 대기
        yield return new WaitForSeconds(0.05f);

        //투명도 100%
        color.a = 1.0f;
        spriteRenderer.color = color;
    }
}
