using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking.Types;

public partial class FlyDutchManController : MonoBehaviour
{
    private class DieState : BaseState
    {

        public DieState(FlyDutchManController owner, StateMachine<State, FlyDutchManController> stateMachine) : base(owner, stateMachine)
        {
        }

        public override void Setup()
        {

        }

        public override void Enter()
        {
            rigidbody.gravityScale = 1.0f;
            rigidbody.velocity = Vector2.up * 3;
            animator.SetBool("IsDie", true);
            collider.enabled = false;
            

            Destroy(gameObject, 5f);
        }

        public override void Update()
        {

        }

        public override void Transition()
        {

        }

        public override void Exit()
        {

        }
    }
}
