using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private TopDownMouseRotate TDMR;
    
    private void Start()
    {
        TDMR = gameObject.GetComponent<TopDownMouseRotate>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            TDMR.RotateToMousePoint();
        }
    }
}
