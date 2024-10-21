using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardousArea : MonoBehaviour
{
    [SerializeField] private float heal = 1f;
    [SerializeField] private float period = 0.5f;
    [SerializeField] private float poisonDamage = 1f;
    private List<GameObject> collidingEnemies = new List<GameObject>();
    private GameObject human;
    private float counter = 0f;

    private void Update()
    {
        if (counter > period)
        {
            foreach (GameObject enemy in collidingEnemies)
            {
                enemy.GetComponent<EnemyHealth>().GetAdditionalHealth(heal);
            }
            if (human != null)
            {
                human.GetComponent<HumanHealth>().GetDamage(poisonDamage);
            }
            counter = 0;
        } else
        {
            counter += Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Enemy"))
        {
            collidingEnemies.Add(collision.gameObject);
        }
        if (collision != null && collision.gameObject.CompareTag("Human"))
        {
            human = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Enemy"))
        {
            collidingEnemies.Remove(collision.gameObject);
        }
        if (collision != null && collision.gameObject.CompareTag("Human"))
        {
            human = null;
        }
    }

    private void OnDisable()
    {
        if (collidingEnemies.Count != 0)
        {
            collidingEnemies.Clear();
        }
        human = null;
    }
}
