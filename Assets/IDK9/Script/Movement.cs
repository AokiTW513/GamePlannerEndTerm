using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]private GameObject OtherCube;
    
    private  MeshRenderer myRenderer;
    private  MeshRenderer otherRenderer;

    private float moveSpeed = 8f;

    private Vector3 startPosMyCube;
    private Vector3 startPosOtherCube;

    private bool isPushingLeft = false;
    private bool isPushingRight = false;
    private bool isPushingUp = false;
    private bool isPushingDown = false;
    private bool isTouching = false;

    private Vector3 direction;

    private void Start()
    {
        myRenderer = GetComponent<MeshRenderer>();
        otherRenderer = OtherCube.GetComponent<MeshRenderer>();

        startPosMyCube = transform.position;
        startPosOtherCube = OtherCube.transform.position;
    }

    private void Update()
    {
        RendererTouching();
        PlayerMovement();
        PushCube();
    }

    private void PushCube()
    {
        if (isTouching)
        {
            if(direction == Vector3.right && !isPushingLeft && !isPushingUp && !isPushingDown)
            {
                OtherCube.transform.position += new Vector3(Time.deltaTime * moveSpeed, 0, 0);
                isPushingRight = true;
            }
            else if(direction == Vector3.left && !isPushingRight && !isPushingUp && !isPushingDown)
            {
                OtherCube.transform.position += new Vector3(Time.deltaTime * -moveSpeed, 0, 0);
                isPushingLeft = true;
            }
            else if(direction == Vector3.up && !isPushingLeft && !isPushingRight && !isPushingDown)
            {
                OtherCube.transform.position += new Vector3(0, Time.deltaTime * moveSpeed, 0);    
                isPushingUp = true;
            }
            else if(direction == Vector3.down && !isPushingLeft && !isPushingRight && !isPushingUp)
            {
                OtherCube.transform.position += new Vector3(0, Time.deltaTime * -moveSpeed, 0);
                isPushingDown = true;
            }
        }
        else
        {
            isPushingLeft = isPushingRight = isPushingUp = isPushingDown = false;
        }
    }

    private void RendererTouching()
    {
        if (myRenderer.bounds.Intersects(otherRenderer.bounds))
        {   
            isTouching = true;
            Debug.Log("Renderer Bounds 重疊了！");
        }
        else
        {
            isTouching = false;
            Debug.Log("Renderer Bounds 沒有重疊");
        }
    }

    //simple movement
    private void PlayerMovement()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(Time.deltaTime * -moveSpeed, 0 , 0);
            direction = Vector3.left;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += new Vector3(Time.deltaTime * moveSpeed, 0, 0);
            direction = Vector3.right;
        }
        else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += new Vector3(0, Time.deltaTime * moveSpeed, 0);
            direction = Vector3.up;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += new Vector3(0, Time.deltaTime * -moveSpeed, 0);
            direction = Vector3.down;
        }
        else if (Input.GetKeyDown(KeyCode.R)) //reset button
        {
            transform.position = startPosMyCube;
            OtherCube.transform.position = startPosOtherCube;
        }
        else
        {
            direction = Vector3.zero;
        }
    }
}