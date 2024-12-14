using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Event/FloatEvent")]
public class FloatEventSO : ScriptableObject
{
    public UnityAction<float> OnRaiseFloatEvent;
    public void RaiseFloatEvent(float Float)
    {
        OnRaiseFloatEvent?.Invoke(Float);
    }
}
