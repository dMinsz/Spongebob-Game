using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireIceShot : MonoBehaviour
{
    [SerializeField] IceShot iceShotPrefab;
    [SerializeField] Transform iceShotPoint;
    [SerializeField] float repeatTime;
    void Start()
    {
        InvokeRepeating("Fire", 0f, repeatTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Fire()
    {
        Instantiate(iceShotPrefab, iceShotPoint.position, iceShotPoint.rotation);
        
    }
}
