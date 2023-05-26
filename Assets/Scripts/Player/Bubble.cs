using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bubble : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField]  private float lifetime = 3f;
    private Rigidbody2D rigd;
    private bool playerflip;

    private GameObject Target;

    private void Awake()
    {
        rigd = GetComponent<Rigidbody2D>();
        Target = GameObject.FindGameObjectWithTag("Boss");

        var renderer = GameObject.FindWithTag("Player").GetComponent<SpriteRenderer>();
        playerflip = renderer.flipX;
    }

    private void Update()
    {
        if (!playerflip)
            this.transform.Translate(transform.right * Speed * Time.deltaTime);
        else
            this.transform.Translate(transform.right* -1 * Speed * Time.deltaTime);
    }

    private void Start()
    {
        rigd.gravityScale = 0.0f;
        rigd.velocity = transform.forward * Speed;
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            Target.GetComponent<IMonster>().Hit(1);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Boundary"))
        {
            Destroy(gameObject);
        }
    }
}
