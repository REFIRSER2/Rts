using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject Explosion;
    public float Timer = 3f;
    public float TimerEXP = 0.5f;

    public bool Detach = false;

    private Transform target;
    private float speed = 4F;
    private GameObject owner;

    public void SetTarget(Transform tr)
    {
        target = tr;
    }

    public void SetSpeed(float num)
    {
        speed = num;
    }

    public void SetOwner(GameObject obj)
    {
        owner = obj;
    }
    
    private void Start()
    {
        //Destroy(gameObject, Timer);
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
        }
        else
        {
            var dir = (target.position - transform.position);
            dir.y = target.position.y;
            transform.position = transform.position + dir.normalized * speed;
        }
    }

    void Exp()
    {
        if (Detach)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject, 1f);
                child.parent = null;
            }
        }
        Destroy(gameObject);
        GameObject E = Instantiate(Explosion, transform.position, Explosion.transform.rotation);
        Destroy(E, TimerEXP);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == owner)
        {
            return;
        }
        Exp();
    }
    
 
}
