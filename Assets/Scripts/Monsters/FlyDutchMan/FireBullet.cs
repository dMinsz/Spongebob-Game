using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FireBullet : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private float lifetime = 3f;
    private Rigidbody2D rigd;
    private GameObject player;
    private Vector3 targetPos;

    private void Awake()
    {
        rigd = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        targetPos = player.transform.position;
    }

    private void Update()
    {
        Vector2 dir = (targetPos - transform.position).normalized;
        this.transform.Translate(dir * Speed * Time.deltaTime);
    }

    private void Start()
    {
        rigd.gravityScale = 0.0f;
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
