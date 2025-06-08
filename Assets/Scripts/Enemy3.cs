using UnityEngine;
using UnityEngine.UI;

public class Enemy3 : Enemy
{
    public override void Awake()
    {
        _maxHealth = 30f;
        _type = 3;
        _dropMoney = 5;
        base.Awake();
    }
}