using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SurfState : State
{
    public override void OnEnter(MainBoss Boss)
    {
        Boss.OnSurfAnim();
        Boss.GetPlayerPo();
        Boss.BossTurnFace();
    }
    public override void LogicUpdate(MainBoss Boss)
    {
        Boss.GetPlayerPo();
        if (math.abs(Boss.transform.position.x - Boss.Player.position.x) < 2)
        {
            if (!Boss.isAttack2)
            {
                Boss.isAttack2 = true;
                Boss.rb.velocity = Vector2.zero;
                Boss.OnSurfAnim();
                Boss.OnAttack2Anim();
            }
        }
    }
    public override void PhysicsUpdate(MainBoss Boss)
    {

    }
    public override void OnExit(MainBoss Boss)
    {
        Boss.OnSurfAnim();
        Boss.SkillTime_Count = Boss.SkillTime;
        Boss.IsSkill = false;
        Boss.isAttack2 = false;
    }
}
