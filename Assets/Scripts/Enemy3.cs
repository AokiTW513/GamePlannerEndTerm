using UnityEngine;

public class Enemy3 : Enemy
{
    [Header("數值")]
    //敵人血量
    [SerializeField]
    private float enemyHealth = 30f;
    //敵人血量
    [SerializeField]
    private int enemyType = 3;
    //敵人掉落金錢
    [SerializeField]
    private int enemyDropMoney = 5;
    public override void Awake()
    {
        _maxHealth = enemyHealth;
        _type = enemyType;
        _dropMoney = enemyDropMoney;
        base.Awake();
    }
}