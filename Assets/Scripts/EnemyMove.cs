
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    //敵人移動速度
    [SerializeField]
    private float moveSpeed = 3f;
    //旋轉速度(他會照著中心點旋轉)
    [SerializeField]
    private float rotateSpeed = 30f; // 每秒旋轉角度
    //轉幾圈(.5=0.5)
    [SerializeField]
    private float circleCount = .5f;
    //圈的半徑多少
    [SerializeField]
    private float circleRadius = 9f;

    private bool clockwise = true; // 順時針 or 逆時針
    private float forwardDistance = 11f;

    private Transform circleCenter;
    private enum State { Forward, Circle, Exit }
    private State _state = State.Forward;

    private Vector3 _startPos;
    private float _circleAngle = 0f;
    private float _totalAngle => 360f * circleCount;

    void Start()
    {
        _startPos = transform.position;
        circleCenter = GameManager.Instance.Player.transform;
        forwardDistance = Mathf.Abs(transform.position.x) - circleRadius;
        int choose = Random.Range(1,3);
        switch (choose)
        {
            case 1:
                clockwise = true;
                break;
            case 2:
                clockwise = false;
                break;
        }
        // 初始面向圓心
        Vector3 dir = (circleCenter.position - transform.position).normalized;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);
    }

    void Update()
    {
        switch (_state)
        {
            case State.Forward:
                MoveForward();
                break;
            case State.Circle:
                RotateAroundCenter();
                break;
            case State.Exit:
                ExitWalk(); // 或切換到其他 waypoint 路線
                break;
        }
    }

    void MoveForward()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        if (_state == State.Forward && Vector3.Distance(_startPos, transform.position) >= forwardDistance)
        {
            if (circleCount > 0 && circleCenter != null)
            {
                // 移動到旋轉起始點
                Vector3 offset = (transform.position - circleCenter.position).normalized * circleRadius;
                transform.position = circleCenter.position + offset;
                _circleAngle = 0;
                _state = State.Circle;
            }
            else
            {
                _state = State.Exit;
            }
        }
    }

    void ExitWalk()
    {
        // 保持移動方向不變（讓他走直線）
        transform.position += Vector3.right * moveSpeed * Time.deltaTime;

        // 但「面向方向」慢慢轉向 Vector3.right
        Quaternion targetRot = Quaternion.LookRotation(Vector3.right);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 2f * Time.deltaTime);
    }

    void RotateAroundCenter()
    {
        if (circleCenter == null) return;

        float step = rotateSpeed * Time.deltaTime;
        float angleStep = clockwise ? -step : step;

        Vector3 oldPos = transform.position;
        transform.RotateAround(circleCenter.position, Vector3.up, angleStep);

        Vector3 moveDir = (transform.position - oldPos).normalized;

        if (moveDir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 5f * Time.deltaTime);
        }

        _circleAngle += Mathf.Abs(step);

        if (_circleAngle >= _totalAngle)
        {
            _state = State.Exit;
        }
    }
}
