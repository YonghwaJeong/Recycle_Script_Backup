using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private float maxHP;
    private float currentHP;
    public bool isDie = false; //���� ��� ���� -> isDie�� true
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
        //���� ü���� damage ��ŭ �����ؼ� ���� ��Ȳ �� ����Ÿ�� ���� ���ÿ� ������
        //enemy.OnDie() �Լ��� ������ ����� �� �ִ�.

        //���� ���� ���°� ��� �����̸� �Ʒ� �ڵ带 ���� x
        if (isDie == true) return;

        //���� ü���� damage��ŭ ����
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
        // ���� �� ����
        Color color = spriteRenderer.color;

        // ���� 40%
        color.a = 0.4f;
        spriteRenderer.color = color;

        //0.05�� ���
        yield return new WaitForSeconds(0.05f);

        //���� 100%
        color.a = 1.0f;
        spriteRenderer.color = color;
    }
}
