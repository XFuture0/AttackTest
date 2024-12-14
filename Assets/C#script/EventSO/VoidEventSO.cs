using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Event/VoidEventSO")]
public class VoidEventSO : ScriptableObject
{
    public UnityAction OnRaiseEvent;
    public void RaiseEvent()
    {
        OnRaiseEvent?.Invoke();
    }
}
