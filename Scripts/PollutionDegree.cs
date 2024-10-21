using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PollutionDegree : MonoBehaviour
{
    [SerializeField]
    private Image imageScreen; // ��üȭ���� ���� ������ �̹���
    [SerializeField]
    private float maxP = 100; // �ִ� ������
    private float currentP; // ���� ������
    [SerializeField]
    private UnityEvent gameOver;

    public float MaxP => maxP;
    public float CurrentP => currentP;

    private void Awake()
    {
        currentP = 0; // ���� �������� 0���� ����
    }

    public void TakeDamage(float damage)
    {
        // ���� �������� damage��ŭ ����
        currentP += damage;

        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        // �������� 100�� �Ǹ� ���ӿ���
        if ( currentP >= maxP)
        {
            gameOver.Invoke();
        }
    }

    private IEnumerator HitAlphaAnimation()
    {
        // ��üȭ�� ũ��� ��ġ�� imageScreen�� ������ color ������ ����
        // imageScreen�� ������ 40%�� ����
        Color color = imageScreen.color;
        color.a = 0.4f;
        imageScreen.color = color;

        // ������ 0%�� �ɶ����� ����
        while (color.a >= 0.0f)
        {
            color.a -= Time.deltaTime;
            imageScreen.color = color;

            yield return null;
        }


    }
}
