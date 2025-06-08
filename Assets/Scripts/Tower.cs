using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    protected float damage = 100f;
    private float _originDamage;
    protected float atkSpeed = 100f;
    protected float fov = 150f;
    public virtual int sellMoney { get; } = 100;
    public virtual int buyMoney { get; } = 100;
    public virtual int upgradeMoney { get; } = 100;

    /*
    0=沒有
    1=普通正硯塔
    2=外語ZX塔
    3=保安子祺塔
    */
    protected int type = 0;

    public FieldOfView _fieldOfView;
    [SerializeField]
    private GameObject _towerObject;
    private Text _towerStateText;
    public List<GameObject> _sameTypeEnemy = new List<GameObject>();
    public List<GameObject> _notSameTypeEnemy = new List<GameObject>();
    public GameObject _enemyATK;

    private bool canAttacked = true;
    private float atkCDCounter = 1f;
    private float atkMultiplier = 1f;
    public bool isUpgrade = false;

    /*
    1=正常
    2=努力
    3=瘋狂努力
    4=摸魚
    5=睡覺
    */
    public int towerState = 1;

    public bool isWatched = false;
    private float _watchedTimer = 0f;
    private bool _isWatchedTimerReset = false;
    private float _notWatchedTimer = 0f;
    private bool _isNotWatchedTimerReset = false;
    
    public virtual void Awake()
    {
        _fieldOfView = GetComponentInChildren<FieldOfView>();
        _towerStateText = GetComponentInChildren<Text>();
        _towerStateText.text = "";
        _fieldOfView.viewAngle = fov;
    }

    public virtual void Update()
    {
        //攻擊後的CD
        if (atkCDCounter <= 0)
        {
            canAttacked = true;
        }
        else
        {
            atkCDCounter -= Time.deltaTime;
        }

        //看有沒有沒看著，然後看幾秒或是沒看幾秒後切狀態
        if (isWatched)
        {
            if (!_isWatchedTimerReset)
            {
                _watchedTimer = 0f;
                _isWatchedTimerReset = true;
            }
            _watchedTimer += Time.deltaTime;
            if (_watchedTimer <= 10f)
            {
                towerState = 1;
                SwitchTowerState();
                _towerStateText.text = "正常";
            }
            else if (_watchedTimer >= 10f && _watchedTimer <= 30f)
            {
                towerState = 2;
                SwitchTowerState();
                _towerStateText.text = "努力";
            }
            else if (_watchedTimer >= 30f)
            {
                towerState = 3;
                SwitchTowerState();
                _towerStateText.text = "瘋狂努力";
            }
        }
        else
        {
            _towerStateText.text = "";
            _isWatchedTimerReset = false;
            if (_watchedTimer >= -50)
            {
                _watchedTimer -= Time.deltaTime;
            }
            if (_watchedTimer <= 10f)
            {
                towerState = 1;
                SwitchTowerState();
                _towerStateText.text = "正常";
            }
            else if (_watchedTimer >= 10f && _watchedTimer <= 30f)
            {
                towerState = 2;
                SwitchTowerState();
                _towerStateText.text = "努力";
            }
            else if (_watchedTimer >= 30f)
            {
                towerState = 3;
                SwitchTowerState();
                _towerStateText.text = "瘋狂努力";
            }
            if (_watchedTimer <= -15)
            {
                towerState = 4;
                SwitchTowerState();
            }
            else if (_watchedTimer <= -30)
            {
                towerState = 5;
                SwitchTowerState();
            }
        }
    }

    public virtual void SetState(float _damage, float _atkSpeed, float _fov, int _type)
    {
        damage = _damage;
        atkSpeed = _atkSpeed;
        fov = _fov;
        type = _type;
        _originDamage = damage;
    }

    public virtual void SetState(float _damage)
    {
        damage = _damage;
    }

    public void Upgrade()
    {
        if (!isUpgrade)
        {
            atkMultiplier = 1.5f;
            isUpgrade = true;
            GameManager.Instance.TakeMoney(-upgradeMoney);
            Debug.Log("OMG is upgrade");
        }
    }

    public virtual void SwitchTowerState()
    { 

    }

    public void TowerAttack()
    {
        if (canAttacked && towerState != 5)
        {
            canAttacked = false;
            atkCDCounter = atkSpeed;
            _enemyATK.GetComponent<Enemy>().TakeDamage(damage * atkMultiplier);
            Debug.Log($"Tower Attack {_enemyATK.gameObject.name}, Damage is {damage * atkMultiplier}");
        }
    }

    public void SetRotation(Quaternion towerRotation)
    {
        _towerObject.transform.rotation = towerRotation;
    }

    public void CheckTarget()
    {
        _sameTypeEnemy.Clear();
        _notSameTypeEnemy.Clear();
        _enemyATK = null;
        foreach (GameObject enemy in _fieldOfView.visibleTargets)
        {
            if (enemy.GetComponent<Enemy>().type == type && type != 1)
            {
                _sameTypeEnemy.Add(enemy);
            }
            else
            {
                _notSameTypeEnemy.Add(enemy);
            }
        }
        if (_sameTypeEnemy.Count > 0)
        {
            for (int i = 0; i < _sameTypeEnemy.Count; i++)
            {
                if (_enemyATK == null)
                {
                    _enemyATK = _sameTypeEnemy[i];
                }
                else
                {
                    if (_sameTypeEnemy[i].transform.position.x > _enemyATK.transform.position.x)
                    {
                        _enemyATK = _sameTypeEnemy[i];
                    }
                }
            }
        }
        else if (_notSameTypeEnemy.Count > 0)
        {
            for (int i = 0; i < _notSameTypeEnemy.Count; i++)
            {
                if (_notSameTypeEnemy[i].GetComponent<Enemy>().type != 2)
                {
                    if (_enemyATK == null)
                    {
                        _enemyATK = _notSameTypeEnemy[i];
                    }
                    else
                    {
                        if (_notSameTypeEnemy[i].transform.position.x > _enemyATK.transform.position.x)
                        {
                            _enemyATK = _notSameTypeEnemy[i];
                        }
                    }
                }
            }
        }
        if (_enemyATK != null)
        {
            if (_enemyATK.GetComponent<Enemy>().type == 3 && type != 3)
            {
                damage = damage / 3;
            }
            TowerAttack();
            damage = _originDamage;
        }
    }
}