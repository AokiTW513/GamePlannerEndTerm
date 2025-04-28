using UnityEngine;

public class bulletMove : MonoBehaviour
{
    void Update()
    {
        transform.position += transform.right * 20 * Time.deltaTime;
        Destroy(gameObject, 2);
    }
}