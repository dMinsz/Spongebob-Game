using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class BasicPortal : MonoBehaviour
{
    public SceneDefine.Scene NextScene;

    public UnityEvent OnPortal;

    private BoxCollider2D col;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnPortal?.Invoke();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.Z))
            {
                GameManager.Scene.LoadScene(NextScene);
            }

        }
    }

}
