using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected MainBoss boss;
    public abstract void OnEnter(MainBoss Boss);
    public abstract void LogicUpdate(MainBoss Boss);
    public abstract void PhysicsUpdate(MainBoss Boss);
    public abstract void OnExit(MainBoss Boss);
}
