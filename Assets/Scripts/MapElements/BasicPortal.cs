using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class BasicPortal : MonoBehaviour
{

    public UnityEvent OnPortal;

    private BoxCollider2D col;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
    }

    //나중에 바꾸도록하자.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Scene.LoadScene(SceneDefine.Scene.FlyDutchManScene);
        }
    }

}
