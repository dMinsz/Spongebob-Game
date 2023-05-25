using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public partial class FlyDutchManController : MonoBehaviour
{
    private class FireState : BaseState
    {
        private Transform target;
        private float range;
        private float speed;
        private float delay = 1.0f;
        private UnityEvent OnFired;
        
        private float timer = 0.0f;
        

        private Coroutine mainRoutine;
        public FireState(FlyDutchManController owner, StateMachine<State, FlyDutchManController> stateMachine) : base(owner, stateMachine)
        {
        }

        public override void Setup()
        {
            target = owner.target;
            range = owner.FireRange;
            speed = owner.moveSpeed;
            delay = owner.fireDelay;
            OnFired = owner.OnFired;
        }

        public override void Enter()
        {
            rigidbody.velocity = Vector3.zero;

            animator.SetTrigger("Fire");

            OnFired?.Invoke();


            timer += Time.deltaTime;
        }

        //코루틴으로 쏴야함
        public override void Update()
        {
            //움직이면써 쏘기
            Vector2 dir = (target.position - transform.position).normalized;
            rigidbody.velocity = dir * speed;
            renderer.flipX = rigidbody.velocity.x > 0 ? true : false;

            timer += Time.deltaTime;

            if (delay < timer)
            {
                OnFired?.Invoke();
                animator.SetTrigger("Fire");

                timer = 0;
            }
        }
        public override void Transition()
        {
            if ((target.position - transform.position).sqrMagnitude > range * range)
            {
                stateMachine.ChangeState(State.Trace);
            }
        }

     
        public override void Exit()
        {
        }
       
    }
}
