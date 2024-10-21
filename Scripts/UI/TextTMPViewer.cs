using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextTMPViewer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textPlayerP; // Text - TextMeshPro UI [�÷��̾��� ������]
    [SerializeField]
    private TextMeshProUGUI textWave; // Text - TextMeshPro UI [���� ���̺� / �� ���̺�]
    [SerializeField]
    private TextMeshProUGUI textEnemyCount; // Text - TextMeshPro UI [���� �� ���� / �ִ� �� ����]
    
    [SerializeField]
    private PollutionDegree playerP; // �÷��̾� ������ ����
    
    [SerializeField]
    private WaveSystem waveSystem; // ���̺� ����
    [SerializeField]
    private EnemySpawner enemySpawner; // �� ����
    [SerializeField]
    private EnemySpawner enemySpawner2; // �� ����
    [SerializeField]
    private EnemySpawner enemySpawner3; // �� ����

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
