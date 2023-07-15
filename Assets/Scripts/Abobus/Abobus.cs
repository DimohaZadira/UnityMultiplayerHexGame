using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Abobus : MonoBehaviour
{
    // у пешки да, у ладьи нет
    public bool limited_turns;

    public GayManager.Teams team;
    private static Vector3[] turns;
    public HexCoordinates hex_coordinates;


    public abstract List<HexCoordinates> GetPossibleTurns(System.Func<HexCoordinates,GayManager.HexCellState> checker, HexCoordinates? from = null);
    
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

    public void MoveToHexCoordinates(HexCoordinates hc)
    {
        hex_coordinates = hc;
        hc = HexCoordinates.ToOffsetCoordinates(hc.X, hc.Z);
        GetComponentInParent<Transform>().localPosition = HexCoordinates.FromHexCoordinates(hc);

        GetComponentInParent<Transform>().localRotation = Quaternion.Euler(0, 120, 0);
    }

    public void ChangeState(InputAction.CallbackContext? value = null)
    {
        state.HandleInput(this, value);
    }
}
