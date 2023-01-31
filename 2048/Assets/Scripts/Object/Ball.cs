using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ball : MonoBehaviour
{
    #region 自身组件
    public TextMeshPro numberText;

    #endregion

    #region 业务变量
    public float initSize;

    private float scale;

    private float leftEdge;

    private float rightEdge;

    private float bottomEdge;

    public Color[] bg_Colors;

    public Color[] num_Colors;

    public List<int> number_index;

    public Sprite[] ballSprites;

    #endregion

    void Awake() {


        float halfSreenHeight = Camera.main.orthographicSize;
        float halfScreenWidth = Screen.width / (float)Screen.height * halfSreenHeight;

        leftEdge = -halfScreenWidth - 1f;

        rightEdge = halfScreenWidth + 1f;

        bottomEdge = -halfSreenHeight - 1f;

        //Debug.Log($"left:{leftEdge}, right:{rightEdge}, bottom:{bottomEdge}");

    }
    
    public void Update() {
        if (transform.position.x < leftEdge || transform.position.x > rightEdge || transform.position.y < bottomEdge) {
            Destroy(gameObject);
            BallMain.instance.SubtractLife();
            BallManager.instancs.NeedNewBall();

        } else {
            scale = initSize + Mathf.Log(GetNumber(), 2) / 20;
            transform.localScale = new Vector3(scale, scale, 1);
            GetComponent<SpriteRenderer>().sprite = ballSprites[number_index.IndexOf(GetNumber())];
            numberText.color = num_Colors[number_index.IndexOf(GetNumber())];
        }
    }

    

    /// <summary>
    /// 碰撞事件
    /// </summary>
    /// <param name="collision"></param>
    public void OnCollisionEnter2D(Collision2D other) {
        
        if (other.gameObject.tag == "Ball") {
            if (GetInstanceID() < other.gameObject.GetInstanceID()) {
                return;
            }
            
            if (GetNumber() == other.gameObject.GetComponent<Ball>().GetNumber()) {
                //Debug.Log($"发生碰撞,碰撞数字:{GetNumber()}, 位置, this:{transform.position}, other:{other.transform.position}");
                BallManager.instancs.MergeBalls(gameObject, other.gameObject);

            }
        }
    }
    /// <summary>
    /// 获取本对象球内数字
    /// </summary>
    /// <returns></returns>
    public int GetNumber() {
        return int.Parse(numberText.text);
    }

    /// <summary>
    /// 设置本对象球内数字
    /// </summary>
    /// <param name="number"></param>
    public void SetNumber(int number) {
        numberText.text = number.ToString();
        
    }
    
}
