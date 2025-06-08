using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Button skillLockRotationButton;
    public Text skillLockRotationButtonText;

    public Button skillScanAllButton;
    public Text skillScanAllButtonText;

    private bool _isLockRotation = false;
    private bool _isLockRotationInCD = false;
    private float _skillLockRotationCD = 3f;
    private float _skillLockRotationCDCounter = 0f;
    private float _rotateAnglePerSecond = -15f;

    private bool _isScaning = false;
    private bool _isScanAllInCD = false;
    private float _skillScanAllCD = 60f;
    private float _skillScanAllCDConnter = 0f;
    private float _skillScanAllTime = 9f;
    private float _skillScanAllCouner = 0f;

    private float _originFOV;
    private FieldOfView _fov;
    public List<GameObject> _newTowerListInFOV = new List<GameObject>();
    public GameObject enemy123;

    private void Awake()
    {
        _fov = GetComponent<FieldOfView>();
        skillLockRotationButton.onClick.AddListener(LockRotation);
        skillScanAllButton.onClick.AddListener(ScanAll);
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
                _skillLockRotationCDCounter -= Time.deltaTime;
                skillLockRotationButtonText.text = $"CD:{_skillLockRotationCDCounter:F1}s";
            }
            else
            {
                _isLockRotationInCD = false;
                skillLockRotationButtonText.text = "Lock";
            }
        }
        if (_isScaning)
        {
            if (_skillScanAllCouner >= 0)
            {
                _skillScanAllCouner -= Time.deltaTime;
                skillScanAllButtonText.text = $"Scaning({_skillScanAllCouner:F1}s)";
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
                _skillScanAllCDConnter -= Time.deltaTime;
                skillScanAllButtonText.text = $"CD:{_skillScanAllCDConnter:F1}s";
            }
            else
            {
                _isScanAllInCD = false;
                skillScanAllButtonText.text = "Scan All";
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
                skillLockRotationButtonText.text = "Unlock";
            }
        }
        else
        {
            _isLockRotationInCD = true;
            _isLockRotation = false;
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
            if (!_newTowerListInFOV.Contains(tower))
            {
                Debug.Log($"塔{tower.gameObject.name}已不在視野內");
                tower.GetComponentInChildren<Tower>().isWatched = false;
            }
        }
    }
}