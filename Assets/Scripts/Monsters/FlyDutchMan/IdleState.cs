using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public partial class FlyDutchManController : MonoBehaviour
{
    private class IdleState : BaseState
    {
        private Transform target;
        private float range;
        private UnityEvent OnIdle;

        public IdleState(FlyDutchManController owner, StateMachine<State, FlyDutchManController> stateMachine) : base(owner, stateMachine)
        {
        }

        public override void Setup()
        {
            target = owner.target;
            range = owner.traceRange;
            OnIdle = owner.OnIdled;
        }

        public override void Enter()
        {
            rigidbody.velocity = Vector3.zero;
            
            OnIdle?.Invoke();
        }

        public override void Update()
        {

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
