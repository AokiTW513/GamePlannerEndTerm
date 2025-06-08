using UnityEngine;

public class Tower2 : Tower
{
    public override int sellMoney { get; } = 60;
    public override int buyMoney { get; } = 120;
    public override int upgradeMoney { get; } = 96;

    public override void Awake()
    {
        SetState(10f, 1f, 150f, 2);
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
            SetState(10f);
        }
        else if (towerState == 2)
        {
            SetState(15f);
        }
        else if (towerState == 3)
        {
            SetState(20f);
        }
        else if (towerState == 4)
        {
            SetState(5f);
        }
    }
}