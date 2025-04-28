using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missileControl : MonoBehaviour
{
    public GameObject goalAirplane, goalElectric;
    public float bulletRotSpeed = 1;
    public float bulletMoveSpeed = 15;
    Vector3 direction;
    int shootingFlag = 0, targetFlag = 0;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            shootingFlag = 1;
        if (Input.GetKeyDown(KeyCode.D))
            targetFlag = 1;
        if (shootingFlag == 1)
        {
            if (targetFlag == 0)
                direction = goalAirplane.transform.position - transform.position;
            else
                direction = goalElectric.transform.position - transform.position;

            transform.rotation = Quaternion.Slerp
            (transform.rotation, Quaternion.LookRotation(direction),
                                 bulletRotSpeed * Time.deltaTime);
            transform.Translate(0, 0, bulletMoveSpeed * Time.deltaTime);
        }
    }
}
