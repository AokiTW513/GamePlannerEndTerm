using JetBrains.Annotations;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    [Header("數值")]
    [SerializeField]
    private float _speed = 8f;
    [SerializeField]
    private float _damage = 10;
    [SerializeField]
    private int _type = 0;

    private void Update()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    public void SetBullet(GameObject enemy, float damage, int type)
    {
        transform.LookAt(enemy.transform.position);
        _damage = damage;
        _type = type;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.GetComponent<Enemy>() != null)
        {
            if (_type != 2 && other.gameObject.GetComponent<Enemy>().type == 2)
            {
                return;
            }
            float _originDamage = _damage;
            if (other.gameObject.GetComponent<Enemy>().type == 3 && _type != 3)
            {
                _damage = _damage / 3;
                _damage = Mathf.Round(_damage);
            }
            other.gameObject.GetComponent<Enemy>().TakeDamage(_damage);
            _damage = _originDamage;
            Destroy(gameObject);
        }
    }
}