using System.Collections;
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

    public GameObject itemPrefab;

    private Vector3 pointerDownPos, pointerUpPos;

    private bool moveAndMerge;

    public enum MoveDirection {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    void Awake() {

        InitGrid();

        StartCoroutine(CreateNumber());
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
    public void InitGrid() {
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
                tiles[i][j].x = i;
                tiles[i][j].y = j;
            }
        }
    }

    public Tile CreateTile() {
        GameObject gameObject = GameObject.Instantiate(itemPrefab, grid);
        //gameObject.GetComponent<Tile>().InitNumber(false);
        return gameObject.GetComponent<Tile>();

    }
    /// <summary>
    /// 创建数字
    /// </summary>
    public IEnumerator CreateNumber() {
        // 选择格子
        emptytiles.Clear();
        for (int i = 0; i < row; i++) {
            for (int j = 0; j < column; j++) {
                if (!tiles[i][j].hasNumber()) {
                    emptytiles.Add(tiles[i][j]);
                }
            }
        }
        if (emptytiles.Count == 0) {
            // TODO YOU LOSE
            Debug.Log("You Lose !!!");
            yield return 0; 
        } else {
            yield return new WaitForSeconds(0.5f);
            // 随机一个格子
            int index = Random.Range(0, emptytiles.Count);
            emptytiles[index].gameObject.GetComponent<Tile>().CreateNumber();
            Debug.Log($"产生新点：({emptytiles[index].x}, {emptytiles[index].y})");
        }
        
    }


    public void OnPointDownEvent() {
        pointerDownPos = Input.mousePosition;
    }

    public void OnPointUpEvent() {
        pointerUpPos = Input.mousePosition;

        if (Vector3.Distance(pointerUpPos, pointerDownPos) < 10) {
            Debug.Log("无效滑动");
            return;
        }
        MoveDirection moveDirection = GetMoveDirection();
        Debug.Log($"滑动类型:{moveDirection}");
        MoveAndMergeNumber(moveDirection);
        if (moveAndMerge) {
            StartCoroutine(CreateNumber());
        }
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
    /// <summary>
    /// 核心算法：移动数字 
    /// </summary>
    /// <param name="direction"></param>
    public void MoveAndMergeNumber(MoveDirection direction) {
        moveAndMerge = false;
        switch (direction) {
            case MoveDirection.UP:
                for (int j = 0; j < column; j++) {
                    for (int i = 1; i < row; i++) {
                        if (tiles[i][j].hasNumber()) {
                            //处理当前tiles[i][j]
                            int targetRow = i;
                            bool merge = false;
                            for (int m = i - 1; m >= 0; m--) {
                                if (tiles[m][j].hasNumber()) {
                                    if (tiles[i][j].GetNumber().GetNumberValue() == tiles[m][j].GetNumber().GetNumberValue()) {
                                        tiles[i][j].MergeNumber(tiles[m][j]);
                                        merge = true;
                                        moveAndMerge = true;
                                        break;
                                    }

                                } else {
                                    targetRow = m;
                                }
                            }
                            if (tiles[i][j].hasNumber() && !merge && targetRow != i) {
                                tiles[i][j].MoveNumber(tiles[targetRow][j]);
                                moveAndMerge = true;
                            }
                        }
                    }
                }
                break;
            case MoveDirection.DOWN:
                for (int j = 0; j < column; j++) {
                    for (int i = row - 2; i >= 0; i--) {
                        if (tiles[i][j].hasNumber()) {
                            //处理当前tiles[i][j]
                            int targetRow = i;
                            bool merge = false;
                            for (int m = i + 1; m <= row - 1; m++) {
                                if (tiles[m][j].hasNumber()) {
                                    if (tiles[i][j].GetNumber().GetNumberValue() == tiles[m][j].GetNumber().GetNumberValue()) {
                                        tiles[i][j].MergeNumber(tiles[m][j]);
                                        merge = true;
                                        moveAndMerge = true;
                                        break;
                                    }

                                } else {
                                    targetRow = m;
                                }
                            }
                            if (tiles[i][j].hasNumber() && !merge && targetRow != i) {
                                tiles[i][j].MoveNumber(tiles[targetRow][j]);
                                moveAndMerge = true;
                            }
                        }
                    }
                }
                break;
            case MoveDirection.LEFT:
                for (int i = 0; i < row; i++) {
                    for (int j = 1; j < column; j++) {
                        if (tiles[i][j].hasNumber()) {
                            //处理当前tiles[i][j]
                            int targetColumn = j;
                            bool merge = false;
                            for (int m = j - 1; m >= 0; m--) {
                                if (tiles[i][m].hasNumber()) {
                                    if (tiles[i][j].GetNumber().GetNumberValue() == tiles[i][m].GetNumber().GetNumberValue()) {
                                        tiles[i][j].MergeNumber(tiles[i][m]);
                                        merge = true;
                                        moveAndMerge = true;
                                        break;
                                    }

                                } else {
                                    targetColumn = m;
                                }
                            }
                            if (tiles[i][j].hasNumber() && !merge && targetColumn != j) {
                                tiles[i][j].MoveNumber(tiles[i][targetColumn]);
                                moveAndMerge = true;
                            }
                        }
                    }
                }
                break;
            case MoveDirection.RIGHT:
                for (int i = 0; i < row; i++) {
                    for (int j  = column - 2; j >= 0; j--) {
                        if (tiles[i][j].hasNumber()) {
                            //处理当前tiles[i][j]
                            int targetColumn = j;
                            bool merge = false;
                            for (int m = j + 1; m < column; m++) {
                                if (tiles[i][m].hasNumber()) {
                                    if (tiles[i][j].GetNumber().GetNumberValue() == tiles[i][m].GetNumber().GetNumberValue()) {
                                        tiles[i][j].MergeNumber(tiles[i][m]);
                                        merge = true;
                                        moveAndMerge = true;
                                        break;
                                    }

                                } else {
                                    targetColumn = m;
                                }
                            }
                            if (tiles[i][j].hasNumber() && !merge && targetColumn != j) {
                                tiles[i][j].MoveNumber(tiles[i][targetColumn]);
                                moveAndMerge = true;
                            }
                        }
                    }
                }
                break;
        }
    }

}