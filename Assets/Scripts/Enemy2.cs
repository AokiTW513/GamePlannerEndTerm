using UnityEngine;
using UnityEngine.UI;

public class Enemy2 : Enemy
{
    public override void Awake()
    {
        _health = 20f;
        _type = 2;
        base.Awake();
    }
}