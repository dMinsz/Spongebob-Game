using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadMeakSquidWrad : MonoBehaviour
{
    [SerializeField] GameObject bubble;
    [SerializeField] float bossHp;


    private Collider2D collider;



    private void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (false)// �ǰݴ��ϸ�
            bossHp -= 20;
    }


}
