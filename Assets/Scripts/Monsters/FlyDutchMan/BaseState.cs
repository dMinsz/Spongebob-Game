using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class FlyDutchManController : MonoBehaviour
{
    private abstract class BaseState : StateBase<State, FlyDutchManController>
    {
        protected GameObject gameObject => owner.gameObject;
        protected Transform transform => owner.transform;
        protected Rigidbody2D rigidbody => owner.rigidbody;
        protected SpriteRenderer renderer => owner.renderer;
        protected Animator animator => owner.animator;
        protected Collider2D collider => owner.collider;

        protected BaseState(FlyDutchManController owner, StateMachine<State, FlyDutchManController> stateMachine) : base(owner, stateMachine)
        {
        }
    }
}
