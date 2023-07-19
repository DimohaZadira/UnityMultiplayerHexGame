using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Abobus : MonoBehaviour
{

    public GayManager gay_manager;
    public GayManager.Team team;
    public HexCoordinates hex_coordinates;

    public abstract List<HexCoordinates> GetPossibleMovementTurns();
    public abstract List<HexCoordinates> GetPossibleSkillTriggerTurns();
    public abstract List<HexCoordinates> GetPossibleSkillTurns(HexCell from);
    public abstract void PerformSkill(HexCell from, HexCell to);
    // public abstract List<HexCoordinates> GetPossibleTurns();
    public abstract void Init();

    // states
    public AbobusIdleState idle_state;
    public AbobusChosenState chosen_state;
    public AbobusMovementState movement_state;
    public AbobusDisabledState disabled_state;
    public AbobusSkillPerformingState skill_performing_state;
    private List<AbobusState> states;
    public AbobusState state;
    public void React(HexCell? hex_cell = null)
    {
        state.HandleInput(hex_cell);
    }
    public void RefreshStates()
    {
        foreach (AbobusState state in states) {
            state.Refresh();
        }
    }

    public void Init(GayManager gm, GayManager.Team team_, HexCoordinates start_hc)
    {
        gay_manager = gm;
        team = team_;
        hex_coordinates = start_hc;
        MoveToHexCoordinates(start_hc);
        // states
        states = new List<AbobusState>();
        idle_state = new AbobusIdleState(gay_manager, this);
        states.Add(idle_state);
        chosen_state = new AbobusChosenState(gay_manager, this);
        states.Add(chosen_state);
        movement_state = new AbobusMovementState(gay_manager, this);
        states.Add(movement_state);
        disabled_state = new AbobusDisabledState(gay_manager, this);
        states.Add(disabled_state);
        skill_performing_state = new AbobusSkillPerformingState(gay_manager, this);
        states.Add(skill_performing_state);

        idle_state.Enter();
        Init();
    }

    public void MoveToHexCoordinates(HexCoordinates hc)
    {
        gay_manager.hex_grid.GetCellByHexCoordinates(hex_coordinates).state = HexCell.State.empty;
        hex_coordinates = hc;
        hc = HexCoordinates.ToOffsetCoordinates(hc.X, hc.Z);
        Vector3 from_hex_coords = HexCoordinates.FromHexCoordinates(hc);
        from_hex_coords.y = GetComponentInParent<Transform>().localPosition.y;
        GetComponentInParent<Transform>().localPosition = from_hex_coords;

        GetComponentInParent<Transform>().localRotation = Quaternion.Euler(0, 120, 0);

        gay_manager.hex_grid.GetCellByHexCoordinates(hex_coordinates).state = HexCell.State.abobus;
    }
    
}
