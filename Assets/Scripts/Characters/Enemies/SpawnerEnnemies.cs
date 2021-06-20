using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnnemies : MonoBehaviour
{
  [SerializeField] public int totalEnemies;
  [SerializeField] public int aliveEnemies;
  [SerializeField] private GameObject enemyPrefab;
    
    
      public IEnumerator Wave()
      {
          aliveEnemies = totalEnemies;
           for (int i = 0; i < totalEnemies; i++)
           {
               yield return new WaitForSeconds(2f);
               GameObject enemy = Instantiate(enemyPrefab, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
               GameObject container = GameObject.Find("EnemyContainer");
               enemy.transform.parent = container.transform;
           }
      }
}
