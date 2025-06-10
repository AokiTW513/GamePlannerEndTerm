using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using UnityEngine.SceneManagement;
using System.Linq;
using Unity.VisualScripting;

[System.Serializable]
public class EnemySpawnData
{
    public int Wave;
    public int Enemy;
    public int EnemyCount;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("數值")]
    //玩家血量
    [SerializeField]
    private int _maxHealth = 10;
    //玩家的錢
    [SerializeField]
    private int _money = 320;
    //敵人生成間隔
    [SerializeField]
    private float _spawnDelay = 1f;
    //每波的間隔(包含按開始遊戲後的那個間隔)
    [SerializeField]
    private float _waveDelay = 5f;
    //總共可以放幾個塔
    [SerializeField]
    private int numberOfPoints = 12;
    //以中心點來看半徑多少要放塔
    [SerializeField]
    private float radius = 5f;

    [Header("物件")]
    public GameObject Player;
    private int _health;
    
    [SerializeField]
    private Text _healthText;
    
    [SerializeField]
    private Text _moneyText;
    private bool isLose = false;
    private bool isWin = false;

    private int _wave = 0;
    private int _waveEnd = 5;
    [SerializeField]
    private Text _waveText;
    private float _spawnDelayCounter = 0f;
    private bool _isAllEnemySpawned = true;
    private float _waveDelayCounter = 0f;
    [SerializeField]
    private Text _waveCDText;
    public bool _isWaveEnable = false;
    public Button waveStartButton;

    private List<GameObject> _spawnEnemyList = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();

    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Enemy3;

    public GameObject tower1;
    public GameObject tower2;
    public GameObject tower3;
    public Button buyTower1Button;
    public Button buyTower2Button;
    public Button buyTower3Button;
    public Text tower1PriceText;
    public Text tower2PriceText;
    public Text tower3PriceText;
    private GameObject _currentBuyTower;
    private int _currentBuyTowerType;
    public LayerMask placeLayer;

    private TowerManager _currentChooseTower;
    public Button upgradeButton;
    public Text upgradeText;
    public GameObject upgradeSellUIObject;
    public Button sellButton;
    public Text sellText;
    public GameObject buyTowerUIObject;

    public Button doubleSpeedButton;
    private bool _isDoubleSpeed = false;
    public Sprite singleSpeed;
    public Sprite doubleSpeed;
    private Vector3 _spawnPosition = new Vector3(-25, 0, 0);

    public GameObject _towerManagerPrefab; // 放置用點的Prefab（可以只是空物件或帶小圓圖）

    private Vector3 _offset = new Vector3(0, 1, 0);

    public Button restartButton;
    public Button mainmenuButton;
    public Button mainmenuPauseButton;
    public Button continueButton;
    public Button pauseButton;
    public GameObject pauseUIObject;
    private bool _isPause = false;
    public GameObject restartMainMenuUI;
    public GameObject winUI;
    public GameObject loseUI;
    public Button galleryButton;
    public Button galleryBackButton;
    public GameObject galleryUIObject;
    public GameObject galleryTowerUIObject;
    public GameObject galleryEnemyUIObject;
    public GameObject galleryBossUIObject;
    public GameObject galleryStateUIObject;
    public Button towerGalleryButton;
    public Button towerGalleryBackButton;
    public Button enemyGalleryButton;
    public Button enemyGalleryBackButton;
    public Button bossGalleryButton;
    public Button bossGalleryBackButton;
    public Button stateGalleryButton;
    public Button stateGalleryBackButton;
    public Button tower1Intro;
    public Button tower2Intro;
    public Button tower3Intro;
    public Button enemy1Intro;
    public Button enemy2Intro;
    public Button enemy3Intro;
    public Button state1Intro;
    public Button state2Intro;
    public Button state3Intro;
    public Button state4Intro;
    public Button state5Intro;
    public Text towerGalleryText;
    public Text enemyGalleryText;
    public Text stateGalleryText;
    [TextArea(3, 7)]
    public List<string> descriptions = new List<string>();

    private List<EnemySpawnData> _enemySpawnDataList;

    private void Awake()
    {
        // 如果場上已經有一個 GameManager，這個就砍掉，保證只有一個
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _waveDelayCounter = _waveDelay;
        _isAllEnemySpawned = true;
        _spawnDelayCounter = 0f;

        _money = 320;

        _health = _maxHealth;

        _enemySpawnDataList = CSVReader.ReadEnemySpawnData("Level1.csv");
        _waveEnd = _enemySpawnDataList.Max(e => e.Wave);

        _currentBuyTower = null;
        buyTower1Button.onClick.AddListener(() => ChooseBuyTower(tower1, 1));
        buyTower2Button.onClick.AddListener(() => ChooseBuyTower(tower2, 2));
        buyTower3Button.onClick.AddListener(() => ChooseBuyTower(tower3, 3));

        waveStartButton.gameObject.SetActive(true);
        waveStartButton.onClick.AddListener(OnStartButton);

        buyTowerUIObject.SetActive(false);
        upgradeButton.onClick.AddListener(UpgradeTower);
        ShowUI(upgradeSellUIObject, false);
        _currentChooseTower = null;
        sellButton.onClick.AddListener(SellTower);
        doubleSpeedButton.onClick.AddListener(SetDoubleSpeed);
        _waveText.text = $"波次:{_wave}/{_waveEnd}";
        _healthText.text = $"{_health}/{_maxHealth}";
        _moneyText.text = $"${_money}";
        _waveCDText.text = $"距離下一波還有{_waveDelayCounter:F1}s";
        tower1PriceText.text = $"${tower1.GetComponent<Tower>().buyMoney}";
        tower2PriceText.text = $"${tower2.GetComponent<Tower>().buyMoney}";
        tower3PriceText.text = $"${tower3.GetComponent<Tower>().buyMoney}";

        restartButton.onClick.AddListener(OnRestartButton);
        mainmenuButton.onClick.AddListener(OnMainMenuButton);
        mainmenuPauseButton.onClick.AddListener(OnMainMenuButton);
        continueButton.onClick.AddListener(OnContinueButton);
        pauseButton.onClick.AddListener(OnPauseButton);
        pauseUIObject.SetActive(false);
        restartMainMenuUI.SetActive(false);
        winUI.SetActive(false);
        loseUI.SetActive(false);

        galleryButton.onClick.AddListener(() => OnGalleryButton(galleryUIObject, true));
        galleryBackButton.onClick.AddListener(() => OnGalleryButton(galleryUIObject, false));

        enemyGalleryButton.onClick.AddListener(() => OnGalleryButton(galleryEnemyUIObject, true));
        enemyGalleryBackButton.onClick.AddListener(() => OnGalleryButton(galleryEnemyUIObject, false));

        towerGalleryButton.onClick.AddListener(() => OnGalleryButton(galleryTowerUIObject, true));
        towerGalleryBackButton.onClick.AddListener(() => OnGalleryButton(galleryTowerUIObject, false));

        bossGalleryButton.onClick.AddListener(() => OnGalleryButton(galleryBossUIObject, true));
        bossGalleryBackButton.onClick.AddListener(() => OnGalleryButton(galleryBossUIObject, false));

        stateGalleryButton.onClick.AddListener(() => OnGalleryButton(galleryStateUIObject, true));
        stateGalleryBackButton.onClick.AddListener(() => OnGalleryButton(galleryStateUIObject, false));

        tower1Intro.onClick.AddListener(() => OnGalleryText(towerGalleryText, 3));
        tower2Intro.onClick.AddListener(() => OnGalleryText(towerGalleryText, 4));
        tower3Intro.onClick.AddListener(() => OnGalleryText(towerGalleryText, 5));

        enemy1Intro.onClick.AddListener(() => OnGalleryText(enemyGalleryText, 0));
        enemy2Intro.onClick.AddListener(() => OnGalleryText(enemyGalleryText, 1));
        enemy3Intro.onClick.AddListener(() => OnGalleryText(enemyGalleryText, 2));

        state1Intro.onClick.AddListener(() => OnGalleryText(stateGalleryText, 6));
        state2Intro.onClick.AddListener(() => OnGalleryText(stateGalleryText, 7));
        state3Intro.onClick.AddListener(() => OnGalleryText(stateGalleryText, 8));
        state4Intro.onClick.AddListener(() => OnGalleryText(stateGalleryText, 9));
        state5Intro.onClick.AddListener(() => OnGalleryText(stateGalleryText, 10));

        galleryUIObject.SetActive(false);
        galleryEnemyUIObject.SetActive(false);
        galleryTowerUIObject.SetActive(false);
        galleryBossUIObject.SetActive(false);
        galleryStateUIObject.SetActive(false);

        Time.timeScale = 1f;
        _isPause = false;
        SpawnTowerPlace();
    }

    private void Update()
    {
        if (!isLose && !isWin && !_isPause)
        {
            if (_isWaveEnable)
            {
                if (_isAllEnemySpawned)
                {
                    if (_wave == _waveEnd)
                    {
                        if (enemies.Count == 0 && !isLose)
                        {
                            isWin = true;
                            OnWin();
                            Time.timeScale = 0f;
                            Debug.Log("You Win!");
                        }
                    }
                    if (_waveDelayCounter <= 0)
                    {
                        _waveCDText.text = "";
                        _isAllEnemySpawned = false;
                        _wave = _wave + 1;
                        _spawnDelayCounter = 0f;
                        NewWave();
                    }
                    else if (enemies.Count == 0)
                    {
                        _waveDelayCounter -= Time.deltaTime;
                        _waveCDText.text = $"距離下一波還有{_waveDelayCounter:F1}s";
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

            if (IsTouchOrClickDown())
            {
                bool clickedUI = false;
                if (Input.touchCount > 0)
                {
                    clickedUI = EventSystem.current.IsPointerOverGameObject(Pointer.current.deviceId);
                }
                else
                {
                    clickedUI = EventSystem.current.IsPointerOverGameObject();
                }

                if (clickedUI) return;

                //一次偵測多個Layer不偵測
                int excludeLayers = (1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Bullet")) | (1 << LayerMask.NameToLayer("Tower")) | (1 << LayerMask.NameToLayer("Player"));
                int finalMask = ~excludeLayers;

                Vector3 inputPos = Input.touchCount > 0 ? Input.GetTouch(0).position : Input.mousePosition;
                Ray ray = Camera.main.ScreenPointToRay(inputPos);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100f, finalMask))
                {
                    if (hit.collider.gameObject.GetComponent<TowerManager>() != null)
                    {
                        if (hit.collider.gameObject.GetComponent<TowerManager>().towerType == 0)
                        {
                            _currentChooseTower = hit.collider.gameObject.GetComponent<TowerManager>();
                            ShowUI(buyTowerUIObject, true);
                        }
                        else
                        {
                            _currentChooseTower = hit.collider.gameObject.GetComponent<TowerManager>();
                            ShowUI(upgradeSellUIObject, true);
                        }
                    }
                    else
                    {
                        CancelBuyUpgrade();
                    }
                }
                else
                {
                    CancelBuyUpgrade();
                }
            }
        }
    }
    private bool IsTouchOrClickDown()
    {
        // 滑鼠點擊
        if (Input.GetMouseButtonDown(0))
            return true;

        // 手機點擊
        if (Touch.activeTouches.Count > 0 && Touch.activeTouches[0].phase == UnityEngine.InputSystem.TouchPhase.Began)
            return true;

        return false;
    }

    private void OnGalleryText(Text text, int index)
    {
        text.text = descriptions[index];
    }

    private void OnGalleryButton(GameObject UI, bool show)
    {
        UI.SetActive(show);
        towerGalleryText.text = "";
        enemyGalleryText.text = "";
        stateGalleryText.text = "";
    }

    private void ShowOutline(bool show)
    {
        buyTower1Button.GetComponent<Outline>().enabled = show;
        buyTower2Button.GetComponent<Outline>().enabled = show;
        buyTower3Button.GetComponent<Outline>().enabled = show;
    }

    private void CancelBuyUpgrade()
    {
        ShowUI(upgradeSellUIObject, false);
        ShowUI(buyTowerUIObject, false);
        _currentBuyTower = null;
        _currentChooseTower = null;
        ShowOutline(false);
        Debug.Log("Cancel");
    }

    private void OnWin()
    {
        winUI.SetActive(true);
        restartMainMenuUI.SetActive(true);
    }

    private void OnLose()
    {
        loseUI.SetActive(true);
        restartMainMenuUI.SetActive(true);
    }

    private void OnStartButton()
    {
        if (!_isPause)
        {
            _isWaveEnable = true;
            waveStartButton.gameObject.SetActive(false);
        }
    }

    private void OnRestartButton()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void OnMainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnContinueButton()
    {
        pauseUIObject.SetActive(false);
        _isPause = false;
        if (!_isDoubleSpeed)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 2f;
        }
    }

    private void OnPauseButton()
    {
        if (!isLose && !isWin)
        {
            _isPause = true;
            pauseUIObject.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    private void ShowUI(GameObject UIObject, bool show)
    {
        if (show)
        {
            upgradeSellUIObject.SetActive(false);
            buyTowerUIObject.SetActive(false);
            if (_currentChooseTower.currentTower != null)
            {
                CheckMoney(_currentChooseTower.currentTower.GetComponent<Tower>().upgradeMoney, upgradeButton);
                if (_currentChooseTower.currentTower.GetComponent<Tower>().isUpgrade)
                {
                    upgradeButton.interactable = false;
                    upgradeText.text = "已升級";
                }
                else
                {
                    upgradeText.text = $"${_currentChooseTower.currentTower.GetComponent<Tower>().upgradeMoney}";
                }
                sellText.text = $"${_currentChooseTower.currentTower.GetComponent<Tower>().sellMoney}";
            }
            else
            {
                CheckMoney(tower1.GetComponent<Tower>().buyMoney, buyTower1Button);
                CheckMoney(tower2.GetComponent<Tower>().buyMoney, buyTower2Button);
                CheckMoney(tower3.GetComponent<Tower>().buyMoney, buyTower3Button);
            }
            UIObject.SetActive(show);
        }
        else
        {
            UIObject.SetActive(show);
            upgradeButton.interactable = true;
        }
    }

    private void CheckMoney(int price, Button button)
    {
        if (_money >= price)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }

    private void UpgradeTower()
    {
        if (!isLose && !isWin && !_isPause)
        {
            if (_currentChooseTower != null)
            {
                if (_money >= _currentChooseTower.currentTower.GetComponent<Tower>().upgradeMoney)
                {
                    _currentChooseTower.currentTower.GetComponent<Tower>().Upgrade();
                    CancelBuyUpgrade(); 
                }
                else
                {
                    Debug.Log("no money to upgrade");
                }
            }
        }
    }

    private void SellTower()
    {
        if (!isLose && !isWin && !_isPause)
        {
            if (_currentChooseTower != null)
            {
                _currentChooseTower.SellTower();
                CancelBuyUpgrade(); 
            }
        }
    }

    private void ChooseBuyTower(GameObject tower, int type)
    {
        if (!isLose && !isWin && !_isPause)
        {
            // CancelBuyUpgrade();
            // _currentBuyTower = tower;
            // _currentBuyTowerType = type;
            // Debug.Log($"You Choose {type}");
            // button.GetComponent<Outline>().enabled = true;
            // ShowUI(upgradeSellUIObject, false);
            // _currentChooseTower = null;
            _currentChooseTower.BuyTower(type, tower);
            CancelBuyUpgrade();
        }
    }

    private void SetDoubleSpeed()
    {
        if (!isLose && !isWin && !_isPause)
        {
            if (Time.timeScale == 1f)
            {
                Time.timeScale = 2f;
                _isDoubleSpeed = true;
                doubleSpeedButton.GetComponent<Image>().sprite = doubleSpeed;
            }
            else if (Time.timeScale == 2f)
            {
                Time.timeScale = 1f;
                _isDoubleSpeed = false;
                doubleSpeedButton.GetComponent<Image>().sprite = singleSpeed;
            }
        }
    }

    private void NewWave()
    {
        // _waveText.text = $"波次:{_wave}/{_waveEnd}";
        // switch (_wave)
        // {
        //     case 1:
        //         for (int i = 0; i < 10; i++)
        //         {
        //             _spawnEnemyList.Add(ChooseEnemy(1));
        //         }
        //         break;
        //     case 2:
        //         for (int i = 0; i < 15; i++)
        //         {
        //             _spawnEnemyList.Add(ChooseEnemy(2));
        //         }
        //         break;
        //     case 3:
        //         for (int i = 0; i < 20; i++)
        //         {
        //             _spawnEnemyList.Add(ChooseEnemy(2));
        //         }
        //         break;
        //     case 4:
        //         for (int i = 0; i < 25; i++)
        //         {
        //             _spawnEnemyList.Add(ChooseEnemy(3));
        //         }
        //         break;
        //     case 5:
        //         for (int i = 0; i < 30; i++)
        //         {
        //             _spawnEnemyList.Add(ChooseEnemy(3));
        //         }
        //         break;
        // }
        _waveText.text = $"波次:{_wave}/{_waveEnd}";

        var waveEnemies = _enemySpawnDataList.FindAll(e => e.Wave == _wave);
        if (waveEnemies.Count == 0)
        {
            Debug.LogWarning($"找不到第 {_wave} 波的資料");
            return;
        }

        foreach (var enemyData in waveEnemies)
        {
            for (int i = 0; i < enemyData.EnemyCount; i++)
            {
                _spawnEnemyList.Add(ChooseEnemy(enemyData.Enemy));
            }
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
        // Instantiate(_spawnEnemyList[rnd], _spawnPosition + _offset, Quaternion.identity);
        Instantiate(_spawnEnemyList[rnd], _spawnPosition + _offset, Quaternion.Euler(0, 90, 0));
        _spawnEnemyList.RemoveAt(rnd);
        _spawnDelayCounter = _spawnDelay;
        if (_spawnEnemyList.Count == 0)
        {
            _isAllEnemySpawned = true;
            _waveDelayCounter = _waveDelay;
        }
    }

    private void SpawnTowerPlace()
    {
        for (int i = 0; i < numberOfPoints; i++)
        {
            float angle = i * Mathf.PI * 2f / numberOfPoints;
            Vector3 newPos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;

            GameObject towerManager = Instantiate(_towerManagerPrefab, transform);
            towerManager.transform.localPosition = newPos;
            towerManager.transform.LookAt(transform.position); // 可選，讓點朝向中心
            towerManager.transform.Rotate(0f, 180f, 0f);  
        }
    }

    public void TakeDamage()
    {
        _health -= 1;
        _healthText.text = $"{_health}/{_maxHealth}";
        if (_health <= 0)
        {
            isLose = true;
            OnLose();
            Debug.Log("You Lose!");
            Time.timeScale = 0f;
        }
    }

    public void TakeMoney(int money)
    {
        _money += money;
        _moneyText.text = $"${_money}";
        if (_currentChooseTower != null)
        {
            if (_currentChooseTower.currentTower != null)
            {
                CheckMoney(_currentChooseTower.currentTower.GetComponent<Tower>().upgradeMoney, upgradeButton);
                if (_currentChooseTower.currentTower.GetComponent<Tower>().isUpgrade)
                {
                    upgradeButton.interactable = false;
                    upgradeText.text = "已升級";
                }
                else
                {
                    upgradeText.text = $"${_currentChooseTower.currentTower.GetComponent<Tower>().upgradeMoney}";
                }
                sellText.text = $"${_currentChooseTower.currentTower.GetComponent<Tower>().sellMoney}";
            }
            else
            {
                CheckMoney(tower1.GetComponent<Tower>().buyMoney, buyTower1Button);
                CheckMoney(tower2.GetComponent<Tower>().buyMoney, buyTower2Button);
                CheckMoney(tower3.GetComponent<Tower>().buyMoney, buyTower3Button);
            }
        }
    }
}