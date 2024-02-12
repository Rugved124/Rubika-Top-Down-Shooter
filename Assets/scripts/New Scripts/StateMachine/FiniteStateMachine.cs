using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class FiniteStateMachine : MonoBehaviour
{
    private Dictionary<Type, BaseState> availableStates;

    public BaseState currentState { get; private set; }

    public void SetStates(Dictionary<Type, BaseState> states)
    {
        availableStates = states;
    }

    private void Update()
    {
        if (currentState == null)
        {
            currentState = availableStates.Values.First();
        }
        else
        {
            UpdateState();
        }
    }

    void SwitchToNextState(Type nextState)
    {
        currentState = availableStates[nextState];
        currentState.EnterState();
    }

    void UpdateState()
    {
        Type nextState = currentState.ExecuteState();

        if (nextState != null && nextState != currentState.GetType())
        {
            SwitchToNextState(nextState);
        }
    }

    
}
