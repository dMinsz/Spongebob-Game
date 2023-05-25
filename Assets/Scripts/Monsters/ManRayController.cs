using manRayState;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;


public class ManRayController : MonoBehaviour
{

    [SerializeField] public Transform groundCheckPoint;
    [SerializeField] public LayerMask groundMask;
    private SpriteRenderer spriteRenderer;

    public Rigidbody2D rb;
    private TMP_Text text;
    public float detectRange;
    public float moveSpeed;
    public float AttackRange;
    public float lastAttackTime;
    public Transform[] patrolPoints;
    public Animator animator;

    private StateBase[] states;
    private State curState;


    public Transform player;
    public Vector3 returnPosition;
    public int patrolIndex = 0;

    private void Awake()
    {
        states = new StateBase[(int)State.Size];
        states[(int)State.Idle] = new IdleState(this);
        states[(int)State.Trace] = new TraceState(this);
        states[(int)State.Return] = new ReturnState(this);
        states[(int)State.Attack] = new AttackState(this);
        states[(int)State.Patrol] = new PatrolState(this);
        spriteRenderer = GetComponent<SpriteRenderer>();

    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        curState = State.Idle;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        returnPosition = transform.position;
    }



    private void Update()
    {
        states[(int)curState].Update();
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            //transform.Rotate(Vector3.up, 180);
        }
    }

    

    public void ChangeState(State state)
    {
        curState = state;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }

    




}

namespace manRayState
{
    public enum State { Idle, Trace, Return, Attack, Patrol, Size }
    public class IdleState : StateBase
    {
        private ManRayController manRay;
        private float idleTime;

        public IdleState(ManRayController manRay)
        {
            this.manRay = manRay;
        }

        

        public override void Update()
        {
            // Nothing Action
            idleTime += Time.deltaTime;

            if (idleTime > 2)
            {
                idleTime = 0;
                manRay.patrolIndex = (manRay.patrolIndex + 1) % manRay.patrolPoints.Length;
                manRay.ChangeState(State.Patrol);
            }


            // detectRange 안에 들어올 경우 State.Trace 상태로 변경
            // 플레이어가 가까워졌을때
            if (Vector2.Distance(manRay.player.position, manRay.transform.position) < manRay.detectRange)
            {
                manRay.ChangeState(State.Trace);
            }
        }
    }

    public class TraceState : StateBase
    {
        private ManRayController manRay;
        public TraceState(ManRayController manRay)
        {
            this.manRay = manRay;
        }

        

        public override void Update()
        {
            // Trace player
            Vector2 dir = (manRay.player.position - manRay.transform.position).normalized;
            manRay.transform.Translate(dir * manRay.moveSpeed * Time.deltaTime);
            manRay.animator.SetBool("Move", true);


            if (Vector2.Distance(manRay.player.position, manRay.transform.position) > manRay.detectRange)
            {
                manRay.ChangeState(State.Return);
            }
            // 공격 범위 안에 있으면 트레이스
            else if (Vector2.Distance(manRay.player.position, manRay.transform.position) < manRay.AttackRange)
            {
                manRay.ChangeState(State.Trace);
            }
        }
    }
    public class ReturnState : StateBase
    {
        private ManRayController manRay;
        public ReturnState(ManRayController manRay)
        {
            this.manRay = manRay;
        }

        

        public override void Update()
        {
            // 원래 자리로 돌아가기
            Vector2 dir = (manRay.returnPosition - manRay.transform.position).normalized;
            manRay.transform.Translate(dir * manRay.moveSpeed * Time.deltaTime);
            manRay.animator.SetBool("Move", true);

            // 원래 자리에 도착했으면
            if (Vector2.Distance(manRay.transform.position, manRay.returnPosition) < 0.02f)
            {
                manRay.ChangeState(State.Idle);
            }
            else if (Vector2.Distance(manRay.player.position, manRay.transform.position) < manRay.detectRange)
            {
                manRay.ChangeState(State.Trace);
            }
        }
    }

    public class AttackState : StateBase
    {
        private ManRayController manRay;
        public AttackState(ManRayController manRay)
        {
            this.manRay = manRay;
        }

        

        public override void Update()
        {
            manRay.animator.SetBool("Attack", true);
            manRay.lastAttackTime += Time.deltaTime;
            if (manRay.lastAttackTime > 3)
            {
                Debug.Log("공격");
                manRay.lastAttackTime = 0;
            }

            else if (Vector2.Distance(manRay.player.position, manRay.transform.position) > manRay.AttackRange)
            {
                manRay.ChangeState(State.Trace);
            }
        }
    }

    public class PatrolState : StateBase
    {
        private ManRayController manRay;
        
        public PatrolState(ManRayController manRay)
        {
            this.manRay = manRay;
        }

        

        public override void Update()
        {
            Vector2 targetPosition = manRay.patrolPoints[manRay.patrolIndex].position;
            Vector2 currentPosition = manRay.transform.position;
            Vector2 direction = (targetPosition - currentPosition).normalized;

            manRay.transform.Translate(direction * manRay.moveSpeed * Time.deltaTime);
            manRay.animator.SetBool("Move", true);

            Vector2 nextPosition = currentPosition + direction * manRay.moveSpeed * Time.deltaTime;
            // wallLayer 설정하기 ManRay에서
            RaycastHit2D hit = Physics2D.Linecast(currentPosition, nextPosition, manRay.wallLayer);


            float distanceToTarget = Vector2.Distance(currentPosition, targetPosition);
            if (distanceToTarget < 0.02f)
            {               
                manRay.patrolIndex = (manRay.patrolIndex + 1) % manRay.patrolPoints.Length;
            }
            else if (Vector2.Distance(manRay.player.position, currentPosition) < manRay.detectRange)
            {                
                manRay.ChangeState(State.Trace);
            }

        }
    }
}
