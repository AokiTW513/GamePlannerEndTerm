using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("數值")]
    //鎖定視角技能冷卻時間
    [SerializeField]
    private float _skillLockRotationCD = 3f;
    //玩家旋轉速度(正的順時針，負的逆時針)
    [SerializeField]
    private float _rotateAnglePerSecond = -15f;
    //精神喊話技能冷卻時間
    [SerializeField]
    private float _skillScanAllCD = 60f;
    //精神喊話技能持續多久
    [SerializeField]
    private float _skillScanAllTime = 9f;
    //老闆視角角度
    [SerializeField]
    private float _playerFOV = 105;
   
    [Header("物件")]
    [SerializeField]
    private Button _skillLockRotationButton;
    [SerializeField]
    private Sprite _skillLockSprite;
    [SerializeField]
    private Sprite _skillUnlockSprite;
    [SerializeField]
    private Button _skillScanAllButton;

    private bool _isLockRotation = false;
    private bool _isLockRotationInCD = false;
    
    private float _skillLockRotationCDCounter = 0f;

    private bool _isScaning = false;
    private bool _isScanAllInCD = false;
    private float _skillScanAllCDConnter = 0f;
    
    private float _skillScanAllCouner = 0f;

    private float _originFOV;
    private FieldOfView _fov;
    [SerializeField]
    private List<GameObject> _newTowerListInFOV = new List<GameObject>();

    [SerializeField]
    private Image scanAllCDMaskImage;
    public Image lockCDMaskImage;

    private void Awake()
    {
        _fov = GetComponent<FieldOfView>();
        _fov.viewAngle = _playerFOV;
        _skillLockRotationButton.onClick.AddListener(LockRotation);
        _skillScanAllButton.onClick.AddListener(ScanAll);

        scanAllCDMaskImage.fillAmount = 0f;
        lockCDMaskImage.fillAmount = 0f;

        scanAllCDMaskImage.gameObject.SetActive(false);
        lockCDMaskImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!_isLockRotation)
        {
            transform.Rotate(Vector3.up, _rotateAnglePerSecond * Time.deltaTime);
        }
        if (_isLockRotationInCD)
        {
            if (_skillLockRotationCDCounter >= 0)
            {
                lockCDMaskImage.gameObject.SetActive(true);
                _skillLockRotationCDCounter -= Time.deltaTime;
                lockCDMaskImage.fillAmount = Mathf.Clamp01(_skillLockRotationCDCounter / _skillLockRotationCD);
            }
            else
            {
                _isLockRotationInCD = false;
                lockCDMaskImage.gameObject.SetActive(false);
            }
        }
        if (_isScaning)
        {
            if (_skillScanAllCouner >= 0)
            {
                _skillScanAllCouner -= Time.deltaTime;
            }
            else
            {
                _isScaning = false;
                _isScanAllInCD = true;
                _fov.viewAngle = _originFOV;
                _skillScanAllCDConnter = _skillScanAllCD;
            }
        }
        if (_isScanAllInCD)
        {
            if (_skillScanAllCDConnter >= 0)
            {
                scanAllCDMaskImage.gameObject.SetActive(true);
                _skillScanAllCDConnter -= Time.deltaTime;
                scanAllCDMaskImage.fillAmount = Mathf.Clamp01(_skillScanAllCDConnter / _skillScanAllCD);
            }
            else
            {
                _isScanAllInCD = false;
                scanAllCDMaskImage.gameObject.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void LockRotation()
    {
        if (!_isLockRotation)
        {
            if (_skillLockRotationCDCounter <= 0)
            {
                _isLockRotation = true;
                _skillLockRotationButton.GetComponent<Image>().sprite = _skillUnlockSprite;
            }
        }
        else
        {
            _isLockRotationInCD = true;
            _isLockRotation = false;
            _skillLockRotationButton.GetComponent<Image>().sprite = _skillLockSprite;
            if (_skillLockRotationCDCounter <= 0)
            {
                _skillLockRotationCDCounter = _skillLockRotationCD;
            }
        }
    }

    private void ScanAll()
    {
        if (_skillScanAllCDConnter <= 0f && !_isScaning)
        {
            _originFOV = _fov.viewAngle;
            _fov.viewAngle = 360f;
            _skillScanAllCouner = _skillScanAllTime;
            _isScaning = true;
        }
    }

    public void CheckTower()
    {
        List<GameObject> _oldTowerListInFOV = new List<GameObject>(_newTowerListInFOV);
        _newTowerListInFOV.Clear();

        //偵測目前視野內的塔
        foreach (GameObject tower in _fov.visibleTargets)
        {
            _newTowerListInFOV.Add(tower);
            tower.GetComponentInChildren<Tower>().isWatched = true;
        }

        //偵測現在跟之前是否缺少哪個，並讓她isWatched變成false
        foreach (GameObject tower in _oldTowerListInFOV)
        {
            if (tower != null)
            {
                if (!_newTowerListInFOV.Contains(tower))
                {
                    Debug.Log($"塔{tower.gameObject.name}已不在視野內");
                    tower.GetComponentInChildren<Tower>().isWatched = false;
                }
            }
            else
            {
                Debug.Log("tower had been destoryed");
            }
        }
    }
}