using UnityEngine;

public class Enemy1 : Enemy
{
    [Header("數值")]
    //敵人血量
    [SerializeField]
    private float enemyHealth = 40f;
    //敵人類型
    [SerializeField]
    private int enemyType = 1;
    //敵人掉落金錢
    [SerializeField]
    private int enemyDropMoney = 10;

    public override void Awake()
    {
        _maxHealth = enemyHealth;
        _type = enemyType;
        _dropMoney = enemyDropMoney;
        base.Awake();
    }
}