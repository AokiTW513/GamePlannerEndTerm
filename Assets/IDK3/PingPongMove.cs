using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongMove : MonoBehaviour
{
    public float speed = 10;
    public float right = 10, left = -10;
    void Update()
    {
        if (transform.position.x + speed * Time.deltaTime > right)
        {
            transform.position = new Vector3(right, transform.position.y, transform.position.z);
            speed *= -1;
        }
        else if (transform.position.x + speed * Time.deltaTime < left)
        {
            transform.position = new Vector3(left, transform.position.y, transform.position.z);
            speed *= -1;
        }
        else
            transform.position += Vector3.right * speed * Time.deltaTime;
    }

}