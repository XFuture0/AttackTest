using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    [Header("π„≤•")]
    public FloatEventSO AttackBossEvent;
    public VoidEventSO PlayerSpeedEvent;
    [Header("…À∫¶")]
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
