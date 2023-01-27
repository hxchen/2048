using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Number : MonoBehaviour
{
    private Image bg;
    private Text number_text;
    private Tile tile;

    public NumberState status;

    private float spawnScaleTime = 1;
    private bool isPlaySpawnAnim;

    private float mergeScaleTime = 1;
    private float mergeScaleTimeBack = 1;
    private bool isPlayMergeAnim;

    private float movePosTime = 1;
    private bool isMoving;
    private bool isDestroyOnMoveEnd;
    private Vector3 startMovPos;

    public Color[] bg_Colors;
    public List<int> number_index;

    public int index;
    private void Awake() {
        //初始化
        bg = transform.GetComponent<Image>();
        number_text = gameObject.GetComponentInChildren<Text>();
        //index = Random.Range(0, 2);
        index = 1;
    }

    //初始化
    public void Init(Tile tile) {
        tile.SetNumber(this);
        //设置所在的格子
        this.SetTile(tile);
        //设置初始化的数字
        if (index == 1)
            SetNumberValue(2);
        else
            SetNumberValue(4);
        status = NumberState.Normal;

        PlaySpawnAnimation();
    }

    //设置格子
    public void SetTile(Tile tile) {
        this.tile = tile;
    }
    //获取格子
    public Tile GetTile() {
        return this.tile;
    }

    //设置数字
    public void SetNumberValue(int number) {
        number_text.text = number.ToString();
        bg.color = bg_Colors[number_index.IndexOf(number)];
    }

    //获取数字
    public int GetNumberValue() {
        return int.Parse(number_text.text);
    }

    //把数字移动到某个格子下面
    public void MoveToTile(Tile tile) {
        transform.SetParent(tile.transform);
        //transform.localPosition = Vector3.zero;
        startMovPos = transform.localPosition;

        PlayMoveAnimation();

        this.GetTile().SetNumber(null);
        //设置格子
        tile.SetNumber(this);
        this.SetTile(tile);
    }

    public void DestroyOnMoveEnd(Tile tile) {
        transform.SetParent(tile.transform);
        startMovPos = transform.localPosition;

        PlayMoveAnimation();
        isDestroyOnMoveEnd = true;
    }

    //合并
    public void Merge() {
        GameBoard gameBoard = GameObject.Find("Canvas/GameBoard").GetComponent<GameBoard>();
        
        int resultNumber = this.GetNumberValue() * 2;
        gameBoard.AddScore(resultNumber);

        this.SetNumberValue(this.GetNumberValue() * 2);
        if (resultNumber == 2048) {
            gameBoard.GameWin();
        }
        status = NumberState.NotMerge;
        PlayMergeAnimation();
        SoundManager.instance.PlaySound();
    }


    //判断能不能合并
    public bool IsMerge(Number number) {
        if (this.GetNumberValue() == number.GetNumberValue() && number.status == NumberState.Normal)
            return true;
        return false;
    }


    public void PlaySpawnAnimation() {
        spawnScaleTime = 0;
        isPlaySpawnAnim = true;
    }

    public void PlayMergeAnimation() {
        mergeScaleTime = 0;
        mergeScaleTimeBack = 0;
        isPlayMergeAnim = true;
    }

    public void PlayMoveAnimation() {
        movePosTime = 0;
        isMoving = true;
    }

    public void Update() {
        //创建的动画
        if (isPlaySpawnAnim) {
            if (spawnScaleTime <= 1) {
                spawnScaleTime += Time.deltaTime * 4;
                transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, spawnScaleTime);
            } else
                isPlaySpawnAnim = false;
        }

        //合并的动画 
        if (isPlayMergeAnim) {
            if (mergeScaleTime <= 1 && mergeScaleTimeBack == 0) {
                mergeScaleTime += Time.deltaTime * 4;
                transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.2f, mergeScaleTime);
            }
            if (mergeScaleTime >= 1 && mergeScaleTimeBack <= 1) {
                mergeScaleTimeBack += Time.deltaTime * 4;
                transform.localScale = Vector3.Lerp(Vector3.one * 1.2f, Vector3.one, mergeScaleTime);
            }
            if (mergeScaleTime >= 1 && mergeScaleTimeBack >= 1)
                isPlayMergeAnim = false;
        }


        //移动的动画
        if (isMoving) {

            movePosTime += Time.deltaTime * 5;
            transform.localPosition = Vector3.Lerp(startMovPos, Vector3.zero, movePosTime);
            if (movePosTime >= 1) {
                isMoving = false;
                if (isDestroyOnMoveEnd)
                    Destroy(this.gameObject);
            }

        }
    }
}
