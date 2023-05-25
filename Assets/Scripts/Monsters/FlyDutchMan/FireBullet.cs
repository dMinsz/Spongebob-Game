using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FireBullet : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private float lifetime = 3f;
    //private Rigidbody2D rigd;

    private Vector3 targetPos;

    private void Awake()
    {
        //rigd = GetComponent<Rigidbody2D>();
        targetPos = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    private void Update()
    {

        Vector2 dir = (targetPos - transform.position).normalized;
        this.transform.Translate(dir * Speed * Time.deltaTime);
       
    }

    private void Start()
    {
       
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
