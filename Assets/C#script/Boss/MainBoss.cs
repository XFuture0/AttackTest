using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class MainBoss : MonoBehaviour
{
    private Bossanim anim;
    private BossHealth health;
    public GameObject MainMap;
    [HideInInspector]public Rigidbody2D rb;
    [HideInInspector]public Transform Player;
    [Header("状态")]
    private int n;
    private State CurrentState;
    private State hideState;
    private State walkState;
    private State iceState;
    private State surfState;
    private State ballattackState;
    private State defendState;
    [Header("Boss属性")]
    public float BossSpeed;
    public bool isAttack1z1;
    public bool isAttack2;
    private bool isState2;
    [Header("技能计时器")]
    [HideInInspector]public bool IsSkill;
    [HideInInspector]public bool isHitPlayer;
    public float SkillTime;
    [HideInInspector]public float SkillTime_Count;
    [Header("受击计时器")]
    public float BeHurtTime;
    private float BeHurtTime_Count;
    [Header("广播")]
    public VoidEventSO GetPlayerPoEvent;
    [Header("事件监听")]
    public TransFormEventSO ReturnPlayerPoEvent;
    public VoidEventSO HitPlayerEvent;
    [Header("冰剑")]
    public GameObject Ice1;
    public GameObject Ice2;
    public GameObject Ice3;
    private void Awake()
    {
        IsSkill = false;
        iceState = new IceState();
        hideState = new HideState();
        walkState = new WalkState();
        surfState = new SurfState();
        ballattackState = new BallAttackState();
        defendState = new DefendState();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Bossanim>();
        health = GetComponent<BossHealth>();
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
            StateType.Defend => defendState,
            _ => null
        };
        CurrentState.OnExit(this);
        CurrentState = newstate;
        CurrentState.OnEnter(this);
    }
    private void BossWalk()
    {
        if (anim.IsWalk || anim.IsSurf)
        {
            var BossFace = transform.localScale.x;
            rb.velocity = new Vector2(BossFace * BossSpeed * Time.deltaTime, rb.velocity.y);
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
    public void OnAttack1z1Anim()
    {
        anim.OnAttack1z1();
        StartCoroutine(StartAttack1z2());
    }
    private IEnumerator StartAttack1z2()
    {
        yield return new WaitForSeconds(0.5f);
        if (isHitPlayer)
        {
            OnAttack1z2Anim();
        }
        else
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
    public void GetPlayerPo()
    {
        GetPlayerPoEvent.RaiseEvent();
    }
    private void ChooseState()
    {
        if (!isState2)
        {
            n = Random.Range(1, 3);
        }
        if (isState2)
        {
            n = Random.Range(3, 5);
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
            IsSkill = true;
            ChooseState();
        }
    }
    private void OnEnable()
    {
        ReturnPlayerPoEvent.OnRaiseTransFormEvent += OnReturnPlayerPo;
        HitPlayerEvent.OnRaiseEvent += OnHitPlayer;
    }

    private void OnHitPlayer()
    {
        isHitPlayer = true;
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
    public void ReturnHideState_Walk()
    {
        isHitPlayer = false;
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
        Ice1.SetActive(true);
        Ice2.SetActive(true);
        Ice3.SetActive(true);
        ChangeState(StateType.Hide);
    }
    private void SetDefend()
    {
        if (health.IsBeHurt)
        {
            BeHurtTime_Count -= Time.deltaTime;
        }
        if(health.BeHurtCount >= 2 && health.IsBeHurt)
        {
            health.IsBeHurt = false;
            ChangeState(StateType.Defend);
        }
        else if(BeHurtTime_Count < 0 && health.BeHurtCount < 2 && health.IsBeHurt)
        {
            health.IsBeHurt = false;
            health.BeHurtCount = 0;
            BeHurtTime_Count = BeHurtTime;
        }
    }
    private IEnumerator SetIceTimeFix()
    {
        yield return new WaitForSeconds(0.2f);
    }
    private void OnDisable()
    {
        ReturnPlayerPoEvent.OnRaiseTransFormEvent -= OnReturnPlayerPo;
        HitPlayerEvent.OnRaiseEvent -= OnHitPlayer;
    }
    public void State2()
    {
        MainMap.SetActive(false);
        isState2 = true;
        ChangeState(StateType.Hide);
    }
}
