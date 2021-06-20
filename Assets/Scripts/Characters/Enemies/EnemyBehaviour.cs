using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private bool turretDetected;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private float life;
    [SerializeField] private float rangeDetectTurret;
    [SerializeField] private float rangeAttack;
    [SerializeField] private int dropAmount;
    private Transform player;
    private GameObject nearestTarget;
    private SpawnerEnnemies spawnerEnnemies;

    private PathRequestManager pathRequestManager;
    private List<Node> path;
    public List<Node> Path
    {
        get => path;
        set => path = value;
    }

    private Node startingNode;
    public Node StartingNode
    {
        get => startingNode;
    }

    private Node targetingNode;

    public Node TargetingNode
    {
        get => targetingNode;
    }
    
    
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spawnerEnnemies = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SpawnerEnnemies>();
        pathRequestManager = FindObjectOfType<PathRequestManager>();
    }

    public void Init()
    {
        pathRequestManager = FindObjectOfType<PathRequestManager>();
        startingNode = GameManager.Instance.ActualGrid.GetNode(transform.position);
        pathRequestManager.AddPath(new PathRequest(this));
    }

    public IEnumerator Move()
    {
        while (life > 0 && path.Any())
        {
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = new Vector3(path[0].position.x, this.transform.position.y, path[0].position.z);
            float startTime = Time.time;
            float journeyLength = Vector3.Distance(startPosition, targetPosition);

            while (life > 0 && transform.position != targetPosition)
            {
                float distCovered = (Time.time - startTime) * speed;
                float fractionOfJourney = distCovered / journeyLength;
                transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);
                FindTurret();
                yield return null;
            }
        }
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        if(!turretDetected)
            ChasePlayer();
        FindTurret();
    }

    void ChasePlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position,speed * Time.deltaTime);
      
    }

    void FindTurret()
    {
        List <GameObject> go = new List<GameObject>(GameManager.Instance.P_EnemiesManager.Targets);
        go.Add(player.gameObject);
        float distance = 1000000f;
        Transform nearestEnemy = player.transform;
        if (!turretDetected)
        {
            for (int i = 0; i < go.Count; i++)
            {
                if (Vector3.Distance(transform.position, go[i].transform.position) < rangeDetectTurret)
                {
                    turretDetected = true;
                    nearestEnemy = go[i].transform;
                }
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, nearestEnemy.position) > rangeAttack)
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(nearestEnemy.position.x,transform.position.y,nearestEnemy.position.z),speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Bullet")
        {
            Die();

        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangeDetectTurret);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeAttack);
    }

    private void Die()
    {
        GameManager.Instance.P_EnemiesManager.RemoveEnemyFromList(gameObject);
        ShopManager.Instance.Coins += dropAmount;
        Destroy(this.gameObject);
    }
    
}
