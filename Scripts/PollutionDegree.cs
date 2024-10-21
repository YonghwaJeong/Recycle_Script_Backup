using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PollutionDegree : MonoBehaviour
{
    [SerializeField]
    private Image imageScreen; // 전체화면을 덮는 빨간색 이미지
    [SerializeField]
    private float maxP = 100; // 최대 오염도
    private float currentP; // 현재 오염도
    [SerializeField]
    private UnityEvent gameOver;

    public float MaxP => maxP;
    public float CurrentP => currentP;

    private void Awake()
    {
        currentP = 0; // 현재 오염도를 0으로 설정
    }

    public void TakeDamage(float damage)
    {
        // 현재 오염도를 damage만큼 증가
        currentP += damage;

        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        // 오염도가 100이 되면 게임오버
        if ( currentP >= maxP)
        {
            gameOver.Invoke();
        }
    }

    private IEnumerator HitAlphaAnimation()
    {
        // 전체화면 크기로 배치된 imageScreen의 색상을 color 변수에 저장
        // imageScreen의 투명도를 40%로 설정
        Color color = imageScreen.color;
        color.a = 0.4f;
        imageScreen.color = color;

        // 투명도가 0%가 될때까지 감소
        while (color.a >= 0.0f)
        {
            color.a -= Time.deltaTime;
            imageScreen.color = color;

            yield return null;
        }


    }
}
