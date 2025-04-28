using UnityEngine;

public class playerMove : MonoBehaviour
{
    public float speed = 8;
    public float angle = 90;

    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            transform.position += transform.forward * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.DownArrow))
            transform.position -= transform.forward * speed / 2 * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftArrow))
            transform.Rotate(0, -angle * Time.deltaTime, 0);
        if (Input.GetKey(KeyCode.RightArrow))
            transform.Rotate(0, angle * Time.deltaTime, 0);
    }
}