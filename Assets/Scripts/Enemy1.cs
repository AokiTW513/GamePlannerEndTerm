using UnityEngine;
using UnityEngine.UI;

public class Enemy1 : Enemy
{
    public override void Awake()
    {
        _maxHealth = 40f;
        _type = 1;
        _dropMoney = 10;
        base.Awake();
    }
}