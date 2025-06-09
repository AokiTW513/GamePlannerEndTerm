using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int _health;
    private int _maxHealth = 10;
    [SerializeField]
    private Text _healthText;
    private int _money = 320;
    [SerializeField]
    private Text _moneyText;
    private bool isLose = false;
    private bool isWin = false;

    public int _wave = 0;
    private int _waveEnd = 5;
    [SerializeField]
    private Text _waveText;
    public float _spawnDelay = 1f;
    private float _spawnDelayCounter = 0f;
    private bool _isAllEnemySpawned = true;
    public float _waveDelay = 5f;
    private float _waveDelayCounter = 0f;
    [SerializeField]
    private Text _waveCDText;
    private bool _isWaveEnable = false;
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

    public Button doubleSpeedButton;
    private bool _isDoubleSpeed = false;
    public Sprite singleSpeed;
    public Sprite doubleSpeed;

    private Vector3 _spawnPosition = new Vector3(-10, 0, 0);

    public GameObject _towerManagerPrefab; // 放置用點的Prefab（可以只是空物件或帶小圓圖）
    private int numberOfPoints = 12;
    private float radius = 5f;
    private Vector3 _offset = new Vector3(0, 1, 0);

    public Button restartButton;
    public Button mainmenuButton;
    public Button mainmenuPauseButton;
    public Button continueButton;
    public Button pauseButton;
    public GameObject pauseUIObject;
    public GameObject restartMainMenuUI;
    public GameObject winUI;
    public GameObject loseUI;

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

        _currentBuyTower = null;
        buyTower1Button.onClick.AddListener(() => ChooseBuyTower(tower1, 1, buyTower1Button));
        buyTower2Button.onClick.AddListener(() => ChooseBuyTower(tower2, 2, buyTower2Button));
        buyTower3Button.onClick.AddListener(() => ChooseBuyTower(tower3, 3, buyTower3Button));

        waveStartButton.gameObject.SetActive(true);
        waveStartButton.onClick.AddListener(OnStartButton);

        upgradeButton.onClick.AddListener(UpgradeTower);
        ShowUI(upgradeSellUIObject, false);
        _currentChooseTower = null;
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

        Time.timeScale = 1f;
        SpawnTowerPlace();
    }

    private void Update()
    {
        if (!isLose || !isWin)
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
                            if (_currentBuyTower != null)
                            {
                                if (_money >= _currentBuyTower.GetComponent<Tower>().buyMoney)
                                {
                                    if (hit.collider.gameObject.GetComponent<TowerManager>().towerType == 0)
                                    {
                                        hit.collider.gameObject.GetComponent<TowerManager>().BuyTower(_currentBuyTowerType, _currentBuyTower);
                                        CancelBuyUpgrade();
                                    }
                                    else
                                    {
                                        Debug.Log("bro u can't place tower again :(");
                                        CancelBuyUpgrade();
                                    }
                                }
                                else
                                {
                                    Debug.Log("bro u don't have money lmao");
                                    CancelBuyUpgrade();
                                }
                            }
                            else
                            {
                                Debug.Log("You didn't choose Tower");
                            }
                        }
                        else
                        {
                            CancelBuyUpgrade();
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

    private void ShowOutline(bool show)
    {
        buyTower1Button.GetComponent<Outline>().enabled = show;
        buyTower2Button.GetComponent<Outline>().enabled = show;
        buyTower3Button.GetComponent<Outline>().enabled = show;
    }

    private void CancelBuyUpgrade()
    {
        ShowUI(upgradeSellUIObject, false);
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
        _isWaveEnable = true;
        waveStartButton.gameObject.SetActive(false);
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
        pauseUIObject.SetActive(true);
        Time.timeScale = 0f;
    }

    private void ShowUI(GameObject UIObject, bool show)
    {
        if (show)
        {
            if (_currentChooseTower.currentTower.GetComponent<Tower>().isUpgrade)
            {
                upgradeButton.interactable = false;
            }
            upgradeText.text = $"${_currentChooseTower.currentTower.GetComponent<Tower>().upgradeMoney}";
            sellText.text = $"${_currentChooseTower.currentTower.GetComponent<Tower>().sellMoney}";
            UIObject.SetActive(show);
        }
        else
        {
            UIObject.SetActive(show);
            upgradeButton.interactable = true;
        }
    }

    private void UpgradeTower()
    {
        if (!isLose || !isWin)
        {
            if (_currentChooseTower != null)
            {
                if (_money >= _currentChooseTower.currentTower.GetComponent<Tower>().upgradeMoney)
                {
                    _currentChooseTower.currentTower.GetComponent<Tower>().Upgrade();
                    _currentChooseTower = null;
                    _currentBuyTower = null;
                    ShowUI(upgradeSellUIObject, false);
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
        if (!isLose || !isWin)
        {
            if (_currentChooseTower != null)
            {
                _currentChooseTower.SellTower();
                _currentChooseTower = null;
                _currentBuyTower = null;
                upgradeSellUIObject.SetActive(false);
            }
        }
    }

    private void ChooseBuyTower(GameObject tower, int type, Button button)
    {
        if (!isLose || !isWin)
        {
            CancelBuyUpgrade();
            _currentBuyTower = tower;
            _currentBuyTowerType = type;
            Debug.Log($"You Choose {type}");
            button.GetComponent<Outline>().enabled = true;
            ShowUI(upgradeSellUIObject, false);
            _currentChooseTower = null;
        }
    }

    private void SetDoubleSpeed()
    {
        if (!isLose || !isWin)
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
        _waveText.text = $"波次:{_wave}/{_waveEnd}";
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
        Instantiate(_spawnEnemyList[rnd], _spawnPosition + _offset, Quaternion.identity);
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
        _healthText.text = $"血量:{_health}/{_maxHealth}";
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
        _moneyText.text = $"錢:{_money}";
    }
}