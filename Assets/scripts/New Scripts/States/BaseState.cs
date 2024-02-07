using System;
using UnityEngine;

public abstract class BaseState
{
    public GameObject gameObject { get; private set; }
    protected Transform transform;

    public BaseState(GameObject gameObject)
    {
        this.gameObject = gameObject;
        this.transform = gameObject.transform;

    }

    public abstract void EnterState();
    public abstract Type ExecuteState();
}
