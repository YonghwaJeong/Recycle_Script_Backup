using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
    //[SerializeField]
    //private GameObject enemyPrefab; // 적 프리팹
    [SerializeField]
    private GameObject enemyHPSliderPrefab; // 적 체력을 나타내는 Slider UI 프리팹
    [SerializeField]
    private Transform canvasTransform; // UI를 표현하는 Canvas 오브젝트의 Transform
    //[SerializeField]
    //private float spawnTime; // 적 생성 주기
    [SerializeField]
    private Transform[] wayPoints; // 현재 스테이지의 이동 경로
    [SerializeField]
    private PollutionDegree playerP; // 플레이어 오염도 컴포넌트
    [SerializeField]
    private Wave currentWave; // 현재 웨이브 정보
    private int currentEnemyCount; // 현재 웨이브에 남아 있는 적 숫자 
    private List<Enemy> enemyList; // 현재 맵에 존재하는 모든 적의 정보
    
    [SerializeField] private WaveSystem waveSystem;
    [SerializeField] private AugmentSystem augmentSystem;
    [SerializeField] private DebuffManager debuffManager;
    private int waveMaxCount;
    private int currentWaveCount = 0;

    // 특정 조건에 따른 이벤트
    [SerializeField] private UnityEvent gameClear;
    [SerializeField] private UnityEvent onWaveStarts;
    [SerializeField] private UnityEvent onWaveEnds;

    // 적의 생성과 삭제는 EnemySpanwer에서 하기 때문에 Set은 필요 없다.
    public List<Enemy> EnemyList => enemyList;
    public int CurrentEnemyCount => currentEnemyCount;
    public int MaxEnemyCount => currentWave.maxEnemyCount;

    private void Awake()
    {
        // 적 리스트 메모리 할당
        enemyList = new List<Enemy>();
        // 적 생성 코루틴 함수 호출
        //StartCoroutine("SpawnEnemy");

        // 게임 종료 조건을 감지하기 위해 wave 정보 저장
        waveMaxCount = waveSystem.MaxWave;
    }

    public void StartWave(Wave wave)
    {
        // 매개변수로 웨이브 정보 저장
        currentWave = wave;
        currentWaveCount += 1;
        // 현재 웨이브의 최대 적 숫자를 저장
        currentEnemyCount = currentWave.maxEnemyCount;
        // 현재 웨이브 시작
        StartCoroutine("SpawnEnemy");

        // 웨이브 시작시 변경할 사항들에 관한 event를 invoke
        onWaveStarts.Invoke();
    }

    private IEnumerator SpawnEnemy()
    {
        int spawnEnemyCount = 0;

        while(spawnEnemyCount < currentWave.maxEnemyCount)
        {
            // 웨이브에 등장하는 적의 종류가 여러 종류일 때 임의의 적이 등장하도록 설정하고, 적 오브젝트 생성
            int enemyIndex = UnityEngine.Random.Range(0, currentWave.enemyPrefabs.Length);
            GameObject clone = Instantiate(currentWave.enemyPrefabs[enemyIndex]);
            Enemy enemy = clone.GetComponent<Enemy>(); // 방금 생성된 적의 Enemy 컴포넌트

            // this는 나 자신 (자신의 EnemySpawner 정보)
            enemy.Setup(this, wayPoints);
            enemyList.Add(enemy);

            SpawnEnemyHPSlider(clone);

            spawnEnemyCount++;

            // 각 웨이브별 spawnTime 상이, 현재 웨이브(currentWave)의 spawnTime 사용
            yield return new WaitForSeconds(currentWave.spawnTime); // spawnTime 시간 동안 대기
        }
    }


    /*
    private IEnumerator SpawnEnemy()
    {
        while(true)
        {
            GameObject clone = Instantiate(enemyPrefab); // 적 오브젝트 생성
            Enemy enemy = clone.GetComponent<Enemy>(); // 방금 생성된 적의 Enemy 컴포넌트

            enemy.Setup(this, wayPoints); // wayPoint 정보를 매개변수로 Setup() 호출
            enemyList.Add(enemy);

            SpawnEnemyHPSlider(clone);

            yield return new WaitForSeconds(spawnTime); // spawnTime 시간 동안 대기
        }
    }
    */ //이전 무한 Spawn Enemy 함수


    public void DestroyEnemy(EnemyDestroyType type, Enemy enemy)
    {
        if (type == EnemyDestroyType.Arrive)
        {
            // 플레이어의 오염도 +1
            playerP.TakeDamage(1);
        }
        // 적 처치를 통해 재화 획득
        if (type == EnemyDestroyType.Kill)
        {
            CurrencySystem.Instance.GetResource(enemy.GetRewardType(), enemy.GetRewardValue());
            
            // 임시로 조건없이 획득
            augmentSystem.AddAugmentGauge();
        }
        // 적 사망 때마다 현재 웨이브 생존 적 숫자 감소 (UI 표시용)
        currentEnemyCount--;

        // 리스트에서 사망하는 적 정보 삭제 
        enemyList.Remove(enemy);
        
        // 적 오브젝트 삭제
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
            // 한 웨이브가 끝난 후 변경사항에 대한 event를 invoke
            onWaveEnds.Invoke();
            // 추가조건 : 마지막 웨이브, 마지막 몬스터였다면 게임을 종료
            if (currentWaveCount == waveMaxCount)
            {
                gameClear.Invoke();
            }
        }
    }

    private void SpawnEnemyHPSlider(GameObject enemy)
    {
        // 적 체력을 나타내는 Slider UI 생성
        GameObject sliderClone = Instantiate(enemyHPSliderPrefab);
        // Slider UI 오브젝트를 parent("Canvas" 오브젝트)의 자식으로 설정
        // Tip. UI는 캔버스의 자식오브젝트로 설정되어 있어야 화면에 보인다
        sliderClone.transform.SetParent(canvasTransform);
        // 계층 설정으로 바뀐 크기를 다시 (1, 1, 1)로 설정
        sliderClone.transform.localScale = Vector3.one;

        // Slider UI가 쫓아다닐 대상을 본인으로 설정
        sliderClone.GetComponent<SliderPositionAutoSetter>().Setup(enemy.transform);
        // Slider UI에 자신의 체력 정보를 표시하도록 설정
        sliderClone.GetComponent<EnemyHPViewer>().Setup(enemy.GetComponent<EnemyHealth>());
    }

}
