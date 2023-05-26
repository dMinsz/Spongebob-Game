using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking.Types;

public partial class FlyDutchManController : MonoBehaviour
{
    private class DieState : BaseState
    {

        private UnityEvent OnDied;
        private float timer= 0.0f;

        public DieState(FlyDutchManController owner, StateMachine<State, FlyDutchManController> stateMachine) : base(owner, stateMachine)
        {
        }

        public override void Setup()
        {
            OnDied = owner.OnDied;
        }

        public override void Enter()
        {
            rigidbody.gravityScale = 1.0f;
            rigidbody.velocity = Vector2.up * 3;
            animator.SetBool("IsDie", true);
            collider.enabled = false;

            OnDied?.Invoke();

            Destroy(gameObject, 5f);
        }

        public override void Update()
        {
            timer += Time.deltaTime;

            if (timer > 2.0f) 
            {
                GameManager.Scene.LoadScene(SceneDefine.Scene.RobbyScene);
            }
        }

        public override void Transition()
        {
           
        }

        public override void Exit()
        {
            
        }
    }
}
