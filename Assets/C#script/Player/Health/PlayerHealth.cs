using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerHealth : MonoBehaviour
{
    private PlayerAnim playerAnim;
    public GameObject BlockBox;
    [Header("格挡灯光")]
    public float LightTime;
    private float LightTime_Count;
    public Light2D BlockLight;
    private bool IsBlock;
    [Header("玩家血量")]
    private bool IsDead;
    public float Fullhealth;
    public float Currenthealth;
    [Header("事件监听")]
    public FloatEventSO AttackPlayerEvent;
    private void Awake()
    {
        Currenthealth = Fullhealth;
        playerAnim = GetComponent<PlayerAnim>();
    }
    private void Update()
    {
        OnBlockWin();
        if (Currenthealth <= 0)
        {
            if (!IsDead)
            {
                IsDead = true;
                PlayerDead();
            }
        }
    }
    private void PlayerDead()
    {
        playerAnim.PlayerisDead();
    }
    private void OnEnable()
    {
        AttackPlayerEvent.OnRaiseFloatEvent += OnAttackPlayer;
    }

    private void OnAttackPlayer(float BossAttackCount)
    {
        if (BlockBox.activeSelf == true)
        {
            IsBlock = true;
            LightTime_Count = LightTime;
            BlockLight.intensity = 2f;
            playerAnim.OnWinBlock();
        }
        else if (BlockBox.activeSelf == false) 
        {
            Currenthealth -= BossAttackCount;
        }
    }
    private void OnBlockWin()
    {
        if (LightTime_Count > 0)
        {
            LightTime_Count -= Time.deltaTime;
        }
        if (LightTime_Count <= 0 && IsBlock)
        {
            IsBlock = false;
            BlockLight.intensity = 1;
        }
    }
    private void OnDisable()
    {
        AttackPlayerEvent.OnRaiseFloatEvent -= OnAttackPlayer;
    }
}
