using UnityEngine;

public class Ball : MonoBehaviour
{
    #region 自身组件
    
    

    #endregion

    #region 业务变量

    public float strength = 5f;

    private bool drag;

    private GameObject selectedBall;

    private float leftEdge;


    #endregion

    void Awake() {
        
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 1f;

    }

    public void Update() {
        HandleInput();
    }

    public void HandleInput() {
        if (Input.GetMouseButtonDown(0)) {
            drag = true;
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Ball")) {
                Debug.Log($"选中" + hit.collider.gameObject.name);
                selectedBall = hit.collider.gameObject;

                selectedBall.GetComponent<SpringJoint2D>().enabled = true;
                selectedBall.GetComponent<LineRenderer>().enabled = true;
                

            }
        }

        if (Input.GetMouseButtonUp(0)) {
            drag = false;
            selectedBall.GetComponent<SpringJoint2D>().enabled = false;
            selectedBall.GetComponent<LineRenderer>().enabled = false;

            selectedBall = null;
        }

        if (drag && selectedBall != null) {
            Debug.Log($"选中" + selectedBall.name);
            var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // 画线
            Vector2 currentPos;
            currentPos.x = point.x;
            currentPos.y = point.y;
            selectedBall.GetComponent<LineRenderer>().SetPosition(0, selectedBall.transform.position);
            selectedBall.GetComponent<LineRenderer>().SetPosition(1, currentPos);
            // 设置弹簧, 链接Ball和当前位置
            Vector2 connectedAnchor = selectedBall.GetComponent<SpringJoint2D>().connectedAnchor;
            connectedAnchor.x = currentPos.x;
            connectedAnchor.y = currentPos.y;
            selectedBall.GetComponent<SpringJoint2D>().connectedAnchor = connectedAnchor;
            // 施加力
            Vector2 ballPos;
            ballPos.x = selectedBall.transform.position.x;
            ballPos.y = selectedBall.transform.position.y;
            var rigidbody2D = selectedBall.GetComponent<Rigidbody2D>();
            rigidbody2D.AddForce((currentPos - ballPos) * rigidbody2D.mass * strength, ForceMode2D.Force);
        }
    }
}
