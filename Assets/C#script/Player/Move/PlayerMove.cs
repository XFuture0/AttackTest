using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private PlayerCheck playercheck;
    private PlayerController inputAction;
    private PlayerAnim playerAnim;
    [HideInInspector]public Vector2 playermove;
    private Rigidbody2D rb;
    [Header("人物属性")]
    public float PlayerSpeed;
    public float SpeedMax;
    private float PlayerStart;
    [HideInInspector]public int Combo;
    private bool IsHoldAttack;
    private bool CanHoldAttack;
    [Header("人物跳跃")]
    private int Jump_Count;
    public float JumpHigh;
    [HideInInspector]public bool isJumping;
    [Header("人物冲刺")]
    private bool isRoll;
    public float RollSpeed;
    [Header("计时器")]
    public float time;
    private float time_Count;
    [Header("加速计时器")]
    public float Speedtime;
    private float Speedtime_Count;
    [Header("广播")]
    public TransFormEventSO ReturnPlayerPoEvent;
    [Header("事件监听")]
    public VoidEventSO PlayerSpeedEvent;
    public VoidEventSO GetPlayerPoEvent;
    private void Awake()
    {
        Jump_Count = 2;
        Combo = -1;
        time_Count = 0;
        CanHoldAttack = true;
        PlayerStart = PlayerSpeed;
        inputAction = new PlayerController();
        playercheck = GetComponent<PlayerCheck>();
        playerAnim = GetComponent<PlayerAnim>();
        rb = GetComponent<Rigidbody2D>(); 
    }
    private void OnEnable()
    {
        inputAction.Player.Jump.started += OnJump;
        inputAction.Player.Attack.performed += HoldAttack;
        inputAction.Player.Attack.canceled += CancelAttack;
        inputAction.Player.Dash.started += OnRoll;
        PlayerSpeedEvent.OnRaiseEvent += OnPlayerSpeed;
        GetPlayerPoEvent.OnRaiseEvent += OnGetPlayerPo;
        inputAction.Enable();
    }

    private void Update()
    {
        if (playercheck.IsGround)
        {
            Jump_Count = 1;
        }
        if (IsHoldAttack)
        {
            if (CanHoldAttack)
            {
                CanHoldAttack = false;
                StartCoroutine(HoldAttackIng());
            }
        }
        if (Speedtime_Count >= 0)
        {
            Speedtime_Count -= Time.deltaTime;
        }
        if(Speedtime_Count <= 0)
        {
            PlayerSpeed *= 0.9f;
            if(PlayerSpeed < PlayerStart)
            {
                PlayerSpeed = PlayerStart;
            }
        }
    }
    private void FixedUpdate()
    {
        if (!isRoll)
        {
            Move();
        }
        if (isRoll && time_Count > 0)
        {
            time_Count -= Time.deltaTime;
            rb.velocity = new Vector2(RollSpeed * transform.localScale.x * Time.deltaTime, rb.velocity.y);
        }
        if(time_Count <= 0)
        {
            isRoll = false;
        }
    }
    private void Move()
    {
        if(PlayerSpeed > SpeedMax)
        {
            PlayerSpeed = SpeedMax;
        }
        playermove = inputAction.Player.WASD.ReadValue<Vector2>();
        if (!IsHoldAttack)
        {
            rb.velocity = new Vector2(playermove.x * PlayerSpeed * Time.deltaTime, rb.velocity.y);
        }
        if (IsHoldAttack)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        if (playermove.x != 0)
        {
            if (playermove.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            if (playermove.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1 );
            }
        }
    }
    private void OnJump(InputAction.CallbackContext context)
    {
        if(Jump_Count > 0)
        {
            isJumping = true;
            Jump_Count--;
            rb.velocity = new Vector2(rb.velocity.x, JumpHigh);
        }
    }
    private void HoldAttack(InputAction.CallbackContext context)
    {
        IsHoldAttack = true;
    }
    private IEnumerator HoldAttackIng()
    {
        Combo++;
        if (Combo == 2)
        {
            Combo = 0;
        }
        playerAnim.AttackCombo();
        yield return new WaitForSeconds(0.25f);
        CanHoldAttack = true;
    }
    private void CancelAttack(InputAction.CallbackContext context)
    {
        IsHoldAttack = false;
    }
    private void OnRoll(InputAction.CallbackContext context)
    {
        if (!isRoll)
        {
            isRoll = true;
            time_Count = time;
            playerAnim.RollTime();
        }
    }
    private void OnPlayerSpeed()
    {
        Speedtime_Count = Speedtime;
        PlayerSpeed *= 1.2f;
    }
    private void OnGetPlayerPo()
    {
        ReturnPlayerPoEvent.RaiseTransFormEvent(this.transform);
    }
    private void OnDisable()
    {
        PlayerSpeedEvent.OnRaiseEvent -= OnPlayerSpeed;
        GetPlayerPoEvent.OnRaiseEvent -= OnGetPlayerPo;
    }
}
