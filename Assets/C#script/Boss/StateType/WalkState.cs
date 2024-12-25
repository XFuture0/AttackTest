using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WalkState : State
{
    public override void OnEnter(MainBoss Boss)
    {
        Boss.GetPlayerPo();
        Boss.BossTurnFace();
        Boss.OnWalkAnim();
    }
    public override void LogicUpdate(MainBoss Boss)
    {
        Boss.GetPlayerPo();
        if(math.abs(Boss.transform.position.x - Boss.Player.position.x) < 2)
        {
            if (!Boss.isAttack1z1)
            {
                Boss.isAttack1z1 = true;
                Boss.rb.velocity = Vector2.zero;
                Boss.OnAttack1z1Anim();
            }
        }
    }
    public override void PhysicsUpdate(MainBoss Boss)
    {

    }
    public override void OnExit(MainBoss Boss)
    {
        Boss.SkillTime_Count = Boss.SkillTime;
        Boss.IsSkill = false;
        Boss.isAttack1z1 = false;
        Boss.OnWalkAnim();
    }
}
