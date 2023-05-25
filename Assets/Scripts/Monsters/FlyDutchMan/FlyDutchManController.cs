using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public partial class FlyDutchManController : MonoBehaviour , IMonster
{

    //Base Components
    private new Rigidbody2D rigidbody;
    private Animator animator;
    private new Collider2D collider;
    private new SpriteRenderer renderer;

    //State Machine
    public enum State { Idle, Trace, Hit, Fire, Die, Size }

    StateMachine<State, FlyDutchManController> stateMachine;
    
    //Base Value
    private Transform target;

    [Header("Debug")]
    [SerializeField]
    public TextMeshPro CurState;
    public TextMeshPro HpText;

    [Header("Status")]
    [SerializeField]
    private int HP = 5;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float traceRange;
    [SerializeField]
    private float FireRange;
    [SerializeField]
    private float StopRange;



    [Header("Events")]
    public UnityEvent OnIdled;
    public UnityEvent OnTraced;
    public UnityEvent OnFired;
    public UnityEvent OnHited;
    public UnityEvent OnDied;


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        renderer = GetComponent<SpriteRenderer>();
        

        //State Machine
        stateMachine = new StateMachine<State, FlyDutchManController>(this);
        stateMachine.AddState(State.Idle, new IdleState(this, stateMachine));
        stateMachine.AddState(State.Trace, new TraceState(this, stateMachine)); // move
        //stateMachine.AddState(State.Returning, new ReturningState(this, stateMachine));
        stateMachine.AddState(State.Hit, new HitState(this, stateMachine));
        stateMachine.AddState(State.Fire, new FireState(this, stateMachine));
        stateMachine.AddState(State.Die, new DieState(this, stateMachine));
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody.gravityScale = 0f;
        target = GameObject.FindGameObjectWithTag("Player").transform;

        stateMachine.SetUp(State.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        CheckDie();
        stateMachine.Update();
        CurState.text = stateMachine.GetNowState().ToString();
        HpText.text = HP.ToString();
    }

    //Basic State

    void CheckDie()
    {
        if (HP < 0)
        {
            stateMachine.ChangeState(State.Die);

        }
    }

    public void Hit(int damage) 
    {

        HP -= damage;

        if (HP <= 0)
        {
            HP = 0;
            stateMachine.ChangeState(State.Die);
        }
        else 
        {
            stateMachine.ChangeState(State.Hit);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, traceRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, FireRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, StopRange);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            Rigidbody2D playerRigidbody = playerController.GetComponent<Rigidbody2D>();
            int dirX = playerController.transform.position.x < transform.position.x ? -1 : 1;
            playerRigidbody.velocity = new Vector2(dirX * 3, 8);//¹ÐÄ¡±â
            
            playerController.Hit(1);
        }
    }
}
