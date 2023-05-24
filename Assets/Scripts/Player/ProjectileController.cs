using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController: MonoBehaviour
{
    //Base Components
    //private new Rigidbody2D rigidbody;

    [SerializeField] private Transform ShootPos;
    [Header("Projectile PreFab")]
    [SerializeField] private GameObject projectilePrefab;
    //[Header("Projectile Status")]
    //[SerializeField] private float Speed;


    public void Fire() 
    {
        Instantiate(projectilePrefab, ShootPos.position, ShootPos.rotation);
    }
    


 
}
