using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Weapon", menuName = "Destroyable/Weapon", order = 1)]
public class SO_Destroyable : ScriptableObject
{
    [SerializeField] private string name;
    public string Name => name;
    
    [SerializeField] private float healthPoints;
    public float HealthPoints => healthPoints;
}