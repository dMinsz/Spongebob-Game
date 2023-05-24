using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bubble : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField]  private float lifetime = 5f;
    private Rigidbody2D rigd;
    private bool playerflip;

    private void Awake()
    {
        rigd = GetComponent<Rigidbody2D>();

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
        rigd.velocity = transform.forward * Speed;
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
