﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private BulletsPool bulletsPool;
    private Rigidbody rb;
    
    private float damage;
    public float Damage
    {
        get => damage;
        set => damage = value;
    }
    private float maxRange;
    public float MaxRange
    {
        get => maxRange;
        set => maxRange = value;
    }

    private float range;

    [SerializeField] private eBulletType bulletType;
    public eBulletType BulletType
    {
        get => bulletType;
        set => bulletType = value;
    }

    private Coroutine cor;
    
    [SerializeField] private float speed;
    [SerializeField] private bool isSeekingBullet;
    [SerializeField] private Transform target;

    public Transform Target
    {
        get => target;
        set => target = value;
    }
    public BulletsPool P_BulletsPool 
    {
        get => bulletsPool;
        set => bulletsPool = value;
    }
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void GetTarget(Transform _target)
    {
        target = _target;
    }
    
    void Update()
    {
       /* if (isSeekingBullet)
        {
            
            /*if (target != null)
            {
                lastPosition = target.position;
            }

            Vector3 dir = target.position - transform.position;
            float distanceThisFrame = speed * Time.deltaTime;

            if (dir.magnitude <= distanceThisFrame)
            {
                return;
            }

            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
            //Shoot(dir.normalized);
        }*/
       
    }
    void UpdateTarget()
    {
        GameObject[] ennemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortsDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in ennemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortsDistance)
            {
                shortsDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }

        }

        if (nearestEnemy != null && shortsDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            DestroyBullet();
        }
    }

    private void DestroyBullet()
    {
            bulletsPool.ReleaseBulletInstance(gameObject, bulletType);
    }
    public void Shoot(Vector3 direction)
    {
        if (cor != null)
        {
            StopAllCoroutines();
            cor = null;
        }

        if (cor == null)
            cor = StartCoroutine(DestroyOnMaxRange());
        
        rb.velocity = direction.normalized * speed * Time.fixedDeltaTime;
    }

    private IEnumerator DestroyOnMaxRange()
    {
        range = maxRange;
        if (isSeekingBullet)
        {
            while (range > 0f && target != null)
            {
                range -= speed * Time.deltaTime;
                    Vector3 dir = target.position - transform.position;
                    float distanceThisFrame = speed * Time.deltaTime;

                    transform.Translate(dir.normalized * distanceThisFrame, Space.World);

                yield return null;
            }
        }
        else
        {
            while (range > 0f)
            {
                range -= speed * Time.deltaTime;

                yield return null;
            }
        }
        

        cor = null;
        DestroyBullet();
    }
    

    private void OnTriggerEnter(Collider col)
    {
        // S'il ne faut pas détruire en fonction de ce que la balle touche
        if (col.tag == "Bullet") return;
        DestroyBullet();
        
    }
    
}

