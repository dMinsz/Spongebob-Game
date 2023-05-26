using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightIceShot : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private float lifetime = 3f;
    private Rigidbody2D rb;
    private GameObject player;
    private Vector3 targetPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        targetPos = player.transform.position;
    }



    private void Start()
    {
        rb.velocity = Vector2.right * Speed;
        rb.gravityScale = 0.0f;
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.GetComponent<PlayerController>().Hit(1);
            Destroy(gameObject);
        }

    }
}
