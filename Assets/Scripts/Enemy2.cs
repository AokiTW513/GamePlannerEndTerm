using UnityEngine;

public class Enemy2 : Enemy
{
    [Header("數值")]
    //敵人血量
    [SerializeField]
    private float enemyHealth = 20f;
    //敵人血量
    [SerializeField]
    private int enemyType = 2;
    //敵人掉落金錢
    [SerializeField]
    private int enemyDropMoney = 20;
    public override void Awake()
    {
        _maxHealth = enemyHealth;
        _type = enemyType;
        _dropMoney = enemyDropMoney;
        base.Awake();
    }
}