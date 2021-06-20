using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    [SerializeField] private float delayBeforeFirstSpawn;
    [SerializeField] private float delayBetweenSpawns;
    [SerializeField] private int totalEnemies;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject container;

    private List<GameObject> targets;
    public List<GameObject> Targets => targets;

    private float timerBeforeNextSpawn;
    
    private int enemiesLeftToSpawn;
    private List<GameObject> aliveEnemies;

    private Coroutine cor;
    
    void Start()
    {
        aliveEnemies = new List<GameObject>();
        if (container == null) GameObject.Find("EnemyContainer");
        // StartCoroutine(nameof(StartWave));
    }

    public void StartWaveSequence()
    {
        if(cor == null)
            cor = StartCoroutine(StartWave());
    }

    // Commence une vague et spawn des ennemis
    IEnumerator StartWave()
    {
        enemiesLeftToSpawn = totalEnemies;
        GetTargets();
        yield return new WaitForSeconds(delayBeforeFirstSpawn);
        while (!isWaveFinished())
        {
            if (enemiesLeftToSpawn > 0)
            {
                if (timerBeforeNextSpawn <= 0f)
                {
                    GameObject enemy = Instantiate(enemyPrefab,
                        new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
                    enemy.transform.parent = container.transform;
                    aliveEnemies.Add(enemy);

                    --enemiesLeftToSpawn;
                    timerBeforeNextSpawn = delayBetweenSpawns;
                }
                else
                {
                    timerBeforeNextSpawn -= Time.deltaTime;
                }
            }

            yield return null;
        }

        cor = null;
        GameManager.Instance.ChangePhase(GameStateEnum.Shop);
    }

    public void RemoveEnemyFromList(GameObject enemy)
    {
        aliveEnemies.Remove(enemy);
    }

    public bool isWaveFinished()
    {
        if (aliveEnemies.Count <= 0 && enemiesLeftToSpawn == 0) return true;
        return false;
    }

    private void GetTargets()
    {
        targets = new List<GameObject>(GameObject.FindGameObjectsWithTag("Turret"));
    }
    
    /*public List<GameObject>()
    {
        
    }*/
}
