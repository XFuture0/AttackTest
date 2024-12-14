using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    [Header("�㲥")]
    public FloatEventSO AttackBossEvent;
    public VoidEventSO PlayerSpeedEvent;
    [Header("�˺�")]
    public float AttackCount;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Boss")
        {
            AttackBossEvent.RaiseFloatEvent(AttackCount);
            PlayerSpeedEvent.RaiseEvent();
        }
    }
}
