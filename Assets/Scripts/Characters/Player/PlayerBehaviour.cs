using System;
using System.Collections.Generic;
using UnityEngine;


public class PlayerBehaviour : MonoBehaviour
{
   private Rigidbody rb;
   [SerializeField] private float speed;
   private Vector3 movement;
   [SerializeField] private Weapon weapon;
 

   void Start()
   {
      rb = GetComponent<Rigidbody>();
   }
   void Update()
   {
         movement = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));
        
        if (Input.GetMouseButtonDown(0))
        {
           if (weapon.CanShoot())
           {
              
              weapon.Shoot(transform.forward);
           }
        }
   }

   void FixedUpdate()
   {
      MoveCharacter(movement);
   }

   void MoveCharacter(Vector3 direction)
   {
      rb.velocity = direction * speed * Time.fixedDeltaTime;
   }

}
