using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Abobus : MonoBehaviour
{
    // у пешки да, у ладьи нет
    public bool limited_turns;

    public GayManager gay_manager;
    public GayManager.Teams team;
    private static Vector3[] turns;
    public HexCoordinates hex_coordinates;

    public abstract List<HexCoordinates> GetPossibleMovementTurns();
    public abstract List<HexCoordinates> GetPossibleSkillTurns();
    // public abstract List<HexCoordinates> GetPossibleTurns();
    public abstract void Init();

    // states
    public AbobusIdleState idle_state;
    public AbobusChosenState chosen_state;
    public AbobusMovementState movement_state;
    public AbobusState state;

    public void Init(GayManager gm, GayManager.Teams team_, HexCoordinates start_hc)
    {
        gay_manager = gm;
        team = team_;
        hex_coordinates = start_hc;
        MoveToHexCoordinates(start_hc);
        idle_state = new AbobusIdleState(gay_manager, this);
        chosen_state = new AbobusChosenState(gay_manager, this);
        movement_state = new AbobusMovementState(gay_manager, this);
        // disabled_state = new AbobusDisabledState(gay_manager, this);
        idle_state.Enter();
        Init();
    }

    public void MoveToHexCoordinates(HexCoordinates hc)
    {
        gay_manager.hex_grid.GetCellByHexCoordinates(hex_coordinates).state = HexCell.State.empty;
        hex_coordinates = hc;
        hc = HexCoordinates.ToOffsetCoordinates(hc.X, hc.Z);
        GetComponentInParent<Transform>().localPosition = HexCoordinates.FromHexCoordinates(hc);

        GetComponentInParent<Transform>().localRotation = Quaternion.Euler(0, 120, 0);

        gay_manager.hex_grid.GetCellByHexCoordinates(hex_coordinates).state = HexCell.State.abobus;
    }
    
}
