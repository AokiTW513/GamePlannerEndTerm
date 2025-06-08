using UnityEngine;
using UnityEngine.UI;

public class Enemy2 : Enemy
{
    public override void Awake()
    {
        _maxHealth = 20f;
        _type = 2;
        _dropMoney = 20;
        base.Awake();
    }
}