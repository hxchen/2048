using System.Collections;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    [Header("UI Component ")]
    public int FPS;
    public GameObject ballsBoard;
    public GameObject ballPrefab;

    public static BallManager instancs;

    private float top;

    private bool drag;

    private GameObject selectedBall;

    public float strength = 30f;

    public int initNum = 2;

    public int target = 2048;

    // 是否需要补充新球，需要的话，一秒钟后补充
    private bool needNewBall;

    private float waitTime;

    private GameState gameState;

    public void Awake() {
        instancs = this;
        Application.targetFrameRate = FPS;
        float halfSreenHeight = Camera.main.orthographicSize;
        top = halfSreenHeight - 1;

        needNewBall = false;
        waitTime = 0;
    }

    public void Update() {
        if (GameState.Play == gameState) {
            HandleInput();
            if (needNewBall) {
                waitTime += Time.deltaTime;
                if (waitTime > 1.5f) {
                    Spawn();
                }
            }
        }
    }

    /// <summary>
    /// 处理事件
    /// </summary>
    public void HandleInput() {
        if (Input.GetMouseButtonDown(0)) {
            drag = true;
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Ball")) {
                //Debug.Log($"选中 {hit.collider.gameObject.name}({GetInstanceID()})");
                selectedBall = hit.collider.gameObject;

                selectedBall.GetComponent<SpringJoint2D>().enabled = true;
                selectedBall.GetComponent<LineRenderer>().enabled = true;


            }
        }

        if (Input.GetMouseButtonUp(0)) {
            drag = false;
            if (selectedBall != null) {
                //Debug.Log($"取消选中 {selectedBall.gameObject.name}({GetInstanceID()})");
                selectedBall.GetComponent<SpringJoint2D>().enabled = false;
                selectedBall.GetComponent<LineRenderer>().enabled = false;
                selectedBall = null;
            }
            //Debug.Log($"selected ball:{selectedBall}, 鼠标弹起");
        }

        if (drag && selectedBall != null) {
            //Debug.Log($"选中并拖拽" + selectedBall.name);
            var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // 画线
            Vector2 currentPos;
            currentPos.x = point.x;
            currentPos.y = point.y;
            selectedBall.GetComponent<LineRenderer>().SetPosition(0, selectedBall.transform.position);
            selectedBall.GetComponent<LineRenderer>().SetPosition(1, currentPos);
            //Debug.Log($"更新划线({selectedBall.transform.position},{currentPos})");
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

    /// <summary>
    /// 生成2个球
    /// </summary>
    private void Spawn() {
        int index = Random.Range(0, 1);
        if (index == 0) {
            var ball1 = NewBall(new Vector3(-1, top, 0), initNum);
            var ball2 = NewBall(new Vector3(1, top, 0), initNum);
        } else {
            NewBall(new Vector3(0, top, 0), initNum * 2);
        }
        
        //重设状态
        needNewBall = false;
        waitTime = 0;

        //Debug.Log($"掉落新球, ball1{ball1.GetInstanceID()}:{ball1.transform.position}, ball2{ball2.GetInstanceID()}:{ball2.transform.position}");
       
    }
    /// <summary>
    /// 扔掉一个球后，掉一个补充球
    /// </summary>
    /// <returns></returns>
    public void ReplenishBall() {
        NewBall(new Vector3(0, top, 0), initNum);
    }
    /// <summary>
    /// 在指定位置生成一个球
    /// </summary>
    /// <param name="pos"></param>
    private GameObject NewBall(Vector3 pos, int number) {
        GameObject ball = Instantiate(ballPrefab, ballsBoard.transform.position, Quaternion.identity);
        ball.transform.SetParent(ballsBoard.transform);
        ball.transform.position = pos;
        ball.GetComponent<Ball>().SetNumber(number);
        ball.name = "ball_" + ball.GetComponent<Ball>().GetNumber();
        return ball;
    }
    /// <summary>
    /// 合并球
    /// </summary>
    /// <param name="ball"></param>
    /// <param name="other"></param>
    public void MergeBalls(GameObject ball, GameObject other) {
        
        int newNumber = ball.GetComponent<Ball>().GetNumber() * 2;
        Vector3 newPos = (ball.transform.position + other.transform.position) / 2;
        // 重置状态
        if (selectedBall == ball || selectedBall == other) {
            selectedBall = null;
        }
        // 销毁旧球
        Destroy(ball);
        Destroy(other);
        // 播放声音
        if (SoundManager.instance) {
            SoundManager.instance.PlaySound();
        }
        //生成新球
        GameObject newBall = Instantiate(ballPrefab, newPos, Quaternion.identity);
        newBall.transform.SetParent(ballsBoard.transform);
        newBall.GetComponent<Ball>().SetNumber(newNumber);
        newBall.name = "ball_" + newBall.GetComponent<Ball>().GetNumber();
        //Debug.Log($"合并出新球, number:{newNumber}, position:{newBall.transform.position}");
        // 生成新球
        NeedNewBall(true);
        //Spawn();
        //更新分数
        BallMain.instance.AddScore(newNumber);
        // 判断胜利条件
        if (newNumber >= target) {
            gameState = GameState.Hang;
            GameObject.Find("GameUICanvas/BallMain").GetComponent<BallMain>().GameWin();
        }
    }
    /// <summary>
    /// 重设状态
    /// </summary>
    public void ResetState() {
        //selectedBall.GetComponent<SpringJoint2D>().enabled = false;
        //selectedBall.GetComponent<LineRenderer>().enabled = false;
        selectedBall = null;
    }

    public void SetGameState(GameState state) {
        gameState = state;
    }
    /// <summary>
    /// 设置需要产生新球
    /// </summary>
    public void NeedNewBall(bool need) {
        needNewBall = need;
    }

    /// <summary>
    /// 开始
    /// </summary>
    public void StartGame() {
        gameState = GameState.Play;
        Spawn();
    }

    
}
