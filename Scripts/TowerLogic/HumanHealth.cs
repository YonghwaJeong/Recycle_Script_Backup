using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Events;

public class HumanHealth : MonoBehaviour
{
    public static float maxHP = 100;
    public static float currentHP = maxHP;
    [SerializeField] private UnityEvent onHumanDie;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void GetDamage(float damage)
    { 
        currentHP -= damage;
        if (currentHP <= 0)
        {
            animator.SetTrigger("Dead");
            onHumanDie.Invoke();
        }
    }
}
