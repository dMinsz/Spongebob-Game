using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArmState;

public class Arm : MonoBehaviour
{
    private Collider2D collider;
    private Rigidbody2D rb;
    private StateBaseMekaSquidWard[] states;
    private StateArm curState;
    public Vector3 returnPosition;

    [SerializeField] public Transform player;
    [SerializeField] public float moveSpeed;
    // [SerializeField] public Collider2D staybox;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        states = new StateBaseMekaSquidWard[(int)StateArm.Size];
        states[(int)StateArm.Idle] = new IdleState(this);
        states[(int)StateArm.Attack] = new AttackState(this);
        states[(int)StateArm.Return] = new ReturnState(this);
    }

    private void Start()
    {
        curState = StateArm.Idle;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        returnPosition = transform.position;
    }

    private void Update()
    {
        states[(int)curState].Update();
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        curState = StateArm.Return;
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
    public enum StateArm { Idle, Attack, Return, Size }

    public class IdleState : StateBaseMekaSquidWard
    {
        private Arm arm;
        private float idleTime;

        public IdleState(Arm arm)
        {
            this.arm = arm;
        }

        public override void Enter()
        {
            Debug.Log("대기진입");
            idleTime = 0;
        }

        public override void Update()
        {
            idleTime += Time.deltaTime;

            if(idleTime > 3)
            {
                idleTime = 0;
                arm.ChangeState(StateArm.Attack);
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

        private Transform attacktransform;
        private float attackedtime;

        public AttackState(Arm arm)
        {
            this.arm = arm;

        }

        public override void Enter()
        {
            Debug.Log("공격진입");
            attacktransform = arm.player.transform;
            attackedtime = 0;
        }

        public override void Update()
        {
            attackedtime += Time.deltaTime;
            Vector2 dir = (attacktransform.position - arm.transform.position).normalized;
            arm.transform.Translate(dir * arm.moveSpeed * Time.deltaTime);

            if (attackedtime > 5)
            {
                arm.ChangeState(StateArm.Return);
            }
        }

        public override void Exit()
        {
            Debug.Log("공격끝");
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
            arm.transform.Translate(dir * arm.moveSpeed * Time.deltaTime);
        
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
