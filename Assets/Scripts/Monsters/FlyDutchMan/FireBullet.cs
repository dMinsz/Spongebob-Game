using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FireBullet : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private float lifetime = 5f;
    private Rigidbody2D rigd;
    private bool bossFlip;

    private Vector3 targetPos;

    private void Awake()
    {
        rigd = GetComponent<Rigidbody2D>();

        GameObject target = GameObject.FindWithTag("Boss");

        var renderer = target.GetComponent<SpriteRenderer>();
        bossFlip = renderer.flipX;

        targetPos = target.transform.position;
    }

    private void Update()
    {

        Vector2 dir = (targetPos - transform.position).normalized;

        //this.transform.Translate(dir * Speed * Time.deltaTime);
        rigd.AddForce(dir*Speed,ForceMode2D.Impulse);
        //if (bossFlip)
        //    this.transform.Translate(transform.right * Speed * Time.deltaTime);
        //else
        //    this.transform.Translate(transform.right * -1 * Speed * Time.deltaTime);
    }

    private void Start()
    {
        //rigd.velocity = transform.forward * Speed;
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
