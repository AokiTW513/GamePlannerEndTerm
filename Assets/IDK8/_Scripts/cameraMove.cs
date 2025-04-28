using UnityEngine;

public class cameraMove : MonoBehaviour
{
    public float moveSpeed = 3;
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S))
            transform.position -= transform.forward * moveSpeed * Time.deltaTime;
    }
}