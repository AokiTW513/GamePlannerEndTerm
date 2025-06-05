using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Button skillLockRotation;
    public Button skillScanAll;
    private bool isLockRotation = false;
    private bool isUnlockRotation = false;
    private float skillLockRotationCD = 3f;
    private float skillScanAllCD = 5f;
    private float skillLockRotationCDCounter = 0f;
    private float skillScanAllCDConnter = 0f;
    private float rotateAnglePerSecond = 15f;

    private void Awake()
    {
        skillLockRotation.onClick.AddListener(LockRotation);
    }

    private void Update()
    {
        if (!isLockRotation)
        {
            transform.Rotate(Vector3.up, rotateAnglePerSecond * Time.deltaTime);
        }
        if (isUnlockRotation)
        {
            if (skillLockRotationCDCounter >= 0)
            {
                skillLockRotationCDCounter -= Time.deltaTime;
            }
            else
            {
                isUnlockRotation = false;
            }
        } 
    }

    private void LockRotation()
    {
        if (!isLockRotation)
        {
            if (skillLockRotationCDCounter <= 0)
            { 
                isLockRotation = true;
            }
        }
        else
        {
            isUnlockRotation = true;
            isLockRotation = false; 
            if (skillLockRotationCDCounter <= 0)
            {
                skillLockRotationCDCounter = skillLockRotationCD;
            }
        }
    }

    private void ScanAll()
    {

    }
}