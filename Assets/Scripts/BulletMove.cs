using UnityEngine;

public class BulletMove : MonoBehaviour
{
    private float _speed = 8f;
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

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}