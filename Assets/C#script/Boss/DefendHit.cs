using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DefendHit : MonoBehaviour
{
    [Header("¹¥»÷µÆ¹â")]
    public float LightTime;
    public float MaxLight;
    [HideInInspector] public float LightTime_Count;
    public Light2D AttackLight;
    private bool IsHit;
    private bool CanHit;
    [Header("¹ã²¥")]
    public FloatEventSO AttackPlayerEvent;
    public TransFormEventSO DefendHitEvent;
    [Header("BossÉËº¦")]
    public float BossAttackCount;
    private void Awake()
    {
        LightTime_Count = 0;
    }
    private void OnEnable()
    {
        CanHit = true;
    }
    private void Update()
    {
        if (LightTime_Count > 0)
        {
            LightTime_Count -= Time.deltaTime;
        }
        if (LightTime_Count <= 0 && IsHit == true)
        {
            IsHit = false;
            AttackLight.intensity = 1;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (CanHit)
            {
                IsHit = true;
                CanHit = false;
                var Count = (int)Random.Range(BossAttackCount, BossAttackCount * 1.2f);
                DefendHitEvent.RaiseTransFormEvent(transform.parent.parent.transform);
                AttackPlayerEvent.RaiseFloatEvent(Count);
                AttackLight.intensity = MaxLight;
                LightTime_Count = LightTime;
            }
        }
    }
    private void OnDisable()
    {
        CanHit = false;
    }
}
