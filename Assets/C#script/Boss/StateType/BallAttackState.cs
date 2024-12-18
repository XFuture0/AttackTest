using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAttackState : State
{
    public override void OnEnter(MainBoss Boss)
    {
        Boss.GetPlayerPo();
        Boss.BossTurnFace();
        Boss.OnBallAttackAnim();
    }
    public override void LogicUpdate(MainBoss Boss)
    {

    }
    public override void PhysicsUpdate(MainBoss Boss)
    {

    }
    public override void OnExit(MainBoss Boss)
    {
        Boss.SkillTime_Count = Boss.SkillTime;
        Boss.IsSkill = false;
    }
}
