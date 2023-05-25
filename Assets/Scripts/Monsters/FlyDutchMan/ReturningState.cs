using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class FlyDutchManController : MonoBehaviour
{
    private class ReturningState : BaseState
    {
        private Vector3 returnPosition;
        private float speed;

        public ReturningState(FlyDutchManController owner, StateMachine<State, FlyDutchManController> stateMachine) : base(owner, stateMachine)
        {
        }

        public override void Setup()
        {
            returnPosition = transform.position;
            speed = owner.moveSpeed;
        }

        public override void Enter()
        {

        }

        public override void Update()
        {
            Vector2 dir = (returnPosition - transform.position).normalized;
            rigidbody.velocity = dir * speed;
            renderer.flipX = rigidbody.velocity.x > 0 ? true : false;
        }

        public override void Transition()
        {
            if ((returnPosition - transform.position).sqrMagnitude < 0.01f)
            {
                stateMachine.ChangeState(State.Idle);
            }
        }

        public override void Exit()
        {

        }
    }
}
