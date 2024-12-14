using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideState : State
{
    public override void OnEnter(MainBoss Boss)
    {
        Boss.OnHideAnim();
    }
    public override void LogicUpdate(MainBoss Boss)
    {
        
    }
    public override void PhysicsUpdate(MainBoss Boss)
    {
        
    }
    public override void OnExit(MainBoss Boss)
    {
        Boss.OnHideAnim();
    }
}
