using UnityEngine;

public class Tower1 : Tower
{   
    public override int sellMoney { get; } = 50;
    public override int buyMoney { get; } = 100;
    public override int upgradeMoney { get; } = 80;

    public override void Awake()
    {
        SetState(20f, 1f, 150f, 1);
        base.Awake();
    }

    public override void SwitchTowerState()
    {
        /*
        1=正常
        2=努力
        3=瘋狂努力
        4=摸魚
        5=睡覺
        */
        if (towerState == 1)
        {
            SetState(20f);
        }
        else if (towerState == 2)
        {
            SetState(30f);
        }
        else if (towerState == 3)
        {
            SetState(40f);
        }
        else if (towerState == 4)
        {
            SetState(10f);
        }
    }
}