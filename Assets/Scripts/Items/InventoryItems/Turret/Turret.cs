using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float range = 10f;

    [SerializeField] private Transform partToRotate;

    private float turnSpeed = 5f;

    [SerializeField] private Weapon weapon;

    //[SerializeField] private GameObject turretPrefab;
   //[SerializeField] private GameObject bulletPrefab;
   [SerializeField] private Transform firePoint;
   [SerializeField] private int price;

    private void Start()
    {
       // InvokeRepeating("UpdateTarget", 0f, 0.5f);
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
            target = null;
        }
    }

    void Update()
    {
        UpdateTarget();
        if (target == null)
            return;

        // SHOOT TOURELLE
            
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        if(weapon.CanShoot())
            Shoot(dir, target);
    }

    public void Deploy(Vector3 position, Quaternion rotation)
    {
        GetComponent<MeshRenderer>().enabled = true;
        transform.position = position;
        transform.rotation = rotation;
        //GameObject newTurret = GameObject.Instantiate(turretPrefab, position, rotation);
    }

    public void Shoot(Vector3 direction, Transform _target)
    {
        weapon.Shoot(direction, _target);
    }

}
