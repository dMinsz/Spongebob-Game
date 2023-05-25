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

    [SerializeField] public Transform player;
    [SerializeField] public Vector3 returnPosition;
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
        returnPosition = player.position;
    }

    private void Update()
    {
        states[(int)curState].Update();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

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
            
        }

        public override void Update()
        {
            idleTime = Time.deltaTime;

            if(idleTime > 8)
            {
                idleTime = 0;
                arm.ChangeState(StateArm.Attack);
            }
        }

        

        public override void Exit()
        {
            
        }
    }

    public class AttackState : StateBaseMekaSquidWard
    {
        private Arm arm;

        public AttackState(Arm arm)
        {
            this.arm = arm;
        }

        public override void Enter()
        {
            
        }

        public override void Update()
        {

        }

        public override void Exit()
        {
            
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
            
        }

        public override void Update()
        {

        }

        public override void Exit()
        {
            
        }
    }
}
