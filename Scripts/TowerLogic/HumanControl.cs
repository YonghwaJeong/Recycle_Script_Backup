using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanControl : MonoBehaviour
{
    public float moveSpeed = 5f;
    private bool isSelected = false;
    private Vector3 targetPosition;
    [SerializeField] private GameObject indicatorArrow;
    public Animator animator;
    SpriteRenderer spriteRenderer;

    private bool isMoving = false;
    

    void Start()
    {
        targetPosition = transform.position;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator.SetBool("Move", false);
    }

    private void Update()
    {
        TakeSelectionInput();
        StartCoroutine("HandleMovement");
        StopCoroutine("HandleMovement");
        

    }

    private void LateUpdate()
    {
       
    }

    public void GoIdle()
    {
        isMoving = false;
        animator.SetBool("Move", false);
    }


    private void TakeSelectionInput()
    {
        // 좌클릭으로 선택 - 우클릭으로 이동(LOL과 유사)
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                isSelected = true;
                indicatorArrow.SetActive(true);
            }
            else
            {
                isSelected = false;
                indicatorArrow.SetActive(false);
            }
        }

        if (isSelected && Input.GetMouseButtonDown(1))
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private IEnumerator HandleMovement()
    {
        //
        if (isSelected && transform.position != targetPosition)
        {
            animator.SetBool("Move", transform.position != targetPosition);
            if (targetPosition.x > transform.position.x)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            yield break;
        }

        animator.SetBool("Move", false);
    }

    public void ReduceMoveSpeed()
    {
        moveSpeed /= 2;
    }

    public void ResetMoveSpeed()
    {
        moveSpeed *= 2;
    }
}
