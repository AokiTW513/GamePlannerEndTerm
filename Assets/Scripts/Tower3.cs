using UnityEngine;

public class Tower3 : Tower
{
    //賣出 
    public override int sellMoney { get; } = 75;
    //買
    public override int buyMoney { get; } = 150;
    //升級
    public override int upgradeMoney { get; } = 120;

    public override void Awake()
    {
        //第一個攻擊力
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