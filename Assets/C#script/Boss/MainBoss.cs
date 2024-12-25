using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class MainBoss : MonoBehaviour
{
    private Animator animor;
    private Bossanim anim;
    private BossHealth health;
    public GameObject MainMap;
    public GameObject WaterGround;
    public GameObject BossTip;
    public GameObject PlayerBox;
    [HideInInspector]public Rigidbody2D rb;
    [HideInInspector]public Transform Player;
    [Header("状态")]
    private int n;
    private State Temp;
    public State CurrentState;
    [HideInInspector] public State hideState;
    private State walkState;
    private State iceState;
    private State surfState;
    private State ballattackState;
    [Header("Boss属性")]
    public float BossSpeed;
    public bool isAttack1z1;
    public bool isAttack2;
    private bool isState2;
    private bool isCatchingPlayer;
    [Header("伤害飘字")]
    public GameObject HurtCount_White;
    public GameObject HurtCount_Gold;
    public GameObject HurtCountBox;
    [Header("技能计时器")]
    [HideInInspector]public bool IsSkill;
    [HideInInspector]public bool isHitPlayer;
    public float SkillTime;
    public float SkillTime_Count;
    [Header("受击计时器")]
    public float BeHurtTime;
    private float BeHurtTime_Count;
    [Header("顿帧计时器")]
    private bool IsStopTime;
    public float StopTime;
    private float StopTime_Count;
    [Header("水球计时器")]
    public float WaterTime;
    private float WaterTime_Count;
    private bool isSetWaterBall;
    private int WaterBallCount;
    [Header("广播")]
    public VoidEventSO GetPlayerPoEvent;
    public FloatEventSO GetBossHurtCountEvent;
    public TransFormEventSO ReturnBossPoEvent;
    public TransFormEventSO SetIceBoxEvent;
    public TransFormEventSO GetPlayerEvent;
    public VoidEventSO PatPlayerEvent;
    [Header("事件监听")]
    public VoidEventSO GetBossPoEvent;
    public TransFormEventSO ReturnPlayerPoEvent;
    public FloatEventSO AttackBossEvent;
    public VoidEventSO HitPlayerEvent;
    [Header("冰剑")]
    public GameObject IcePool;
    private void Awake()
    {
        IsSkill = false;
        WaterBallCount = 0;
        WaterTime_Count = WaterTime;
        iceState = new IceState();
        hideState = new HideState();
        walkState = new WalkState();
        surfState = new SurfState();
        ballattackState = new BallAttackState();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Bossanim>();
        health = GetComponent<BossHealth>();
        animor = GetComponent<Animator>();
        SkillTime_Count = SkillTime;
        BeHurtTime_Count = BeHurtTime;
    }
    private void Start()
    {
        CurrentState = hideState;
        CurrentState.OnEnter(this);
    }
    private void Update()
    {
        OnSkill();
        CurrentState.LogicUpdate(this);
        SetDefend();
        AttackBossStop();
        OnSetWaterBall();
        OnCatchPlayer();
    }
    private void FixedUpdate()
    {
        CurrentState.PhysicsUpdate(this);
        BossWalk();
    }
    public void ChangeState(StateType changeState)
    {
        var newstate = changeState switch
        {
            StateType.Hide => hideState,
            StateType.Walk => walkState,
            StateType.Ice => iceState,
            StateType.Surf => surfState,
            StateType.BallAttack => ballattackState,
            _ => null
        };
        CurrentState.OnExit(this);
        Temp = newstate;
        StartCoroutine(EnterTip());
    }
    private IEnumerator EnterTip()
    {
        if (Temp == walkState || Temp == iceState)
        {
            BossTip.SetActive(true);
            OnHideAnim();
            yield return new WaitForSeconds(1f);
            OnHideAnim();
            BossTip.SetActive(false);
            CurrentState = Temp;
            CurrentState.OnEnter(this);
            Debug.Log(CurrentState.ToString());
        }
        else
        {
            CurrentState = Temp;
            CurrentState.OnEnter(this);
        }
    }
    private void BossWalk()
    {
        if (anim.IsWalk || anim.IsSurf)
        {
            var BossFace = transform.localScale.x;
            rb.velocity = new Vector2(BossFace * BossSpeed * Time.deltaTime, rb.velocity.y);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
    public void OnHideAnim()
    {
        anim.OnHideAnim();
    }
    public void OnWalkAnim()
    {
        anim.OnWalkAnim();
    }
    public void OnSurfAnim()
    {
        anim.OnSurfAnim();
    }
    public void HitPlayer1z1()
    {
        if (isHitPlayer)
        {
            isCatchingPlayer = true;
        }
    }
    public void OnAttack1z1Anim()
    {
        anim.OnAttack1z1();
        Invoke("OnWalkAnim", 0.2f);
        StartCoroutine(StartAttack1z2());
    }
    private IEnumerator StartAttack1z2()
    {
        yield return new WaitForSeconds(0.5f);
        if (isHitPlayer)
        {
            OnAttack1z2Anim();
        }
        if(!isHitPlayer)
        {
            isHitPlayer = false;
            OnWalkAnim();
            ChangeState(StateType.Hide);
        }
    }
    public void OnAttack2Anim()
    {
        anim.OnAttack1z1();
        StartCoroutine(StartAttack2());
    }
    private IEnumerator StartAttack2()
    {
        yield return new WaitForSeconds(0.5f);
        if (isHitPlayer)
        {
            anim.OnAttack2();
        }
        else
        {
            isHitPlayer = false;
            OnSurfAnim();
            ChangeState(StateType.Hide);
        }
    }
    private void OnCatchPlayer()
    {
        if (isCatchingPlayer)
        {
            GetPlayerEvent.RaiseTransFormEvent(PlayerBox.transform);
        }
    }
    public void OnAttack1z2Anim()
    {
        anim.OnAttack1z2();
    }
    public void OnBallAttackAnim()
    {
        anim.OnBallAttack();
    }
    public void OnDefendAnim()
    {
        anim.OnDefend();
    }
    public void OnHealAnim()
    {
        anim.OnHeal();
    }
    public void GetPlayerPo()
    {
        GetPlayerPoEvent.RaiseEvent();
    }
    private void ChooseState()
    {
        if (!isState2)
        {
            n = UnityEngine.Random.Range(1, 3);
        }
        if (isState2)
        {
            n = UnityEngine.Random.Range(3, 5);
        }
        switch (n)
        {
            case 1:
                ChangeState(StateType.Walk);
                break;
            case 2:
                ChangeState(StateType.Ice);
                break;
            case 3:
                ChangeState(StateType.Surf);
                break;
            case 4:
                ChangeState(StateType.BallAttack);
                break;
        }
    }
    private void OnSkill()
    {
        if (!IsSkill)
        {
            SkillTime_Count -= Time.deltaTime;
        }
        if (SkillTime_Count <= 0 && !IsSkill)
        {
            SkillTime_Count = SkillTime;
            IsSkill = true;
            ChooseState();
        }
    }
    private void OnEnable()
    {
        ReturnPlayerPoEvent.OnRaiseTransFormEvent += OnReturnPlayerPo;
        HitPlayerEvent.OnRaiseEvent += OnHitPlayer;
        AttackBossEvent.OnRaiseFloatEvent += OnAttackBoss;
        GetBossPoEvent.OnRaiseEvent += OnGetBossPo;
    }

    private void OnGetBossPo()
    {
        ReturnBossPoEvent.RaiseTransFormEvent(transform);
    }
    private void OnHitPlayer()
    {
        isHitPlayer = true;
        StopTime_Count = StopTime;
        IsStopTime = true;
    }
    private void OnReturnPlayerPo(Transform PlayerPo)
    {
        Player = PlayerPo;
    }
    public void BossTurnFace()
    {
        if (transform.position.x - Player.position.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
    public void OnPatPlayer()
    {
        PatPlayerEvent.RaiseEvent();
        isCatchingPlayer = false;
        isHitPlayer = false;
    }
    public void ReturnHideState_Walk()
    {
        OnWalkAnim();
        ChangeState(StateType.Hide);
    }
    public void ReturnHideState_Surf()
    {
        isHitPlayer = false;
        OnSurfAnim();
        ChangeState(StateType.Hide);
    }
    public void ReturnHideState_Defend()
    {
        health.IsBeHurt = false;
        health.BeHurtCount = 0;
        BeHurtTime_Count = BeHurtTime;
        ChangeState(StateType.Hide);
    }
    public void ReturnHideState()
    {
        ChangeState(StateType.Hide);
    }
    public void SetIce()
    {
        StartCoroutine(SetIceTimeFix());
        ChangeState(StateType.Hide);
    }
    private void SetDefend()
    {
        if (health.IsBeHurt)
        {
            BeHurtTime_Count -= Time.deltaTime;
        }
        if(health.BeHurtCount >= 8 && health.IsBeHurt)
        {
            StartCoroutine(WaitDefend());
            health.BeHurtCount = 0;
            health.IsBeHurt = false;
            var m = UnityEngine.Random.Range(1, 3);
            switch (m)
            {
                case 1:
                    SkillTime_Count = SkillTime;
                    GetPlayerPo();
                    BossTurnFace();
                    OnDefendAnim();
                    IsSkill = false;
                    break;
                case 2:
                    SkillTime_Count = SkillTime;
                    OnHealAnim();
                    OnWaterBallFalling();
                    IsSkill = false;
                    break;
            }
            BeHurtTime_Count = BeHurtTime;
        }
        else if(BeHurtTime_Count < 0 && health.BeHurtCount < 8 && health.IsBeHurt)
        {
            health.IsBeHurt = false;
            health.BeHurtCount = 0;
            BeHurtTime_Count = BeHurtTime;
        }
    }
    private IEnumerator WaitDefend()
    {
        BossTip.SetActive(true);
        yield return new WaitForSeconds(1f);
        BossTip.SetActive(false);
    }
    private IEnumerator SetIceTimeFix()
    {
        yield return new WaitForSeconds(0.2f);
        if (transform.localScale.x == 1)
        {
            var SetPo = new Vector3(transform.position.x + 1.85f,transform.position.y - 3f,transform.position.z);
            Instantiate(IcePool,SetPo, Quaternion.identity);
            SetIceBoxEvent.RaiseTransFormEvent(transform);
        }
        if (transform.localScale.x == -1)
        {
            var SetPo = new Vector3(transform.position.x - 1.85f, transform.position.y - 3f, transform.position.z);
            Instantiate(IcePool, SetPo, Quaternion.identity);
            SetIceBoxEvent.RaiseTransFormEvent(transform);
        }
    }
    private void OnAttackBoss(float HurtCount)
    {
        StopTime_Count = StopTime;
        IsStopTime = true;
        GetPlayerPoEvent.RaiseEvent();
        SetBossHurtCount(HurtCount);
    }
    private void SetBossHurtCount(float hurtCount)
    {
        if(Player.localScale.x == -1)
        {
            var SethurtPo = new Vector3(transform.position.x - 0.5f,transform.position.y - 1.7f,transform.position.z);
            if(hurtCount < 15)
            {
                Instantiate(HurtCount_White, SethurtPo, Quaternion.identity, HurtCountBox.transform);
                GetBossHurtCountEvent.RaiseFloatEvent(hurtCount);
            }
            else
            {
                Instantiate(HurtCount_Gold, SethurtPo, Quaternion.identity, HurtCountBox.transform);
                GetBossHurtCountEvent.RaiseFloatEvent(hurtCount);
            }
        }
        if(Player.localScale.x == 1)
        {
            var SethurtPo = new Vector3(transform.position.x + 0.9f, transform.position.y - 1.7f, transform.position.z);
            if (hurtCount < 15)
            {
                Instantiate(HurtCount_White, SethurtPo, Quaternion.identity, HurtCountBox.transform);
                GetBossHurtCountEvent.RaiseFloatEvent(hurtCount);
            }
            else
            {
                Instantiate(HurtCount_Gold, SethurtPo, Quaternion.identity, HurtCountBox.transform);
                GetBossHurtCountEvent.RaiseFloatEvent(hurtCount);
            }
        }
    }
    private void AttackBossStop()
    {
        if (StopTime_Count > 0 && IsStopTime)
        {
            StopTime_Count -= Time.deltaTime;
            animor.speed = math.lerp(0, 1, (1 - StopTime_Count / StopTime));
        }
        if (StopTime_Count < 0 && IsStopTime)
        {
            IsStopTime = false;
        }
    }
    public void OnWaterBallFalling()
    {
        WaterBallCount = 5;
        isSetWaterBall = true;
    }
    private void OnSetWaterBall()
    {
        if (isSetWaterBall)
        {
            WaterTime_Count -= Time.deltaTime;
            if (WaterBallCount > 0 && WaterTime_Count < 0)
            {
                WaterBallCount--;
                var l = UnityEngine.Random.Range(-3, 2);
                var SetWaterPo = new Vector3(transform.position.x + l, transform.position.y - 3.5f, transform.position.z);
                Instantiate(WaterGround, SetWaterPo, Quaternion.identity);
                WaterTime_Count = WaterTime;
            }
            if (WaterBallCount == 0)
            {
                isSetWaterBall = false;
            }
        }
    }
    private void OnDisable()
    {
        ReturnPlayerPoEvent.OnRaiseTransFormEvent -= OnReturnPlayerPo;
        HitPlayerEvent.OnRaiseEvent -= OnHitPlayer;
        AttackBossEvent.OnRaiseFloatEvent -= OnAttackBoss;
        GetBossPoEvent.OnRaiseEvent -= OnGetBossPo;
    }
    public void State2()
    {
        MainMap.SetActive(false);
        isState2 = true;
        ChangeState(StateType.Hide);
    }
}
