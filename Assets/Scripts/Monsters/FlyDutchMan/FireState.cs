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
            range = owner.FireRange;
            speed = owner.moveSpeed;
            OnFired = owner.OnFired;
        }

        public override void Enter()
        {
            rigidbody.velocity = Vector3.zero;

            animator.SetTrigger("Fire");

            OnFired?.Invoke();
        }

        //�ڷ�ƾ���� ������
        public override void Update()
        {
            //�����̸�� ���
            Vector2 dir = (target.position - transform.position).normalized;
            rigidbody.velocity = dir * speed;
            renderer.flipX = rigidbody.velocity.x > 0 ? true : false;

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f) // fire �ִϸ��̼��� ��������
            {
                //OnFired?.Invoke();
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