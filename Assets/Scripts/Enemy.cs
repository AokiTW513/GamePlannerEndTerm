using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    protected float _maxHealth = 100f;
    private float _health;
    [SerializeField]
    private Text _healthText;
    [SerializeField]
    private GameObject _enemyObject;
    // protected float _speed = 3f;
    protected int _dropMoney = 100;
    // private float _rotationSpeed = 5f;
    // private int _currentWayPointIndex = 0;
    /*
    0=沒有
    1=普通客戶
    2=外語客戶
    3=奧客
    */
    protected int _type = 0;

    // private string _wayPointAName = "WayPointA";
    // private string _wayPointBName = "WayPointB";
    // private Transform[] _wayPoints;
    // private int _chooseWayPoint = 1;

    public int type { get => _type; }

    public virtual void Awake()
    {
        _health = _maxHealth;
        _healthText.text = $"血量:{_health}/{_maxHealth}";
        // _chooseWayPoint = Random.Range(1, 3);
        // switch (_chooseWayPoint)
        // {
        //     case 1:
        //         SetWayPoint(_wayPointAName);
        //         break;
        //     case 2:
        //         SetWayPoint(_wayPointBName);
        //         break;
        // }
    }

    private void OnEnable()
    {
        GameManager.Instance.enemies.Add(gameObject);
    }

    private void OnDisable()
    {
        GameManager.Instance.enemies.Remove(gameObject);
    }

    private void Update()
    {
        // if (_wayPoints.Length == 0) return;

        // Transform targetWaypoint = _wayPoints[_currentWayPointIndex];
        // Vector3 dir = (targetWaypoint.position - transform.position).normalized;

        // // 平滑轉向
        // if (dir != Vector3.zero)
        // {
        //     Quaternion targetRotation = Quaternion.LookRotation(dir);
        //     _enemyObject.transform.rotation = Quaternion.Slerp(_enemyObject.transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        // }

        // // 移動
        // transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, _speed * Time.deltaTime);

        // // 到達該 waypoint，切換到下一個
        // if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        // {
        //     _currentWayPointIndex = _currentWayPointIndex + 1; // 循環走
        // }

        // if (_currentWayPointIndex == _wayPoints.Length)
        // {
        //     GameManager.Instance.TakeDamage();
        //     Destroy(gameObject);
        // }
    }

    // private void SetWayPoint(string wayPointName)
    // {
    //     GameObject wayPointGroup = GameObject.Find(wayPointName);
    //     if (wayPointGroup != null)
    //     {
    //         _wayPoints = new Transform[wayPointGroup.transform.childCount];
    //         for (int i = 0; i < wayPointGroup.transform.childCount; i++)
    //         {
    //             _wayPoints[i] = wayPointGroup.transform.GetChild(i);
    //         }
    //     }
    //     else
    //     {
    //         Debug.Log($"不是，我找不到{wayPointName}誒老兄");
    //     }
    // }

    public void TakeDamage(float _damage)
    {
        _health -= _damage;
        _healthText.text = $"血量:{_health}/{_maxHealth}";
        if (_health <= 0)
        {
            Destroy(gameObject);
            GameManager.Instance.TakeMoney(_dropMoney);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EndPoint")
        {
            Destroy(gameObject);
            GameManager.Instance.TakeDamage();
        }
    }
}