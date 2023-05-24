using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float movePower;
    [SerializeField]
    private float jumpPower;

    [SerializeField]
    private LayerMask groundMask;

    private new Rigidbody2D rigidbody;
    //private Animator animator;
    private new SpriteRenderer renderer;
    private Vector2 inputDir;
    private bool isGround;
    //private bool isHited;

    private Coroutine moveRoutine;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        moveRoutine = StartCoroutine(MoveRoutine());
    }

    private void FixedUpdate()
    {
        GroundCheck();
    }

    private void OnMove(InputValue value)
    {
        inputDir = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        if (!value.isPressed)
            return;
        if (!isGround)
            return;
        //if (isHited)
        //    return;

        Jump();
    }

    private void Jump()
    {
        rigidbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }

    private void GroundCheck()
    {
        Debug.DrawRay(transform.position, Vector2.down * 1.5f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, groundMask);
        if (hit.collider != null)
        {
            isGround = true;
            //animator.SetBool("IsGround", true);

            // Smooth landing
            if (rigidbody.velocity.y < -3)
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, -3);
            }
        }
        else
        {
            isGround = false;
            //animator.SetBool("IsGround", false);
        }
    }



    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            if (inputDir.x < 0 && rigidbody.velocity.x > -maxSpeed) // 왼쪽으로 이동하는데 , 속력이 최고 속력이 아닐때
                rigidbody.AddForce(Vector2.right * inputDir.x * movePower);
            else if (inputDir.x > 0 && rigidbody.velocity.x < maxSpeed)// 오른쪽으로 이동하는데 , 속력이 최고 속력이 아닐때
                rigidbody.AddForce(Vector2.right * inputDir.x * movePower);

            //animator.SetFloat("MoveSpeed", Mathf.Abs(inputDir.x));
            if (inputDir.x > 0)
                renderer.flipX = false;
            else if (inputDir.x < 0)
                renderer.flipX = true;

            yield return null;
        }
    }

    public void Hit()
    {
        StartCoroutine(HitRoutine());
    }

    private IEnumerator HitRoutine()
    {
        StopCoroutine(moveRoutine);
        //animator.SetBool("IsHit", true);
        //isHited = true;
        yield return new WaitForSeconds(1f);
        //animator.SetBool("IsHit", false);
        //isHited = false;
        moveRoutine = StartCoroutine(MoveRoutine());
    }
}
