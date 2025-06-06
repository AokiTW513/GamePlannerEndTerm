using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int _health = 10;
    public int money = 100;
    private bool isLose = false;
    private bool isWin = false;

    public int _wave = 0;
    public float _spawnDelay = 1f;
    public float _spawnDelayCounter = 0f;
    private bool _isAllEnemySpawned = true;
    public float _waveDelay = 5f;
    public float _waveDelayCounter = 0f;
    public bool _isWaveEnable = true;

    private List<GameObject> _spawnEnemyList = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();

    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Enemy3;

    private Vector3 _spawnPosition = new Vector3(-10, 0, 0);

    private void Awake()
    {
        // 如果場上已經有一個 GameManager，這個就砍掉，保證只有一個
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 場景切換不會消失

        _waveDelayCounter = _waveDelay;
        _isAllEnemySpawned = true;
        _spawnDelayCounter = 0f;

        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (_isWaveEnable)
        {
            if (_isAllEnemySpawned)
            {
                if (_wave == 5)
                {
                    if (enemies.Count == 0 && !isLose)
                    {
                        isWin = true;
                        Time.timeScale = 0f;
                        Debug.Log("You Win!");
                    }
                }
                if (_waveDelayCounter <= 0)
                {
                    _isAllEnemySpawned = false;
                    _wave = _wave + 1;
                    _spawnDelayCounter = 0f;
                    NewWave();
                }
                else if (enemies.Count == 0)
                {
                    _waveDelayCounter -= Time.deltaTime;
                }
            }
            else
            {
                if (_spawnDelayCounter <= 0)
                {
                    SpawnEnemy();
                }
                else
                {
                    _spawnDelayCounter -= Time.deltaTime;
                }
            }
        }
    }

    private void NewWave()
    {
        switch (_wave)
        {
            case 1:
                for (int i = 0; i < 10; i++)
                {
                    _spawnEnemyList.Add(ChooseEnemy(1));
                }
                break;
            case 2:
                for (int i = 0; i < 15; i++)
                {
                    _spawnEnemyList.Add(ChooseEnemy(2));
                }
                break;
            case 3:
                for (int i = 0; i < 20; i++)
                {
                    _spawnEnemyList.Add(ChooseEnemy(2));
                }
                break;
            case 4:
                for (int i = 0; i < 25; i++)
                {
                    _spawnEnemyList.Add(ChooseEnemy(3));
                }
                break;
            case 5:
                for (int i = 0; i < 30; i++)
                {
                    _spawnEnemyList.Add(ChooseEnemy(3));
                }
                break;
        }
    }

    private GameObject ChooseEnemy(int _index)
    {
        switch (Random.Range(1, _index + 1))
        {
            case 1:
                return Enemy1;
            case 2:
                return Enemy2;
            case 3:
                return Enemy3;
            default:
                Debug.Log("物件為Null，不知道為什麼出現1~3以外的情況");
                return null;
        }
    }

    private void SpawnEnemy()
    {
        int rnd = Random.Range(0, _spawnEnemyList.Count);
        Instantiate(_spawnEnemyList[rnd], _spawnPosition, Quaternion.identity);
        _spawnEnemyList.RemoveAt(rnd);
        _spawnDelayCounter = _spawnDelay;
        if (_spawnEnemyList.Count == 0)
        {
            _isAllEnemySpawned = true;
            _waveDelayCounter = _waveDelay;
        }
    }

    public void TakeDamage()
    {
        _health -= 1;
        if (_health <= 0)
        {
            isLose = true;
            Debug.Log("You Lose!");
            Time.timeScale = 0f;
        }
    }
}