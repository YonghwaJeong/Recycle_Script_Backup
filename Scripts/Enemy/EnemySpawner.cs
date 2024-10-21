using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
    //[SerializeField]
    //private GameObject enemyPrefab; // �� ������
    [SerializeField]
    private GameObject enemyHPSliderPrefab; // �� ü���� ��Ÿ���� Slider UI ������
    [SerializeField]
    private Transform canvasTransform; // UI�� ǥ���ϴ� Canvas ������Ʈ�� Transform
    //[SerializeField]
    //private float spawnTime; // �� ���� �ֱ�
    [SerializeField]
    private Transform[] wayPoints; // ���� ���������� �̵� ���
    [SerializeField]
    private PollutionDegree playerP; // �÷��̾� ������ ������Ʈ
    [SerializeField]
    private Wave currentWave; // ���� ���̺� ����
    private int currentEnemyCount; // ���� ���̺꿡 ���� �ִ� �� ���� 
    private List<Enemy> enemyList; // ���� �ʿ� �����ϴ� ��� ���� ����
    
    [SerializeField] private WaveSystem waveSystem;
    [SerializeField] private AugmentSystem augmentSystem;
    [SerializeField] private DebuffManager debuffManager;
    private int waveMaxCount;
    private int currentWaveCount = 0;

    // Ư�� ���ǿ� ���� �̺�Ʈ
    [SerializeField] private UnityEvent gameClear;
    [SerializeField] private UnityEvent onWaveStarts;
    [SerializeField] private UnityEvent onWaveEnds;

    // ���� ������ ������ EnemySpanwer���� �ϱ� ������ Set�� �ʿ� ����.
    public List<Enemy> EnemyList => enemyList;
    public int CurrentEnemyCount => currentEnemyCount;
    public int MaxEnemyCount => currentWave.maxEnemyCount;

    private void Awake()
    {
        // �� ����Ʈ �޸� �Ҵ�
        enemyList = new List<Enemy>();
        // �� ���� �ڷ�ƾ �Լ� ȣ��
        //StartCoroutine("SpawnEnemy");

        // ���� ���� ������ �����ϱ� ���� wave ���� ����
        waveMaxCount = waveSystem.MaxWave;
    }

    public void StartWave(Wave wave)
    {
        // �Ű������� ���̺� ���� ����
        currentWave = wave;
        currentWaveCount += 1;
        // ���� ���̺��� �ִ� �� ���ڸ� ����
        currentEnemyCount = currentWave.maxEnemyCount;
        // ���� ���̺� ����
        StartCoroutine("SpawnEnemy");

        // ���̺� ���۽� ������ ���׵鿡 ���� event�� invoke
        onWaveStarts.Invoke();
    }

    private IEnumerator SpawnEnemy()
    {
        int spawnEnemyCount = 0;

        while(spawnEnemyCount < currentWave.maxEnemyCount)
        {
            // ���̺꿡 �����ϴ� ���� ������ ���� ������ �� ������ ���� �����ϵ��� �����ϰ�, �� ������Ʈ ����
            int enemyIndex = UnityEngine.Random.Range(0, currentWave.enemyPrefabs.Length);
            GameObject clone = Instantiate(currentWave.enemyPrefabs[enemyIndex]);
            Enemy enemy = clone.GetComponent<Enemy>(); // ��� ������ ���� Enemy ������Ʈ

            // this�� �� �ڽ� (�ڽ��� EnemySpawner ����)
            enemy.Setup(this, wayPoints);
            enemyList.Add(enemy);

            SpawnEnemyHPSlider(clone);

            spawnEnemyCount++;

            // �� ���̺꺰 spawnTime ����, ���� ���̺�(currentWave)�� spawnTime ���
            yield return new WaitForSeconds(currentWave.spawnTime); // spawnTime �ð� ���� ���
        }
    }


    /*
    private IEnumerator SpawnEnemy()
    {
        while(true)
        {
            GameObject clone = Instantiate(enemyPrefab); // �� ������Ʈ ����
            Enemy enemy = clone.GetComponent<Enemy>(); // ��� ������ ���� Enemy ������Ʈ

            enemy.Setup(this, wayPoints); // wayPoint ������ �Ű������� Setup() ȣ��
            enemyList.Add(enemy);

            SpawnEnemyHPSlider(clone);

            yield return new WaitForSeconds(spawnTime); // spawnTime �ð� ���� ���
        }
    }
    */ //���� ���� Spawn Enemy �Լ�


    public void DestroyEnemy(EnemyDestroyType type, Enemy enemy)
    {
        if (type == EnemyDestroyType.Arrive)
        {
            // �÷��̾��� ������ +1
            playerP.TakeDamage(1);
        }
        // �� óġ�� ���� ��ȭ ȹ��
        if (type == EnemyDestroyType.Kill)
        {
            CurrencySystem.Instance.GetResource(enemy.GetRewardType(), enemy.GetRewardValue());
            
            // �ӽ÷� ���Ǿ��� ȹ��
            augmentSystem.AddAugmentGauge();
        }
        // �� ��� ������ ���� ���̺� ���� �� ���� ���� (UI ǥ�ÿ�)
        currentEnemyCount--;

        // ����Ʈ���� ����ϴ� �� ���� ���� 
        enemyList.Remove(enemy);
        
        // �� ������Ʈ ����
        Destroy(enemy.gameObject);
        CheckIfStageEnds();
    }

    public void OnDebuffCondition(Vector3 position, DebuffCode code)
    {
        debuffManager.ActivateDebuff(position, code);
    }

    private void CheckIfStageEnds()
    {
        if (currentEnemyCount == 0)
        {
            // �� ���̺갡 ���� �� ������׿� ���� event�� invoke
            onWaveEnds.Invoke();
            // �߰����� : ������ ���̺�, ������ ���Ϳ��ٸ� ������ ����
            if (currentWaveCount == waveMaxCount)
            {
                gameClear.Invoke();
            }
        }
    }

    private void SpawnEnemyHPSlider(GameObject enemy)
    {
        // �� ü���� ��Ÿ���� Slider UI ����
        GameObject sliderClone = Instantiate(enemyHPSliderPrefab);
        // Slider UI ������Ʈ�� parent("Canvas" ������Ʈ)�� �ڽ����� ����
        // Tip. UI�� ĵ������ �ڽĿ�����Ʈ�� �����Ǿ� �־�� ȭ�鿡 ���δ�
        sliderClone.transform.SetParent(canvasTransform);
        // ���� �������� �ٲ� ũ�⸦ �ٽ� (1, 1, 1)�� ����
        sliderClone.transform.localScale = Vector3.one;

        // Slider UI�� �Ѿƴٴ� ����� �������� ����
        sliderClone.GetComponent<SliderPositionAutoSetter>().Setup(enemy.transform);
        // Slider UI�� �ڽ��� ü�� ������ ǥ���ϵ��� ����
        sliderClone.GetComponent<EnemyHPViewer>().Setup(enemy.GetComponent<EnemyHealth>());
    }

}
