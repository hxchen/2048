using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 屏幕左右边界碰撞体
/// </summary>
public class SideCollider : MonoBehaviour
{
    [Header("border colliders --- ")]
    public Transform leftBorderCollider;
    public Transform rightBorderCollider;

    [Header("half collider width --- ")]
    public float halfColliderWidth = 0.01f;

    // half screen width size
    private float halfScreenWidth;

    private void Start() {
        float halfSreenHeight = Camera.main.orthographicSize;
        halfScreenWidth = Screen.width / (float)Screen.height * halfSreenHeight;
        leftBorderCollider.position = new Vector3(-halfScreenWidth + halfColliderWidth, leftBorderCollider.position.y, leftBorderCollider.position.z);
        rightBorderCollider.position = new Vector3(halfScreenWidth - halfColliderWidth, rightBorderCollider.position.y, rightBorderCollider.position.z);
    }
}
