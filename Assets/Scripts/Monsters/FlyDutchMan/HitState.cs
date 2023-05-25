using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public partial class FlyDutchManController : MonoBehaviour
{
    private class HitState : BaseState
    {

        private UnityEvent OnHited;

        public HitState(FlyDutchManController owner, StateMachine<State, FlyDutchManController> stateMachine) : base(owner, stateMachine)
        {
        }

        public override void Setup()
        {
            OnHited = owner.OnHited;
        }

        public override void Enter()
        {
            animator.SetTrigger("Hited");
            OnHited?.Invoke();
        }

        public override void Update()
        {

        }

        public override void Transition()
        {
            stateMachine.ChangeState(State.Idle);
        }

        public override void Exit()
        {

        }
    }
}
