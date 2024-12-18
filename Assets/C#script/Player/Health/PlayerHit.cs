using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerHit : MonoBehaviour
{
    [Header("�����ƹ�")]
    public float LightTime;
    [HideInInspector]public float LightTime_Count;
    public Light2D AttackLight;
    private bool IsHit;
    [Header("�㲥")]
    public FloatEventSO AttackBossEvent;
    public VoidEventSO PlayerSpeedEvent;
    [Header("�˺�")]
    public float AttackCount;
    private void Awake()
    {
        LightTime_Count = 0;
    }
    private void Update()
    {
        if (LightTime_Count > 0)
        {
            LightTime_Count -= Time.deltaTime;
        }
        if(LightTime_Count <= 0 && IsHit == true)
        {
            IsHit = false;
            AttackLight.intensity = 1;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Boss")
        {
            IsHit = true;
            AttackBossEvent.RaiseFloatEvent(AttackCount);
            PlayerSpeedEvent.RaiseEvent();
            AttackLight.intensity = 1.5f;
            LightTime_Count = LightTime;
        }
    }
}
