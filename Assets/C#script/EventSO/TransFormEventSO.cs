using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Event/TransFormEvent")]

public class TransFormEventSO : ScriptableObject
{
    public UnityAction<Transform> OnRaiseTransFormEvent;
    public void RaiseTransFormEvent(Transform transform)
    {
        OnRaiseTransFormEvent?.Invoke(transform);
    }
}
