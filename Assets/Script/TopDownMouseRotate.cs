using UnityEngine;
using UnityEngine.UI;

public class TopDownMouseRotate : MonoBehaviour
{
    public Camera Cam;
    public LayerMask groundLayer;
    public Text RotateCDText;

    private float RotateCD = 0;
    private float RotateCDTime = 5;

    private void Update()
    {
        RotateCDCounter();
        RotateCDChangeText();
    }

    public void RotateToMousePoint()
    {
        if(RotateCD <= 1)
        {
            RotateCD = RotateCDTime;
            Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                Vector3 targetPosition = hit.point;
                targetPosition.y = transform.position.y;
                transform.LookAt(targetPosition);
                Debug.Log("U rotated.");
            }
        }
        else
        {
            Debug.Log("WTF!");
        }
    }

    private void RotateCDCounter()
    {
        if(RotateCD > 0)
        {
            RotateCD -= Time.deltaTime;
        }
    }

    private void RotateCDChangeText()
    {
        RotateCDText.text = "CD:" + RotateCD.ToString("0.00") + "s";
    }
}
