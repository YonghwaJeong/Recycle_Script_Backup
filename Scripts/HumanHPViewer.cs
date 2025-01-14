using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HumanHPViewer : MonoBehaviour
{

    [SerializeField]
    private Vector3 distance = Vector3.down * 20.0f;
    [SerializeField]
    private Transform targetTransform;
    [SerializeField]
    private RectTransform rectTransform;

    

    [SerializeField]
    private GameObject human;
    [SerializeField]
    private Slider hpSlider;

    public void Awake()
    {
        //RectTransform 컴포넌트 정보 얻어오기
        rectTransform = GetComponent<RectTransform>();

        hpSlider = GetComponent<Slider>();
        hpSlider.value = HumanHealth.currentHP / HumanHealth.maxHP;
    }

    private void Update()
    {
        hpSlider.value = HumanHealth.currentHP / HumanHealth.maxHP;
    }

    private void LateUpdate()
    {
        // 오브젝트의 위치가 갱신된 이후에 Slider UI도 함께 위치를 설정하도록 하기 위해
        // LateUpdate()에서 호출한다

        // 오브젝트의 월드 좌표를 기준으로 화면에서의 좌표 값을 구함

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);
        // 화면내에서 좌표 + distance만큼 떨어진 위치를 Slider UI의 위치로 설정
        rectTransform.position = screenPosition + distance;
    }

}
