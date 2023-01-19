using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameBoard : MonoBehaviour
{
    public Text scoreText;

    public Text bestScoreText;

    public Button lastButton;

    public Button newButton;

    public Button exitButton;

    public Transform grid;

    public Dictionary<int, int> gridConfig = new Dictionary<int, int>() { {4, 230}, {5, 180},{6, 150},{7, 130}, {8, 110} };

    private int row;
    private int column;

    public Tile[][] tiles = null;

    public List<Tile> emptytiles = new List<Tile>();

    public GameObject tilePrefab;
    public GameObject numberPrefab;

    private Vector3 pointerDownPos, pointerUpPos;

    private bool isNeedNumber;

    public enum MoveDirection {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    void Awake() {

        InitTile();

        CreateNumber();
        CreateNumber();
    }


    public void OnLastButtonPressed() {

    }

    public void OnNewButtonPressed() { }

    public void OnExitButtonPressed() {
        SceneManager.LoadSceneAsync(0);
    }
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

    public Tile CreateTile() {
        GameObject gameObject = GameObject.Instantiate(tilePrefab, grid);
        return gameObject.GetComponent<Tile>();

    }
    /// <summary>
    /// 创建数字
    /// </summary>
    public void CreateNumber() {
        //找到这个数字所在的格子
        emptytiles.Clear();
        for (int i = 0; i < row; i++) {
            for (int j = 0; j < column; j++) {
                //判断这个格子是否有数字
                if (!tiles[i][j].isHaveNumber()) {
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


    public void OnPointDownEvent() {
        pointerDownPos = Input.mousePosition;
    }

    public void OnPointUpEvent() {
        pointerUpPos = Input.mousePosition;
        if (Vector3.Distance(pointerUpPos, pointerDownPos) < 100) {
            return;
        }
        //保存数据
        //lastStepModel.UpdateData(this.currentScore, PlayerPrefs.GetInt(Const.BestScore, 0), tiles);
        //btn_LastStep.interactable = true;

        MoveDirection direction = GetMoveDirection();
        MoveNumber(direction);
        //产生一个新的数字
        if (isNeedNumber) {
            CreateNumber();
            isNeedNumber = false;
        }

        //把所有数字恢复正常状态
        ResetNumberStatus();

        //判断游戏是否结束？
        //if (IsGameOver()) {
        //    GameOver();
        //}
    }


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

    public void MoveNumber(MoveDirection direction) {
        switch (direction) {
            case MoveDirection.UP:
                for (int j = 0; j < column; j++) {
                    for (int i = 1; i < row; i++) {
                        if (tiles[i][j].isHaveNumber()) {
                            Number number = tiles[i][j].GetNumber();

                            for (int m = i - 1; m >= 0; m--) {
                                Number targetNumber = null;
                                if (tiles[m][j].isHaveNumber()) {
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
                        if (tiles[i][j].isHaveNumber()) {
                            Number number = tiles[i][j].GetNumber();
                            for (int m = i + 1; m < row; m++) {
                                Number targetNumber = null;
                                if (tiles[m][j].isHaveNumber()) {
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
                        if (tiles[i][j].isHaveNumber()) {
                            Number number = tiles[i][j].GetNumber();
                            for (int m = j - 1; m >= 0; m--) {
                                Number targetNumber = null;
                                if (tiles[i][m].isHaveNumber()) {
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
                        if (tiles[i][j].isHaveNumber()) {
                            Number number = tiles[i][j].GetNumber();
                            for (int m = j + 1; m < column; m++) {
                                Number targetNumber = null;
                                if (tiles[i][m].isHaveNumber()) {
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

    //处理数字
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

    //恢复数字
    public void ResetNumberStatus() {
        for (int i = 0; i < row; i++) {
            for (int j = 0; j < column; j++) {
                if (tiles[i][j].isHaveNumber()) {
                    tiles[i][j].GetNumber().status = NumberState.Normal;
                }
            }
        }
    }

}