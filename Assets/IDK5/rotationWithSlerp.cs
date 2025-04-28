using UnityEngine;

public class rotationWithSlerp : MonoBehaviour
{
    public GameObject goal;
    private float carRotSpeed = 1.5f;
    private float carMoveSpeed = 3f;
    private float goalMoveSpeed = 4.5f;

    void Update()
    {
        var direction = goal.transform.position - transform.position;
        transform.rotation = Quaternion.Slerp
            (transform.rotation, Quaternion.LookRotation(direction), carRotSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, goal.transform.position) > 3)
            transform.Translate(0, 0, carMoveSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.UpArrow))
            goal.transform.position += Vector3.forward * goalMoveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.DownArrow))
            goal.transform.position -= Vector3.forward * goalMoveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftArrow))
            goal.transform.position -= Vector3.right * goalMoveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.RightArrow))
            goal.transform.position += Vector3.right * goalMoveSpeed * Time.deltaTime;
    }
}