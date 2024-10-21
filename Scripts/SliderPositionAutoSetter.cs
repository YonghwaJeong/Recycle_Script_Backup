using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderPositionAutoSetter : MonoBehaviour
{
    [SerializeField]
    private Vector3         distance = Vector3.down * 20.0f;
    private Transform       targetTransform;
    private RectTransform   rectTransform;
    private EnemyHealth enemyHealth;
    private GameObject gObject;

    public void Setup(Transform target)
    {
        //ui�� �Ѿư� target ����
        targetTransform = target;
        //RectTransform ������Ʈ ���� ������
        rectTransform = GetComponent<RectTransform>();
        enemyHealth = target.GetComponentInParent<EnemyHealth>();
        
    }

    private void LateUpdate()
    {
        if (enemyHealth.CurrentHP <= 0)
        {
            Destroy(gameObject);
            return;
        }

        if (targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }

        if (targetTransform.gameObject.activeSelf == false)
        {
            Destroy(gameObject);
            return;
        }


        // ������Ʈ�� ��ġ�� ���ŵ� ���Ŀ� Slider UI�� �Բ� ��ġ�� �����ϵ��� �ϱ� ����
        // LateUpdate()���� ȣ���Ѵ�

        // ������Ʈ�� ���� ��ǥ�� �������� ȭ�鿡���� ��ǥ ���� ����

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);
        // ȭ�鳻���� ��ǥ + distance��ŭ ������ ��ġ�� Slider UI�� ��ġ�� ����
        rectTransform.position = screenPosition + distance;
    }

    

}
