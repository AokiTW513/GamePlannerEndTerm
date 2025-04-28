using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goalMove : MonoBehaviour
{
    public float speed = 3;
    public float dropSpeed = 8;
    public GameObject baseLine, L, M, R;
    int dropFlag = 0;
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.D))
        {
            L.transform.SetParent(null);
            M.transform.SetParent(null);
            R.transform.SetParent(null);
            baseLine.SetActive(true);
            dropFlag = 1;
        }
        if (dropFlag == 1)
        {
            L.transform.position += Vector3.down * dropSpeed * Time.deltaTime;
            M.transform.position += Vector3.down * dropSpeed * Time.deltaTime;
            R.transform.position += Vector3.down * dropSpeed * Time.deltaTime;
        }
    }
}
