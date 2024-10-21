using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextTMPViewer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textPlayerP; // Text - TextMeshPro UI [플레이어의 오염도]
    [SerializeField]
    private TextMeshProUGUI textWave; // Text - TextMeshPro UI [현재 웨이브 / 총 웨이브]
    [SerializeField]
    private TextMeshProUGUI textEnemyCount; // Text - TextMeshPro UI [현재 적 숫자 / 최대 적 숫자]
    
    [SerializeField]
    private PollutionDegree playerP; // 플레이어 오염도 정보
    
    [SerializeField]
    private WaveSystem waveSystem; // 웨이브 정보
    [SerializeField]
    private EnemySpawner enemySpawner; // 적 정보
    [SerializeField]
    private EnemySpawner enemySpawner2; // 적 정보
    [SerializeField]
    private EnemySpawner enemySpawner3; // 적 정보

    private void Update()
    {
        textPlayerP.text = playerP.CurrentP + " %";
        
        textWave.text = waveSystem.CurrentWave + "/" + waveSystem.MaxWave;
        
        if (enemySpawner2 != null && enemySpawner3 != null)
        {
            textEnemyCount.text = (enemySpawner.CurrentEnemyCount +
            enemySpawner2.CurrentEnemyCount +
            enemySpawner3.CurrentEnemyCount) +

            "/" + (enemySpawner.MaxEnemyCount +
            enemySpawner2.MaxEnemyCount +
            enemySpawner3.MaxEnemyCount);

        }
        else
        {
            textEnemyCount.text = enemySpawner.CurrentEnemyCount + "/" + enemySpawner.MaxEnemyCount;
        }
        
    }

}
