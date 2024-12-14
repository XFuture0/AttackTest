using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheck : MonoBehaviour
{
    public bool IsGround;
    public LayerMask Ground;
    private PlayerMove playermove;
    [Header("判断地点调整")]
    public float AdjustX;
    public float AdjustY;
    private void Awake()
    {
        playermove = GetComponent<PlayerMove>();
    }
    private void Update()
    {
        Checked();
    }
    private void Checked()
    {
        var GroundCircle = new Vector2(transform.position.x, transform.position.y - 1);
        IsGround = Physics2D.OverlapCircle(GroundCircle,(float)0.2,Ground);
    }
}
 