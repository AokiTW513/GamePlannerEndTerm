using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    private int Money = 500;
    public int _money => Money;

    public Text moneyText;

    private void Start()
    {
        ChangeMoneyText();
    }

    public void SetMoney(int value)
    {
        Money = value;
        ChangeMoneyText();
    }

    public void AddMoney(int value)
    {
        Money += value;
        ChangeMoneyText();
    }

    private void ChangeMoneyText()
    {
        moneyText.text = "Money:" + Money + '$';
    }
}