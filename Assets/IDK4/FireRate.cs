using UnityEngine;

public class FireRate : MonoBehaviour
{
    public GameObject effect, bullet;
    public Transform muzzlePos, bulletPos;
    public float fireRate = 2;
    float nextTimeToFire = 0;

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + (1 / fireRate);
            GameObject obj1 = Instantiate(effect, muzzlePos.position, Quaternion.identity);
            Destroy(obj1, 1);
            GameObject obj2 = Instantiate(bullet, bulletPos.position, Quaternion.identity);
            Destroy(obj2, 3);
        }
    }
}