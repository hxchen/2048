using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameBoard : MonoBehaviour {

    #region UI
    public Text scoreText;

    public int score;

    public Text bestScoreText;

    public Button lastButton;

    public Button newButton;

    public Button exitButton;

    public Transform grid;

    public GameObject tilePrefab;

    public GameObject numberPrefab;

    public AlertPanel alertPanel;


    #endregion


#region 业务逻辑
    public Dictionary<int, int> gridConfig = new Dictionary<int, int>() { { 4, 230 }, { 5, 180 }, { 6, 150 }, { 7, 130 }, { 8, 110 } };

    private int row;

    private int column;

    public Tile[][] tiles = null;

    public List<Tile> emptytiles = new List<Tile>();

    private Vector3 pointerDownPos, pointerUpPos;

    private bool isNeedNumber;

    private StepModel lastStepModel;
#endregion




    public enum MoveDirection {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    void Awake() {

        InitUIData();

        InitTile();

        CreateNumber();
        CreateNumber();

        // Unity 广告加载
        InterstitialAd.instance.LoadAd();

    }


    #region UI
    /// <summary>
    /// 初始化界面数据
    /// </summary>
    public void InitUIData() {
        bestScoreText.text = PlayerPrefs.GetInt(Const.BestScore, 0).ToString();
        lastStepModel = new StepModel();
        lastButton.interactable = false;
    }

    /// <summary>
    /// 上一步事件
    /// </summary>
    public void OnLastButtonPressed() {
        BackToLastStep();
        lastButton.interactable = false;
    }

    /// <summary>
    /// 重新开始按钮事件
    /// </summary>
    public void OnNewButtonPressed() {
        // 展示广告
        InterstitialAd.instance.ShowAd();
        RestartGame();
    }

    /// <summary>
    /// 退出事件
    /// </summary>
    public void OnExitButtonPressed() {
        SceneManager.LoadSceneAsync(0);
    }

    /// <summary>
    /// 显示胜利UI
    /// </summary>
    public void GameWin() {
        alertPanel.setAlertText("You Win!");
        alertPanel.Show();
    }

    /// <summary>
    /// 显示失败UI
    /// </summary>
    public void GameLose() {
        alertPanel.setAlertText("Game Over!");
        alertPanel.Show();
    }

    /// <summary>
    /// 添加分数
    /// </summary>
    /// <param name="score"></param>
    public void AddScore(int value) {
        score += value;
        UpdateScoreUI(score);
        int bestScore = PlayerPrefs.GetInt(Const.BestScore, 0);
        if (score > bestScore) {
            bestScore = score;
            PlayerPrefs.SetInt(Const.BestScore, bestScore);
            UpdateBestScoreUI(bestScore);
        }
        
    }

    /// <summary>
    /// 更新分数
    /// </summary>
    /// <param name="score"></param>
    public void UpdateScoreUI(int score) {
        scoreText.text = score.ToString();
    }
    /// <summary>
    /// 重新开始分数
    /// </summary>
    public void RestartScore() {
        score = 0;
        UpdateScoreUI(score);
    }

    /// <summary>
    /// 设置最高分
    /// </summary>
    /// <param name="score"></param>
    public void UpdateBestScoreUI(int bestScore) {
        bestScoreText.text = bestScore.ToString();
    }


    /// <summary>
    /// 重新开始
    /// </summary>
    public void RestartGame() {

        lastButton.interactable = false;

        RestartScore();

        for (int i = 0; i < row; i++) {
            for (int j = 0; j < column; j++) {
                if (tiles[i][j].HaveNumber()) {
                    GameObject.Destroy(tiles[i][j].GetNumber().gameObject);
                    tiles[i][j].SetNumber(null);
                }
            }
        }

        CreateNumber();
    }

    /// <summary>
    /// 返回到上一步
    /// </summary>
    public void BackToLastStep() {
        score = lastStepModel.score;
        UpdateScoreUI(score);

        PlayerPrefs.SetInt(Const.BestScore, lastStepModel.bestScore);
        UpdateBestScoreUI(lastStepModel.bestScore);

        for (int i = 0; i < row; i++) {
            for (int j = 0; j < column; j++) {
                if (lastStepModel.numbers[i][j] == 0) {
                    if (tiles[i][j].HaveNumber()) {
                        GameObject.Destroy(tiles[i][j].number.gameObject);
                        tiles[i][j].SetNumber(null);
                    }
                } else {
                    if (tiles[i][j].HaveNumber()) {
                        tiles[i][j].GetNumber().SetNumberValue(lastStepModel.numbers[i][j]);
                    } else {
                        CreateNumber(tiles[i][j], lastStepModel.numbers[i][j]);
                    }
                }
            }
        }
    }
#endregion

#region 核心逻辑

    /// <summary>
    /// 初始化格子
    /// </summary>
    public void InitTile() {
        int number = PlayerPrefs.GetInt(Const.GameModel, 4);
        GridLayoutGroup gridLayoutGroup = grid.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.constraintCount = number;
        gridLayoutGroup.cellSize = new Vector2(gridConfig[number], gridConfig[number]);

        tiles = new Tile[number][];

        row = number;
        column = number;
        for (int i = 0; i < row; i++) {
            for (int j = 0; j < column; j++) {
                if (tiles[i] == null) {
                    tiles[i] = new Tile[number];
                }
                tiles[i][j] = CreateTile();
                tiles[i][j].name = "tile" + i + j;
            }
        }
    }
    /// <summary>
    /// 根据预制体创建Tile
    /// </summary>
    /// <returns></returns>
    public Tile CreateTile() {
        GameObject gameObject = GameObject.Instantiate(tilePrefab, grid);
        return gameObject.GetComponent<Tile>();

    }
    /// <summary>
    /// 随机创建数字
    /// </summary>
    public void CreateNumber() {
        //找到这个数字所在的格子
        emptytiles.Clear();
        for (int i = 0; i < row; i++) {
            for (int j = 0; j < column; j++) {
                //判断这个格子是否有数字
                if (!tiles[i][j].HaveNumber()) {
                    //添加空格子
                    emptytiles.Add(tiles[i][j]);
                }
            }
        }
        //如果空格子数量为0，那么就不能创建数字
        if (emptytiles.Count == 0)
            return;
        //随机一个格子
        int index = Random.Range(0, emptytiles.Count);
        //创建数字并把数字放到格子里
        GameObject gameObject = Instantiate(numberPrefab, emptytiles[index].transform);
        //对数字进行初始化
        gameObject.GetComponent<Number>().Init(emptytiles[index]);

    }

    /// <summary>
    /// 创建数字
    /// </summary>
    /// <param name="tile"></param>
    /// <param name="number"></param>
    public void CreateNumber(Tile tile, int number) {
        GameObject gameObject = Instantiate(numberPrefab, tile.transform);
        gameObject.GetComponent<Number>().Init(tile);
        gameObject.GetComponent<Number>().SetNumberValue(number);
    }

    /// <summary>
    /// 鼠标按下
    /// </summary>
    public void OnPointDownEvent() {
        pointerDownPos = Input.mousePosition;
    }

    /// <summary>
    /// 鼠标抬起
    /// </summary>
    public void OnPointUpEvent() {
        pointerUpPos = Input.mousePosition;
        if (Vector3.Distance(pointerUpPos, pointerDownPos) < 100) {
            return;
        }
        //保存数据
        lastStepModel.UpdateData(this.score, PlayerPrefs.GetInt(Const.BestScore, 0), tiles);
        lastButton.interactable = true;
        // 处理移动
        Move(GetMoveDirection());
    }

    /// <summary>
    /// 键盘上下左右控制
    /// </summary>
    public void OnKeyBoardEvent(){
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            Move(MoveDirection.UP);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            Move(MoveDirection.DOWN);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            Move(MoveDirection.LEFT);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            Move(MoveDirection.RIGHT);
        }
    }

    /// <summary>
    /// 处理移动
    /// </summary>
    /// <param name="direction"></param>
    public void Move(MoveDirection direction) {
        MoveNumber(direction);
        //产生一个新的数字
        if (isNeedNumber) {
            CreateNumber();
            isNeedNumber = false;
        }

        //把所有数字恢复正常状态
        ResetNumberStatus();

        //判断游戏是否结束？
        if (IsGameOver()) {
            GameLose();
        }
    }

    /// <summary>
    /// 获取移动方向
    /// </summary>
    /// <returns></returns>
    public MoveDirection GetMoveDirection() {
        if (Mathf.Abs(pointerUpPos.x - pointerDownPos.x) > Mathf.Abs(pointerUpPos.y - pointerDownPos.y)) {
            if (pointerUpPos.x - pointerDownPos.x > 0) {
                return MoveDirection.RIGHT;
            } else {
                return MoveDirection.LEFT;
            }
        } else {
            if (pointerUpPos.y - pointerDownPos.y > 0) {
                return MoveDirection.UP;
            } else {
                return MoveDirection.DOWN;
            }
        }
    }

    /// <summary>
    /// 移动数字
    /// </summary>
    /// <param name="direction"></param>
    public void MoveNumber(MoveDirection direction) {
        switch (direction) {
            case MoveDirection.UP:
                for (int j = 0; j < column; j++) {
                    for (int i = 1; i < row; i++) {
                        if (tiles[i][j].HaveNumber()) {
                            Number number = tiles[i][j].GetNumber();

                            for (int m = i - 1; m >= 0; m--) {
                                Number targetNumber = null;
                                if (tiles[m][j].HaveNumber()) {
                                    targetNumber = tiles[m][j].GetNumber();
                                }
                                HandleNumber(number, targetNumber, tiles[m][j]);
                                if (targetNumber != null) {
                                    break;
                                }
                            }
                        }
                    }
                }
                break;
            case MoveDirection.DOWN:
                for (int j = 0; j < column; j++) {
                    for (int i = row - 2; i >= 0; i--) {
                        if (tiles[i][j].HaveNumber()) {
                            Number number = tiles[i][j].GetNumber();
                            for (int m = i + 1; m < row; m++) {
                                Number targetNumber = null;
                                if (tiles[m][j].HaveNumber()) {
                                    targetNumber = tiles[m][j].GetNumber();
                                }
                                HandleNumber(number, targetNumber, tiles[m][j]);
                                if (targetNumber != null) {
                                    break;
                                }
                            }
                        }
                    }
                }
                break;
            case MoveDirection.LEFT:
                for (int i = 0; i < row; i++) {
                    for (int j = 1; j < column; j++) {
                        if (tiles[i][j].HaveNumber()) {
                            Number number = tiles[i][j].GetNumber();
                            for (int m = j - 1; m >= 0; m--) {
                                Number targetNumber = null;
                                if (tiles[i][m].HaveNumber()) {
                                    targetNumber = tiles[i][m].GetNumber();
                                }
                                HandleNumber(number, targetNumber, tiles[i][m]);
                                if (targetNumber != null) {
                                    break;
                                }
                            }
                        }
                    }
                }
                break;
            case MoveDirection.RIGHT:
                for (int i = 0; i < row; i++) {
                    for (int j = column - 2; j >= 0; j--) {
                        if (tiles[i][j].HaveNumber()) {
                            Number number = tiles[i][j].GetNumber();
                            for (int m = j + 1; m < column; m++) {
                                Number targetNumber = null;
                                if (tiles[i][m].HaveNumber()) {
                                    targetNumber = tiles[i][m].GetNumber();
                                }
                                HandleNumber(number, targetNumber, tiles[i][m]);
                                if (targetNumber != null) {
                                    break;
                                }
                            }
                        }
                    }
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 处理数字
    /// </summary>
    /// <param name="current"></param>
    /// <param name="target"></param>
    /// <param name="targetTile"></param>
    public void HandleNumber(Number current, Number target, Tile targetTile) {
        if (target != null) {
            //判断能不能合并
            if (current.IsMerge(target)) {

                isNeedNumber = true;
                //销毁当前的数字 
                current.GetTile().SetNumber(null);
                //GameObject.Destroy(current.gameObject);
                current.DestroyOnMoveEnd(target.GetTile());
                target.Merge();
            }
        } else {
            current.MoveToTile(targetTile);
            isNeedNumber = true;
        }
    }

    /// <summary>
    /// 恢复数字
    /// </summary>
    public void ResetNumberStatus() {
        for (int i = 0; i < row; i++) {
            for (int j = 0; j < column; j++) {
                if (tiles[i][j].HaveNumber()) {
                    tiles[i][j].GetNumber().status = NumberState.Normal;
                }
            }
        }
    }


    /// <summary>
    /// 检查游戏是否失败
    /// </summary>
    /// <returns></returns>
    public bool IsGameOver() {

        //判断格子是否满了
        for (int i = 0; i < row; i++) {
            for (int j = 0; j < column; j++) {
                if (!tiles[i][j].HaveNumber()) {
                    return false;
                }
            }
        }
        //判断有没有数字能合并
        for (int i = 0; i < row; i++) {
            for (int j = 0; j < column; j++) {
                Tile up = IsHaveTile(i - 1, j) ? tiles[i - 1][j] : null;
                Tile down = IsHaveTile(i + 1, j) ? tiles[i + 1][j] : null;
                Tile left = IsHaveTile(i, j - 1) ? tiles[i][j - 1] : null;
                Tile right = IsHaveTile(i, j + 1) ? tiles[i][j + 1] : null;

                if (up != null) {
                    if (tiles[i][j].GetNumber().IsMerge(up.GetNumber()))
                        return false;
                }
                if (down != null) {
                    if (tiles[i][j].GetNumber().IsMerge(down.GetNumber()))
                        return false;
                }
                if (left != null) {
                    if (tiles[i][j].GetNumber().IsMerge(left.GetNumber()))
                        return false;
                }
                if (right != null) {
                    if (tiles[i][j].GetNumber().IsMerge(right.GetNumber()))
                        return false;
                }
            }
        }
        return true;
    }

    private bool IsHaveTile(int i, int j) {
        if (i >= 0 && i < row && j >= 0 && j < column) {
            return true;
        }
        return false;
    }

    #endregion

#region Ads

#endregion
}