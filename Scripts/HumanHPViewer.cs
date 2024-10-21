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
        //RectTransform ������Ʈ ���� ������
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
        // ������Ʈ�� ��ġ�� ���ŵ� ���Ŀ� Slider UI�� �Բ� ��ġ�� �����ϵ��� �ϱ� ����
        // LateUpdate()���� ȣ���Ѵ�

        // ������Ʈ�� ���� ��ǥ�� �������� ȭ�鿡���� ��ǥ ���� ����

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);
        // ȭ�鳻���� ��ǥ + distance��ŭ ������ ��ġ�� Slider UI�� ��ġ�� ����
        rectTransform.position = screenPosition + distance;
    }

}
