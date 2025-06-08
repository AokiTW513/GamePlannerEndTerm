using UnityEngine;

public class Tower3 : Tower
{
    public override int sellMoney { get; } = 75;
    public override int buyMoney { get; } = 150;
    public override int upgradeMoney { get; } = 120;

    public override void Awake()
    {
        SetState(15f, 1f, 150f, 3);
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
            SetState(15f);
        }
        else if (towerState == 2)
        {
            SetState(23f);
        }
        else if (towerState == 3)
        {
            SetState(30f);
        }
        else if (towerState == 4)
        {
            SetState(8f);
        }
    }
}