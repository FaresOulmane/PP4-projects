using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Weapon", menuName = "Destroyable/Movable", order = 1)]
public class SO_Character : SO_Destroyable
{
    [SerializeField] private float speed;
    public float Speed => speed;
}