using UnityEngine;
using UnityEngine.UI;

public class Enemy1 : Enemy
{
    public override void Awake()
    {
        _health = 40f;
        _type = 1;
        base.Awake();
    }
}