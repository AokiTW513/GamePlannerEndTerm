using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricLine : MonoBehaviour
{
    public float moveSpeed = 5;
    //privates
    private LineRenderer lineRen;
    private Vector3[] baseLine;

    [Header("Points")]
    public Transform startPoint;
    public Transform endPoint;

    [Header("Settings")]
    [Tooltip("distance between each point on the line")]
    public float interval = 1f;
    [Tooltip("how extreme the displacment of each point is")]
    public float frequency = 0.5f;
    [Tooltip("how quickly it changes")]
    public float speed = 0.05f;

    private void Awake()
    {
        //Setup Line Renderer
        if (this.gameObject.GetComponent<LineRenderer>() != null)
        {
            lineRen = this.gameObject.GetComponent<LineRenderer>();
        }
        else
        {
            this.gameObject.AddComponent<LineRenderer>();
            lineRen = this.gameObject.GetComponent<LineRenderer>();
        }
    }



    void Start()
    {
        setupLine(startPoint.transform.localPosition, endPoint.transform.localPosition);
        Invoke("animateLine", speed);
    }


    void Update()
    {
        //Set start and end every frame ||| only do this if you need the start and end to move smoothly
        lineRen.SetPosition(0, baseLine[0]);
        lineRen.SetPosition(baseLine.Length - 1, baseLine[baseLine.Length - 1]);
        if (Input.GetKey(KeyCode.W))
            endPoint.position += Vector3.forward * moveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S))
            endPoint.position -= Vector3.forward * moveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D))
            endPoint.position += Vector3.right * moveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
            endPoint.position -= Vector3.right * moveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.UpArrow))
            startPoint.position += Vector3.forward * moveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.DownArrow))
            startPoint.position -= Vector3.forward * moveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.RightArrow))
            startPoint.position += Vector3.right * moveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftArrow))
            startPoint.position -= Vector3.right * moveSpeed * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
            startPoint.GetComponent<Rigidbody>().AddForce(0, 300, 0);
        if (Input.GetKeyDown(KeyCode.F))
            endPoint.GetComponent<Rigidbody>().AddForce(0, 300, 0);
        /*if(Vector3.Distance(startPoint.position,endPoint.position) > 10)
        {
            Vector3 dir = startPoint.position - endPoint.position;
            dir.Normalize();
            endPoint.position += dir * moveSpeed * Time.deltaTime;
        }*/

    }



    public void setupLine(Vector3 sPoint, Vector3 ePoint)
    {
        //Calculate distance and number of points based on interval

        float fullDistance = Vector3.Distance(ePoint, sPoint);
        float pointCount = (int)(fullDistance / interval);


        //plus one to add endpoint;
        lineRen.positionCount = (int)pointCount + 1;


        //Create Temporary Position Arrray
        Vector3[] tempPos = new Vector3[lineRen.positionCount];


        tempPos[0] = sPoint; //set start point
        tempPos[tempPos.Length - 1] = ePoint; // set end point


        for (int i = 1; i < tempPos.Length - 1; i++) // set middle points *each an equal distance apart*
        {
            tempPos[i] = new Vector3(sPoint.x + i / pointCount * (ePoint.x - sPoint.x), sPoint.y + i / pointCount * (ePoint.y - sPoint.y), sPoint.z + i / pointCount * (ePoint.z - sPoint.z));
        }


        //Set base line
        baseLine = tempPos;
    }

    public void animateLine()
    {
        setupLine(startPoint.transform.localPosition, endPoint.transform.localPosition);
        Vector3[] tempPos = baseLine;

        for (int i = 1; i < baseLine.Length - 1; i++)
        {
            tempPos[i] = tempPos[i] + new Vector3(Random.Range(-frequency, frequency), Random.Range(-frequency, frequency), Random.Range(-frequency, frequency));
        }

        lineRen.SetPositions(tempPos);

        //Repeat
        Invoke("animateLine", speed);
    }
}
