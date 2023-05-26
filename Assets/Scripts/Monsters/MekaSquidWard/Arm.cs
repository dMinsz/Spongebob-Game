using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArmState;
using System.Runtime.InteropServices.WindowsRuntime;

public class Arm : MonoBehaviour
{
    // private Collider2D armcollider;
    // private Rigidbody2D rb;
    private StateBaseMekaSquidWard[] states;
    private StateArm curState;
    public Vector3 returnPosition;
    
    [SerializeField] public GameObject player;
    [SerializeField] public float moveSpeed;
    [SerializeField] public LayerMask groundMask;
    // [SerializeField] public Collider2D staybox;

    private void Awake()
    {
        // armcollider = GetComponent<Collider2D>();
        // rb = GetComponent<Rigidbody2D>();
        states = new StateBaseMekaSquidWard[(int)StateArm.Size];
        states[(int)StateArm.Idle] = new IdleState(this);
        states[(int)StateArm.Attack] = new AttackState(this, player);
        states[(int)StateArm.TakeDown] = new TakeDownState(this);
        states[(int)StateArm.Return] = new ReturnState(this);
    }

    private void Start()
    {
        curState = StateArm.Idle;

        // player = GameObject.FindGameObjectWithTag("Player").transform;
        returnPosition = transform.position;
    }

    private void Update()
    {
        states[(int)curState].Update();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == player.tag)
        {
            curState = StateArm.Return;
        }
    }

    public bool IsGroundExist()
    {
        Debug.DrawRay(transform.position, Vector2.down, Color.red);
        return Physics2D.Raycast(transform.position, Vector2.down, 0.8f, groundMask);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            Rigidbody2D playerRigidbody = playerController.GetComponent<Rigidbody2D>();
            int dirX = playerController.transform.position.x < transform.position.x ? -1 : 1;
            playerRigidbody.velocity = new Vector2(dirX * 3, 8);//밀치기

            playerController.Hit(1);
        }
    }

    public void ChangeState(StateArm state)
    {
        states[(int)curState].Exit();
        curState = state;
        states[(int)curState].Enter();
        states[(int)curState].Update();
    }
}

namespace ArmState
{
    public enum StateArm { Idle, Attack, TakeDown, Return, Size }

    public class IdleState : StateBaseMekaSquidWard
    {
        private Arm arm;
        private float idleTime;
        public int random;

        public IdleState(Arm arm)
        {
            this.arm = arm;
        }

        public override void Enter()
        {
            random = Random.Range(1, 3);
            Debug.Log("대기진입");
            idleTime = 0;
        }

        public override void Update()
        {
            idleTime += Time.deltaTime;

            if(idleTime > 3)
            {
                idleTime = 0;
                // arm.ChangeState(StateArm.Attack);
                arm.ChangeState((StateArm)random);
                
            }
        }

        public override void Exit()
        {
            Debug.Log("대기끝");
        }
    }

    public class AttackState : StateBaseMekaSquidWard
    {
        private Arm arm;

        private Transform attackPoint;
        private GameObject player;
        private float attackedtime;

        public AttackState(Arm arm, GameObject player)
        {
            this.arm = arm;
            this.player = player;
        }

        public AttackState DeepCopyAttackPoint()
        {
            AttackState newCopy = new AttackState(arm,player);
            newCopy.player = this.arm.player;

            return newCopy;
        }

        public override void Enter()
        {
            Debug.Log("공격진입");
            attackPoint = DeepCopyAttackPoint().player.transform;
            attackedtime = 0;
        }

        public override void Update()
        {
            attackedtime += Time.deltaTime;
            Vector2 dir = (attackPoint.position - arm.transform.position).normalized;
            arm.transform.Translate(dir * arm.moveSpeed * Time.deltaTime);

            if (attackedtime > 3 || arm.IsGroundExist())
            {
                arm.ChangeState(StateArm.Return);
            }
        }

        public override void Exit()
        {
            Debug.Log("공격끝");
        }

    }

    public class TakeDownState : StateBaseMekaSquidWard
    {
        private Arm arm;

        public TakeDownState(Arm arm)
        {
            this.arm = arm;
        }

        public override void Enter()
        {
            Debug.Log("내려찍기 진입");
        }

        public override void Update()
        {
            arm.transform.Translate(0, -Time.deltaTime * arm.moveSpeed, 0);

            if (arm.IsGroundExist())
            {
                arm.ChangeState(StateArm.Return);
            }
        }

        public override void Exit()
        {
            Debug.Log("내려찍기 끝");
        }
    }

    public class ReturnState : StateBaseMekaSquidWard
    {
        private Arm arm;

        public ReturnState(Arm arm)
        {
            this.arm = arm;
        } 

        public override void Enter()
        {
            Debug.Log("리턴진입");
        }

        public override void Update()
        {
            Vector2 dir = (arm.returnPosition - arm.transform.position).normalized;
            arm.transform.Translate(dir * arm.moveSpeed / 2 * Time.deltaTime);
        
            if(Vector2.Distance(arm.transform.position, arm.returnPosition) < 0.2)
            {
                arm.ChangeState(StateArm.Idle);
            }
        }

        public override void Exit()
        {
            Debug.Log("리턴끝");
        }
    }
}
