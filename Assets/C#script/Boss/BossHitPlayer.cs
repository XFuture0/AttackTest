using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitPlayer : MonoBehaviour
{
    [Header("¹ã²¥")]
    public VoidEventSO HitPlayerEvent;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            HitPlayerEvent.RaiseEvent();
        }
    }
}
