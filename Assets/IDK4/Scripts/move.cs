using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    public float speed = 10;
    void Update()
    {
        transform.position += -transform.right * speed * Time.deltaTime;
    }
}
