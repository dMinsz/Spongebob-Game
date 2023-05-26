using manRayState;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;


public class ManRayController : MonoBehaviour , IMonster
{

    [SerializeField] public Transform groundCheckPoint;
    [SerializeField] public LayerMask groundMask;
    public SpriteRenderer spriteRenderer;
    private new Collider2D collider;


    public Rigidbody2D rb;
    private TMP_Text text;
    public int Hp = 10;
    public float detectRange;
    public float moveSpeed;
    public float AttackRange;
    public float lastAttackTime;
    public float jumpPower;
    public float jumpInterval;
    public Transform[] patrolPoints;
    public Animator animator;

    private StateBase[] states;
    private State curState;


    public Transform player;
    public Vector3 returnPosition;
    public int patrolIndex = 0;

    public UnityEvent OnIdled;
    public UnityEvent OnTraced;
    public UnityEvent OnHited;
    public UnityEvent OnDied;

    private void Awake()
    {
        collider =GetComponent<Collider2D>();

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
        InvokeRepeating("Jump", jumpInterval, jumpInterval);
    }



    private void Update()
    {
        states[(int)curState].Update();
        renderdir();
    }

    
    public void ChangeState(State state)
    {
        curState = state;
    }
    private void renderdir()
    {
        if(player.transform.position.x > transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        if (player.transform.position.x < transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
    }

    public void Jump()
    {
        rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }
    public void Hit(int damage)
    {
        Hp -= damage;
        if (Hp <= 0)
        {
            Hp = 0;

            Destroy(gameObject, 6f);
            
            animator.SetBool("Died", true);
            

            //rb.velocity = Vector2.zero;
            //rb.constraints = RigidbodyConstraints2D.FreezeAll;

            detectRange = 0.0f;
            ChangeState(State.Idle);

            collider.enabled = false;
            OnDied?.Invoke();
            

        }
        else
        {
            //ChangeState(State.Idle);
            animator.SetTrigger("Hited");
            OnHited?.Invoke();
        }
       
    }
    

    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            ChangeState(State.Return);
        }

        else if (collision.gameObject.tag == "Player")
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            Rigidbody2D playerRigidbody = playerController.GetComponent<Rigidbody2D>();
            int dirX = playerController.transform.position.x < transform.position.x ? -1 : 1;
            playerRigidbody.velocity = new Vector2(dirX * 3, 8);//��ġ��

            playerController.Hit(1);
        }
    
    }


}

namespace manRayState
{
    public enum State { Idle, Trace, Return, Attack, Patrol, Size, Died }
    public class IdleState : StateBase
    {
        private ManRayController manRay;
        private float idleTime;

        public IdleState(ManRayController manRay)
        {
            this.manRay = manRay;
        }

        public override void Enter()
        {
            manRay.OnIdled?.Invoke();
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


            // detectRange �ȿ� ���� ��� State.Trace ���·� ����
            // �÷��̾ �����������
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

        public override void Enter()
        {
            manRay.OnTraced?.Invoke();
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
            // ���� ���� �ȿ� ������ Ʈ���̽�
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

        public override void Enter()
        {
            throw new System.NotImplementedException();
        }

        public override void Update()
        {
            // ���� �ڸ��� ���ư���
            Vector2 dir = (manRay.returnPosition - manRay.transform.position).normalized;
            manRay.transform.Translate(dir * manRay.moveSpeed * Time.deltaTime);
            manRay.animator.SetBool("Move", true);

            // ���� �ڸ��� ����������
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
        private UnityEvent OnFired;
        private ManRayController manRay;
        public AttackState(ManRayController manRay)
        {
            this.manRay = manRay;
        }

        public override void Enter()
        {
            throw new System.NotImplementedException();
        }

        public override void Update()
        {
            if (Vector2.Distance(manRay.player.position, manRay.transform.position) <= manRay.AttackRange)
            {
                manRay.rb.velocity = Vector2.zero; // �������� ����

                // ���� �ִϸ��̼� ���
                manRay.animator.SetTrigger("Attack");
                ExecuteAttack();
            }
            else
            {
                manRay.ChangeState(State.Trace);
            }
        }

        private void ExecuteAttack()
        {
            
            Debug.Log("����");
            
        }
    }

    public class PatrolState : StateBase
    {
        private ManRayController manRay;
        
        public PatrolState(ManRayController manRay)
        {
            this.manRay = manRay;
        }

        public override void Enter()
        {
            throw new System.NotImplementedException();
        }

        public override void Update()
        {
            Vector2 targetPosition = manRay.patrolPoints[manRay.patrolIndex].position;
            Vector2 currentPosition = manRay.transform.position;
            Vector2 direction = (targetPosition - currentPosition).normalized;

            if (direction.x > 0)
            {
                manRay.spriteRenderer.flipX = false;
            }
            else if (direction.x < 0)
            {
                manRay.spriteRenderer.flipX = true;
            }
            direction = -direction;

            manRay.transform.Translate(direction * manRay.moveSpeed * Time.deltaTime);
            manRay.animator.SetBool("Move", true);

            Vector2 nextPosition = currentPosition + direction * manRay.moveSpeed * Time.deltaTime;
            // wallLayer �����ϱ� ManRay����
            RaycastHit2D hit = Physics2D.Linecast(currentPosition, nextPosition);


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
