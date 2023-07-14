using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Abobus : MonoBehaviour
{
    private static Vector3[] turns;
    public HexCoordinates hex_coordinates;

    public abstract List<HexCoordinates> GetPossibleTurns(HexCoordinates? from = null);
    
    // states
    public AbobusIdleState idle_state;
    public AbobusChosenState chosen_state;
    public AbobusState state;

    public void InitStates()
    {
        idle_state = new AbobusIdleState();
        chosen_state = new AbobusChosenState();
        state = idle_state;
    }
    void Awake()
    {
        InitStates();
        // state.Meow();
    }

    public void ChangeState(InputAction.CallbackContext? value = null)
    {
        state.HandleInput(this, value);
    }
}
