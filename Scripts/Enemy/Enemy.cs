using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyDestroyType { Kill = 0, Arrive }

public class Enemy : MonoBehaviour
{
    private int                 wayPointCount;      // �̵� ��� ����
    private Transform[]         wayPoints;          // �̵� ��� ����
    private int                 currentIndex = 0;   // ���� ��ǥ���� �ε���
    private Movement2D          movement2D;         // ������Ʈ �̵� ����
    private EnemySpawner        enemySpawner;       // ���� ������ ������ ���� �ʰ� EnemySpanwer�� �˷��� ����

    // ����� ��ġ ����
    [SerializeField] private int reward = 2;
    public CurrencySystem.ResourceType resourceType;
    [SerializeField] private Turret.TowerType negativeTowerType;
    [SerializeField] private DebuffCode debuffCode;

    public void Setup(EnemySpawner enemySpawner,Transform[] wayPoints)
    {
        movement2D = GetComponent<Movement2D>();
        this.enemySpawner = enemySpawner;

        // �� �̵� ��� WayPoints ���� ����
        wayPointCount = wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount];
        this.wayPoints = wayPoints;

        // ���� ��ġ�� ù��° wayPoints ��ġ�� ����
        transform.position = wayPoints[currentIndex].position;

        // �� �̵�/��ǥ���� ���� �ڷ�ƾ �Լ� ����
        StartCoroutine("OnMove");
    }

    private IEnumerator OnMove()
    {
        // ���� �̵� ���� ����
        NextMoveTo();

        while (true)
        {
            // �� ������Ʈ ȸ��
            //transform.Rotate(Vector3.forward * 10);

            // ���� ���� ��ġ�� ��ǥ��ġ�� �Ÿ��� 0.02 * movement2D.MoveSpeed���� ���� �� if ���ǹ� ����
            // Tip. movement2D.MoveSpeed�� �����ִ� ������ �ӵ��� ������ �� �����ӿ� 0.02���� ũ�� �����̱� ������
            // if ���ǹ��� �ɸ��� �ʰ� ��θ� Ż���ϴ� ������Ʈ�� �߻��� �� �ִ�.
            if ( Vector3.Distance(transform.position, wayPoints[currentIndex].position) < 0.02f * movement2D.MoveSpeed )
            {
                // ���� �̵� ���� ����
                NextMoveTo();
            }

            yield return null;
        }
    }

    private void NextMoveTo()
    {
        // ���� �̵��� wayPoints�� �����ִٸ�
        if ( currentIndex < wayPointCount - 1)
        {
            // ���� ��ġ�� ��Ȯ�ϰ� ��ǥ ��ġ�� ����
            transform.position = wayPoints[currentIndex].position;
            // �̵� ���� ���� => ���� ��ǥ����(wayPoints)
            currentIndex++;
            Vector3 direction = (wayPoints[currentIndex].position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        // ���� ��ġ�� ������ wayPoints�̸�
        else
        {
            // �� ������Ʈ ����
            OnDie(EnemyDestroyType.Arrive);
        }
    }

    public void OnDie(EnemyDestroyType type, Turret.TowerType lastHitTowerType = Turret.TowerType.None)
    {
        // EnemySpawner���� ����Ʈ�� �� ������ �����ϱ� ������ Destroy()�� �����ϱ� �ʰ�
        // EnemySpanwer���� ������ ������ �� �ʿ��� ó���� �ϵ��� DestoryEnemy() �Լ� ȣ��
        if (lastHitTowerType != Turret.TowerType.None && lastHitTowerType == negativeTowerType)
        {
            enemySpawner.OnDebuffCondition(transform.position, debuffCode);
        }
        enemySpawner.DestroyEnemy(type, this);
    }

    // Serializable�� �̿��� private�ϰ� ��ȣ�ϸ� �����Ϳ��� ���� + public�ϰ� �����ϱ� ���� �޼��� 
    public int GetRewardValue()
    {
        return reward;
    }

    public CurrencySystem.ResourceType GetRewardType()
    {
        return resourceType;
    }
}
