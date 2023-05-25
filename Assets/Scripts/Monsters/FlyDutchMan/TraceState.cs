using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class FlyDutchManController : MonoBehaviour
{
    private class TraceState : BaseState
    {
        private Transform target;
        private float speed;
        private float range;
        private float attackRange;
        private float stopRange;
        public TraceState(FlyDutchManController owner, StateMachine<State, FlyDutchManController> stateMachine) : base(owner, stateMachine)
        {
        }

        public override void Setup()
        {
            target = owner.target;
            speed = owner.moveSpeed;
            range = owner.traceRange;
            attackRange = owner.FireRange;
            stopRange = owner.StopRange;
        }

        public override void Enter()
        {
            animator.SetBool("IsTrace", true);
            //rigidbody.velocity = Vector3.zero;
        }

        public override void Update()
        {
            Vector2 dir = (target.position - transform.position).normalized;
            rigidbody.velocity = dir * speed;
            //rigidbody.AddForce(dir * speed, ForceMode2D.Impulse);
            renderer.flipX = rigidbody.velocity.x > 0 ? true : false;
        }

        public override void Transition()
        {
            if ((target.position - transform.position).sqrMagnitude < stopRange * stopRange)
            { 
                stateMachine.ChangeState(State.Idle);
            }

            if ((target.position - transform.position).sqrMagnitude < attackRange * attackRange)
            {
                stateMachine.ChangeState(State.Fire);
            }
        }

        public override void Exit()
        {
            animator.SetBool("IsTrace", false);
        }
    }
}
