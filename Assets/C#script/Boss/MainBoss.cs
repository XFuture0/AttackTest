using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class MainBoss : MonoBehaviour
{
    private Bossanim anim;
    [HideInInspector]public Rigidbody2D rb;
    [HideInInspector]public Transform Player;
    [Header("状态")]
    private int n;
    private State CurrentState;
    private State hideState;
    private State walkState;
    private State iceState;
    [Header("Boss属性")]
    public float BossSpeed;
    public bool isAttack1z1;
    [Header("技能计时器")]
    [HideInInspector]public bool IsSkill;
    [HideInInspector]public bool isHitPlayer;
    public float SkillTime;
    [HideInInspector]public float SkillTime_Count;
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
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Bossanim>();
        SkillTime_Count = SkillTime;
    }
    private void Start()
    {
        n = Random.Range(1, 3);
        CurrentState = hideState;
        CurrentState.OnEnter(this);
    }
    private void Update()
    {
        if (!IsSkill)
        {
            SkillTime_Count -= Time.deltaTime;
        }
        if(SkillTime_Count <= 0 && !IsSkill)
        {
            IsSkill = true;
            ChooseState();
        }
        CurrentState.LogicUpdate(this);
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
            _ => null
        };
        CurrentState.OnExit(this);
        CurrentState = newstate;
        CurrentState.OnEnter(this);
    }
    private void BossWalk()
    {
        if (anim.IsWalk)
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
    public void OnAttack1z2Anim()
    {
        anim.OnAttack1z2();
    }
    public void Skill_1()
    {
        GetPlayerPoEvent.RaiseEvent();
    }
    private void ChooseState()
    {
        switch (n)
        {
            case 1:
                ChangeState(StateType.Walk);
                break;
            case 2:
                ChangeState(StateType.Ice);
                break;
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
    public void ReturnHideState()
    {
        isHitPlayer = false;
        OnWalkAnim();
        ChangeState(StateType.Hide);
    }
    public void SetIce()
    {
        ChangeState(StateType.Hide);
    }
    private void OnDisable()
    {
        ReturnPlayerPoEvent.OnRaiseTransFormEvent -= OnReturnPlayerPo;
        HitPlayerEvent.OnRaiseEvent -= OnHitPlayer;
    }
}
