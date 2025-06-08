using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public int towerType = 0;
    public GameObject currentTower;
    private MeshRenderer towerPlaceMesh;
    private Vector3 _offset = new Vector3(0, 1, 0);

    private void Awake()
    {
        towerPlaceMesh = GetComponent<MeshRenderer>();
    }

    public void BuyTower(int type, GameObject tower)
    {
        towerType = type;
        currentTower = Instantiate(tower, transform.position + _offset, Quaternion.identity);
        currentTower.GetComponent<Tower>().SetRotation(transform.rotation);
        GameManager.Instance.TakeMoney(-tower.GetComponent<Tower>().buyMoney);
        towerPlaceMesh.enabled = false;
    }

    public void SellTower()
    {
        GameManager.Instance.TakeMoney(currentTower.GetComponent<Tower>().sellMoney);
        Destroy(currentTower);
        towerType = 0;
        currentTower = null;
        towerPlaceMesh.enabled = true;
    }
}