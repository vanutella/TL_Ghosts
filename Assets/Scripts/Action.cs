using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action
{
    public virtual void Start() { }
    public virtual void Update() { }
    public virtual bool IsFinished() { return true; }
}
