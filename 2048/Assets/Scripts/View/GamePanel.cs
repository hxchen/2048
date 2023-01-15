using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
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

    public MyGrid[][] grids = null;

    public List<MyGrid> emptyGrids = new List<MyGrid>();

    public GameObject itemPrefab;
    public GameObject numberPrefab;

    void Awake() {

        InitGrid();

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
    public void InitGrid() {
        int number = PlayerPrefs.GetInt(Const.GameModel, 4);
        GridLayoutGroup gridLayoutGroup = grid.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.constraintCount = number;
        gridLayoutGroup.cellSize = new Vector2(gridConfig[number], gridConfig[number]);

        grids = new MyGrid[number][];

        row = number;
        column = number;
        for (int i = 0; i < row; i++) {
            for (int j = 0; j < column; j++) {
                if (grids[i] == null) {
                    grids[i] = new MyGrid[number];
                }
                grids[i][j] = CreateGrid();
            }
        }
    }

    public MyGrid CreateGrid() {
        GameObject gameObject = GameObject.Instantiate(itemPrefab, grid);
        return gameObject.GetComponent<MyGrid>();

    }
    /// <summary>
    /// 创建数字
    /// </summary>
    public void CreateNumber() {
        // 选择格子
        emptyGrids.Clear();
        for (int i = 0; i < row; i++) {
            for (int j = 0; j < column; j++) {
                if (grids[i][j].isEmpty()) {
                    emptyGrids.Add(grids[i][j]);
                }
            }
        }
        if (emptyGrids.Count == 0) {
            // TODO YOU LOSE
            return;
        }
        
        // 随机一个格子
        int index = Random.Range(0, emptyGrids.Count);

        // 数字放进格子
        GameObject gameObject =  GameObject.Instantiate(numberPrefab, emptyGrids[index].transform);

    }

}
