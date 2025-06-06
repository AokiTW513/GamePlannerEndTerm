using UnityEngine;
using UnityEngine.UI;

public class Enemy3 : Enemy
{
    public override void Awake()
    {
        _health = 30f;
        _type = 3;
        base.Awake();
    }
}