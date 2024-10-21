using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPollutionArea : MonoBehaviour
{

    [SerializeField] private float speedMultiplier = 1.5f;
    private GameObject human;
    private List<GameObject> collidingEnemies = new List<GameObject>();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Movement2D>().AccelerateBy(speedMultiplier);
            if (!collidingEnemies.Contains(collision.gameObject))
            {
                collidingEnemies.Add(collision.gameObject);
            }
        }
        if (collision != null && collision.gameObject.CompareTag("Human"))
        {
            human = collision.gameObject;
            human.GetComponent<HumanAttack>().ReduceAttackSpeed();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Movement2D>().DecelerateBy(speedMultiplier);
            if (collidingEnemies.Contains(collision.gameObject))
            {
                collidingEnemies.Remove(collision.gameObject);
            }
        }
        if (collision != null && collision.gameObject.CompareTag("Human"))
        {
            human.GetComponent<HumanAttack>().ResetAttackSpeed();
            human = null;
        }
    }

    private void OnDisable()
    {
        if (collidingEnemies.Count != 0)
        {
            foreach (GameObject obj in collidingEnemies)
            {
                if (obj != null) { obj.GetComponent<Movement2D>().TurnToNormal(); }
            }
        }
        human = null;
    }
}
