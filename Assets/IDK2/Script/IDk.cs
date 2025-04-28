using UnityEngine;

public class IDk : MonoBehaviour
{
    private float top = 50;
    private float bottom = -50;
    private float right = 80;
    private float left = -80;
    private float vx = 0.4f, vy = 0.3f;

    bool holdFlag = false;
    public Camera cam;
    float screenPosZ;

    Vector3 previousPos;

    private void Update()
    {
        previousPos = transform.position;

        if (holdFlag == true)
        {
            vx = vy = 0;
            Vector3 mouseScreenPos = Input.mousePosition;
            mouseScreenPos.z = screenPosZ;
            transform.position = cam.ScreenToWorldPoint(mouseScreenPos);
        }

        vy -= 0.01f;
        if(transform.position.x + 4 + vx > right)
        {
            transform.position = new Vector3(right - 4, transform.position.y, transform.position.z);
            vx *= -0.8f;
        }
        else if (transform.position.x - 4 + vx < left)
        {
            transform.position = new Vector3(left + 4, transform.position.y, transform.position.z);
            vx *= -0.8f;
        }
        else
        {
            transform.position += new Vector3(vx, 0, 0);
        }

        if (transform.position.y + 4 + vy > top)
        {
            transform.position = new Vector3(transform.position.x, top - 4, transform.position.z);
            vy *= -0.8f;
        }
        else if(transform.position.y - 4 + vy < bottom)
        {
            transform.position = new Vector3(transform.position.x, bottom + 4, transform.position.z);
            vy *= -0.8f;
            vx *= 0.995f;
        }
        else
        {
            transform.position += new Vector3(0, vy, 0);
        }
    }

    private void OnMouseDown()
    {
        holdFlag = true;
        screenPosZ = cam.WorldToScreenPoint(transform.position).z;
    }

    private void OnMouseUp()
    {
        holdFlag = false;
        vx = (transform.position - previousPos).x;
        vy = (transform.position - previousPos).y;
    }
}