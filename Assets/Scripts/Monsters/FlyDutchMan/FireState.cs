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
        private UnityEvent OnFired;

        public FireState(FlyDutchManController owner, StateMachine<State, FlyDutchManController> stateMachine) : base(owner, stateMachine)
        {
        }

        public override void Setup()
        {
            target = owner.target;
            range = owner.traceRange;
            speed = owner.moveSpeed;
            OnFired = owner.OnFired;
        }

        public override void Enter()
        {
            rigidbody.velocity = Vector3.zero;

            animator.SetTrigger("Fire");

            OnFired?.Invoke();
        }

        //코루틴으로 쏴야함
        public override void Update()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f) // fire 애니메이션이 끝났으면
            {
                //OnFired?.Invoke();
            }
        }
        public override void Transition()
        {
            if ((target.position - transform.position).sqrMagnitude < range * range)
            {
                stateMachine.ChangeState(State.Trace);
            }
        }

        public override void Exit()
        {

        }
    }
}
