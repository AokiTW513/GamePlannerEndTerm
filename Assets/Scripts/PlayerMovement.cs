using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Button skillLockRotationButton;
    public Sprite skillLockSprite;
    public Sprite skillUnlockSprite;
    public Button skillScanAllButton;

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

    public Image scanAllCDMaskImage;
    public Image lockCDMaskImage;

    private void Awake()
    {
        _fov = GetComponent<FieldOfView>();
        skillLockRotationButton.onClick.AddListener(LockRotation);
        skillScanAllButton.onClick.AddListener(ScanAll);

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
                skillLockRotationButton.GetComponent<Image>().sprite = skillUnlockSprite;
            }
        }
        else
        {
            _isLockRotationInCD = true;
            _isLockRotation = false;
            skillLockRotationButton.GetComponent<Image>().sprite = skillLockSprite;
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