using HeadState;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Head : MonoBehaviour , IMonster
{
    private Rigidbody2D rigidbody;
    public Animator animator;
    private Collider2D collider;
    public SpriteRenderer renderer;
    private StateBaseMekaSquidWard[] states;
    private StateHead curState;
    private Coroutine curRoutine;
    public UnityEvent OnHited;

    [SerializeField] public int hp;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        renderer = GetComponent<SpriteRenderer>();

        states = new StateBaseMekaSquidWard[(int)StateHead.Size];
        states[(int)StateHead.Idle] = new IdleState(this);
        states[(int)StateHead.Hit] = new HitState(this);
        states[(int)StateHead.Die] = new DieState(this);
    }

    private void Start()
    {
        curState = StateHead.Idle;
    }

    private void Update()
    {
        states[(int)curState].Update();
    }

    public void StartCoroutine(string coroutine, Coroutine curRoutine)
    {
        curRoutine = StartCoroutine(coroutine);
    }

    public void StopCoroutin(IEnumerator coroutine)
    {
        StopCoroutine(coroutine);
    }

    public void ChangeState(StateHead state)
    {
        states[(int)curState].Exit();
        curState = state;
        states[(int)curState].Enter();
        states[(int)curState].Update();
    }

    public void Hit(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            hp = 0;
            ChangeState(StateHead.Die);
        }
        else
        {
            ChangeState(StateHead.Hit);
        }
    }
}

namespace HeadState
{
    public enum StateHead { Idle, Hit, Die, Size }

    public class IdleState : StateBaseMekaSquidWard
    {
        private Head head;

        public IdleState(Head head)
        {
            this.head = head;
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

    public class HitState : StateBaseMekaSquidWard
    {
        private Head head;
        private float hitAnimationTime;

        public HitState(Head head)
        {
            this.head = head;
        }

        public void Hit(int damage)
        {
            
        }

        IEnumerator HitRoutine()
        {
            while (head.animator.GetBool("Hited"))
            {
                yield return new WaitForSecondsRealtime(1f);
            }
        }

        public override void Enter()
        {
            head.animator.SetBool("Hited", true);
            hitAnimationTime = 0;
            head.StartCoroutine(HitRoutine());
        }

        public override void Update()
        {
            hitAnimationTime += Time.deltaTime;
            if (hitAnimationTime < 0.25)
                head.renderer.color = new Color(255, 0, 0);
            else if ((hitAnimationTime < 0.5))
                head.renderer.color = new Color(255, 255, 255);
            else if (hitAnimationTime < 0.75)
                head.renderer.color = new Color(255, 0, 0);
            else if (hitAnimationTime < 1)
            {
                head.renderer.color = new Color(255, 255, 255);
                head.ChangeState(StateHead.Idle);
            }
        }

        public override void Exit()
        {
            head.StopCoroutin(HitRoutine());
        }
    }

    public class DieState : StateBaseMekaSquidWard
    {
        private Head head;

        public DieState(Head head)
        {
            this.head = head;
        }

        public override void Enter()
        {
            head.animator.SetTrigger("Died");
        }

        public override void Update()
        {

        }
        public override void Exit()
        {
            
        }

    }
}
