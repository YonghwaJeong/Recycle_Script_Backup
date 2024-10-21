using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenGlassArea : MonoBehaviour
{
    [SerializeField] private float slowdownValue = -0.3f;
    private GameObject human;
    [SerializeField] private List<GameObject> collidingTowers = new List<GameObject>();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Turret"))
        {
            collision.gameObject.GetComponent<Turret>().ChangeAttackSpeed(slowdownValue);
            if (!collidingTowers.Contains(collision.gameObject))
            {
                collidingTowers.Add(collision.gameObject);
            }
        }
        if (collision != null && collision.gameObject.CompareTag("Human"))
        {
            human = collision.gameObject;
            human.GetComponent<HumanControl>().ReduceMoveSpeed();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Human"))
        {
            human.GetComponent<HumanControl>().ResetMoveSpeed();
            human = null;
        }
    }

    private void OnDisable()
    {
        foreach (GameObject obj in collidingTowers)
        {
            if (obj != null)
            {
                obj.GetComponent<Turret>().ResetToNormalSpeed();
            }
        }
        if (human != null)
        {
            human.GetComponent<HumanControl>().ResetMoveSpeed();
            human = null;
        }
    }
}
