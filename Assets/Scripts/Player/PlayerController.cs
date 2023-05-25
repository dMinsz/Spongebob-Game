using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{

    [Header("Status")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float movePower;
    [SerializeField] private float jumpPower;
    [SerializeField] private int Hp;
    [SerializeField] private float hitStunTime;

    [Header("Player Attack Settings")]
    [SerializeField] private float melleAttackRange;
    [SerializeField] private float rangeAttackRange;
    [SerializeField] private Transform ShootPos;

    [Header("Debug Test Attack and Hit")]
    //[SerializeField] private float rangeAttackRange;
    [SerializeField] private TextMeshPro HPText;
    [Header("Layer Masking")]
    [SerializeField] private LayerMask groundMask;


    [Header("Player Event")]
    public UnityEvent<Vector2> OnMoved;
    public UnityEvent OnJumped;
    public UnityEvent OnMeleeAttacked;
    public UnityEvent OnRangeAttacked;
    public UnityEvent OnDied;


    //Base Components
    private new Rigidbody2D rigidbody;
    private Animator animator;
    private new SpriteRenderer renderer;

    //target
    private GameObject Target;

    private Vector2 inputDir;

    private bool isGround;
    private bool isHited;
    private bool isDied;

    private Coroutine mainRoutine;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();

        Target = GameObject.FindGameObjectWithTag("Boss");
    }

    private void Start()
    {
        //mainRoutine = StartCoroutine(MoveRoutine());
    }

    private void Update()
    {
        CheckDie();
        if (HPText != null)
        {
            HPText.text = Hp.ToString();
        }
    }

    private void FixedUpdate()
    {
        GroundCheck();
    }

    private void OnDrawGizmos()
    {
        //근접공격 범위
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, melleAttackRange);

    }


    private void OnMove(InputValue value)
    {
        inputDir = value.Get<Vector2>();

        mainRoutine = StartCoroutine(MoveRoutine());
      
        OnMoved?.Invoke(inputDir);
    }

  

    private void OnJump(InputValue value)
    {
        if (!value.isPressed)
            return;
        if (!isGround)
            return;
        if (isHited)
            return;

        Jump();
        OnJumped?.Invoke();
    }

    //공격 테스트 안해봄
    private void OnMeleeAttack(InputValue value)
    {
        if (!value.isPressed)
            return;
        if (isHited) // 맞았을때 공격못함
            return;

        OnMeleeAttacked?.Invoke();
        animator.SetTrigger("MeleeAttack");
    }

   

    private void OnRangeAttack(InputValue value)
    {
        if (!value.isPressed)
            return;
        if (isHited)// 맞았을때 공격못함
            return;

        //공격 하는 곳 체크용
        Debug.DrawRay(transform.position, Vector2.right * inputDir.x * rangeAttackRange, Color.red);

        OnRangeAttacked?.Invoke();
        animator.SetTrigger("RangeAttack");

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
            animator.SetBool("IsGround", true);

            // Smooth landing
            if (rigidbody.velocity.y < -3)
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, -3);
            }
        }
        else
        {
            isGround = false;
            animator.SetBool("IsGround", false);
        }
    }

    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            if (inputDir.x < 0 && rigidbody.velocity.x > -maxSpeed) // 왼쪽으로 이동하는데 , 속력이 최고 속력이 아닐때
            {

                rigidbody.AddForce(Vector2.right * inputDir.x * movePower);

                if (rigidbody.velocity.x > -maxSpeed) // 이동시 최대 속력을 넘어가면
                {
                    Vector2 temp = new Vector2() { x = rigidbody.velocity.x, y = rigidbody.velocity.y };
                    temp.x = -maxSpeed;
                    rigidbody.velocity = temp;
                }

            }
            else if (inputDir.x > 0 && rigidbody.velocity.x < maxSpeed)// 오른쪽으로 이동하는데 , 속력이 최고 속력이 아닐때
            {
                rigidbody.AddForce(Vector2.right * inputDir.x * movePower);

                if (rigidbody.velocity.x < maxSpeed) // 이동시 최대 속력을 넘어가면
                {
                    Vector2 temp = new Vector2() { x = rigidbody.velocity.x, y = rigidbody.velocity.y };
                    temp.x = maxSpeed;
                    rigidbody.velocity = temp;
                }

            }
         
            //Debug.Log("MoveSpeed velocity:" + rigidbody.velocity.x);

            animator.SetFloat("MoveDir", Mathf.Abs(inputDir.x));
            if (inputDir.x > 0) 
                renderer.flipX = false;
             
            else if (inputDir.x < 0)
                renderer.flipX = true;

            yield return null;
        }
    }

    private void CheckDie()
    {
        if (isDied)
        {
            Die();
        }
    }


    public void Die()
    {
        if (!isDied)
        {
            return;
        }

        animator.SetBool("IsDied", true);

        OnDied?.Invoke();

        //if die go To Robby
        //GameManager.Scene.LoadScene(SceneDefine.Scene.RobbyScene);
    }

    public void Hit(int Damage)
    {
        Hp -= Damage;

        if (Hp <= 0)
        {
            Hp = 0;
            isDied = true;
            return;
        }

        if (mainRoutine != null)
        {
            StopCoroutine(mainRoutine);
        }
        mainRoutine = StartCoroutine(HitRoutine());
    }

    private IEnumerator HitRoutine()
    {
        
        animator.SetBool("IsHited", true);
        isHited = true;


        yield return new WaitForSeconds(hitStunTime);

        animator.SetBool("IsHited", false);
        isHited = false;
        mainRoutine = StartCoroutine(MoveRoutine());
    }
}
