using UnityEngine;

public class tankAI : MonoBehaviour
{
    public GameObject player;
    public GameObject bullet;
    public GameObject muzzle;
    Animator am;

    void Start()
    {
        am = GetComponent<Animator>();
    }
    void Update()
    {
        am.SetFloat("distance", Vector3.Distance(transform.position,
        player.transform.position));
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public void StartFiring()
    {
        InvokeRepeating("Fire", 0, 0.5f);
    }

    public void StopFiring()
    {
        CancelInvoke("Fire");
    }
    
    void Fire()
    {
        GameObject b = Instantiate(bullet, muzzle.transform.position,
        muzzle.transform.rotation);
        b.transform.eulerAngles += new Vector3(0, -90, 0);
    }
}