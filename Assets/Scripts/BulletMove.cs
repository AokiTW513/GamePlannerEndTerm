using UnityEngine;

public class BulletMove : MonoBehaviour
{
    [Header("數值")]
    [SerializeField]
    private float _speed = 8f;
    [SerializeField]
    private float _damage = 10;

    private void Update()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    public void SetBullet(GameObject enemy, float damage)
    {
        transform.LookAt(enemy.transform.position);
        _damage = damage;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.GetComponent<Enemy>() != null)
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}