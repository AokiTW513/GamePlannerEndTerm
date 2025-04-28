using UnityEngine;

public class doorControl : MonoBehaviour
{
    private float openDistance = 4.1f;
    public GameObject door;
    public Transform leftmostPos, rightmostPos;
    private float doorSpeed = 1;
    private bool isOpening = false;
    private bool isClosing = false;
    private bool atLeftmost = true;
    private bool atRightmost = false;

    void Update()
    {
        if (Vector3.Distance(transform.position, leftmostPos.position) < openDistance && !atRightmost)
        {
            isOpening = true;
            atLeftmost = isClosing = false;
        }
        else if(Vector3.Distance(transform.position, leftmostPos.position) >= openDistance && !atLeftmost)
        {
            isClosing = true;
            atRightmost = isOpening = false;
        }

        if (isOpening)
        {
            if(door.transform.position.x < rightmostPos.position.x)
            {
                door.transform.position += Vector3.right * doorSpeed * Time.deltaTime;
            }
            else
            {
                door.transform.position = rightmostPos.position;
                isOpening = false;
                atRightmost = true;
            }
        }

        if (isClosing)
        {
            if(door.transform.position.x > leftmostPos.position.x)
            {
                door.transform.position -= Vector3.right * doorSpeed * Time.deltaTime;
            }
            else
            {
                door.transform.position = leftmostPos.position;
                isClosing = false;
                atLeftmost = true;
            }
        }
    }
}