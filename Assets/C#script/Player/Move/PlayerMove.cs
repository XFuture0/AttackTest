using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private PlayerCheck playercheck;
    private PlayerController inputAction;
    private PlayerAnim playerAnim;
    [HideInInspector]public Vector2 playermove;
    private Rigidbody2D rb;
    private float MainGravity;
    [Header("人物属性")]
    public float PlayerSpeed;
    public float SpeedMax;
    private float PlayerStart;
    [HideInInspector]public int Combo;
    private bool IsHoldAttack;
    private bool CanHoldAttack;
    [HideInInspector]public bool IsBlocking;
    [Header("冲刺特效")]
    public ParticleSystem StopDashEffect;
    public GameObject DashEffect;
    private bool isDash_Effect;
    private bool IsStopDashEffect;
    [Header("格挡特效")]
    public GameObject BlockEffect;
    [Header("冲刺特效计时器")]
    public float DashEffecttime;
    private float DashEffecttime_Count;
    [Header("人物跳跃")]
    private int Jump_Count;
    public float JumpHigh;
    [HideInInspector]public bool isJumping;
    [Header("人物翻滚")]
    private bool isRoll;
    public float RollSpeed;
    [Header("翻滚计时器")]
    public float time;
    private float time_Count;
    [Header("加速计时器")]
    public float Speedtime;
    private float Speedtime_Count;
    [Header("人物冲刺")]
    private bool isDash;
    [Header("冲刺计时器")]
    public float DashSpeed;
    public float Dashtime;
    private float Dashtime_Count;
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
        inputAction.Player.Roll.started += OnRoll;
        inputAction.Player.Dash.started += OnDash;
        inputAction.Player.Block.started += OnBlock;
        PlayerSpeedEvent.OnRaiseEvent += OnPlayerSpeed;
        GetPlayerPoEvent.OnRaiseEvent += OnGetPlayerPo;
        MainGravity = rb.gravityScale;
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
            PlayerSpeed *= 0.999f;
            if(PlayerSpeed < PlayerStart)
            {
                PlayerSpeed = PlayerStart;
            }
        }
        OnDashEffecting();
    }
    private void FixedUpdate()
    {
        if (!isRoll)
        {
            Move();
        }
        OnRolling();
        OnDashing();
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
        if (math.abs(rb.velocity.x) < 10)
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
        else if (math.abs(rb.velocity.x) > 10)
        {
            playerAnim.OnRockHit();
            PlayerSpeed = PlayerStart;
            yield return new WaitForSeconds(0.25f);
            CanHoldAttack = true;
        }
    }
    private void CancelAttack(InputAction.CallbackContext context)
    {
        IsHoldAttack = false;
    }
    private void OnRolling()
    {
        if (isRoll && time_Count > 0)
        {
            time_Count -= Time.deltaTime;
            rb.velocity = new Vector2(RollSpeed * transform.localScale.x * Time.deltaTime, rb.velocity.y);
        }
        if (time_Count <= 0)
        {
            isRoll = false;
        }
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
    private void OnDashEffecting()
    {
        if (isDash_Effect)
        {
            DashEffecttime_Count -= Time.deltaTime;
        }
        if(DashEffecttime_Count < 0)
        {
            if (!IsStopDashEffect)
            {
                IsStopDashEffect = true;
                StopDashEffect.Stop();
            }
        }
        if(DashEffecttime_Count < -1.5f)
        {
            isDash_Effect = false;
            DashEffect.SetActive(false);
        }
    }
    private void OnDashing()
    {
        if (isDash && Dashtime_Count > 0)
        {
            rb.gravityScale = 0;
            Dashtime_Count -= Time.deltaTime;
            rb.velocity = new Vector2(DashSpeed * transform.localScale.x * Time.deltaTime,0);
        }
        if (isDash && Dashtime_Count <= 0)
        {
            rb.gravityScale = MainGravity;
            PlayerSpeed = PlayerStart;
            isDash = false;
        }
    }
    private void OnDash(InputAction.CallbackContext context)
    {
        if(PlayerSpeed > 550)
        {
            if (!isDash && !isRoll)
            {
                DashEffecttime_Count = DashEffecttime;
                DashEffect.SetActive(true);
                StopDashEffect.Play();
                IsStopDashEffect = false;
                if (transform.localScale.x == 1)
                {
                    DashEffect.transform.localRotation = quaternion.Euler(-90, 180, 0);
                }
                if(transform.localScale.x == -1)
                {
                    DashEffect.transform.localRotation = quaternion.Euler(-90, 0, 0);
                }
                isDash_Effect = true;
                isDash = true;
                Dashtime_Count = Dashtime;
                playerAnim.DashTime();
            }
        }
    }
    private void OnBlock(InputAction.CallbackContext context)
    {
        if (!IsBlocking)
        {
            rb.velocity = Vector2.zero;
            IsBlocking = true;
            playerAnim.OnBlock();
        }
    }
    public void SetBlockEffect()
    {
        if(transform.localScale.x == 1)
        {
            var BlockEffectPo = new Vector3(transform.position.x + 0.62f, transform.position.y - 0.1f, transform.position.z);
            Instantiate(BlockEffect, BlockEffectPo, Quaternion.Euler(0, 0, 0));
        }
        else if (transform.localScale.x == -1)
        {
            var BlockEffectPo = new Vector3(transform.position.x - 0.62f, transform.position.y + 0.1f, transform.position.z);
            Instantiate(BlockEffect, BlockEffectPo, Quaternion.Euler(0, 180, 0));
        }
    }
    public void EndBlock()
    {
        IsBlocking = false;
    }
    public void BlockContinuePlayer()
    {
        playerAnim.CanMove = true;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
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
