using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Abobus : MonoBehaviour
{

    public GameManager game_manager;
    public GameManager.Team team;
    public HexCoordinates hex_coordinates;

    public String abobus_name;
    public abstract void RefreshSelf();
    public abstract List<HexCoordinates> GetPossibleMovementTurns();
    public abstract List<HexCoordinates> GetPossibleSkillTriggerTurns();
    public abstract List<HexCoordinates> GetPossibleSkillTurns(HexCell from);
    public abstract bool PerformSkill(HexCell from, HexCell to);
    // public abstract List<HexCoordinates> GetPossibleTurns();
    public bool perform_skill_on_enter = false;

    // states
    public AbobusIdleState idle_state;
    public AbobusSelectedState selected_state;
    public AbobusMovementState movement_state;
    public AbobusDisabledState disabled_state;
    public AbobusSkillPerformingState skill_performing_state;
    private List<AbobusState> states;
    private AbobusState state;
    public AbobusState GetState() {
        return state;
    }
    public void React(HexCell hex_cell = null)
    {
        state.HandleInput(hex_cell);
    }
    public void Refresh()
    {
        RefreshStates();
        RefreshSelf();
    }
    public void RefreshStates()
    {
        foreach (AbobusState state in states) {
            state.Refresh();
        }
    }
    public void SwitchState(AbobusState state_, bool force_switch = false)
    {
        if (!force_switch && state == disabled_state) {
            return;
        }
        Debug.Log("Switching from <color=yellow>"+ state.class_name + "</color> to <color=yellow>" + state_.class_name + "</color>", this);
        state.Exit();
        state = state_;
        state.Enter();
        
    }

    public void Init(GameManager gm, GameManager.Team team_, HexCoordinates start_hc, String name)
    {
        abobus_name = name;
        game_manager = gm;
        team = team_;
        hex_coordinates = start_hc;
        MoveToHexCoordinates(start_hc);
        // states
        states = new List<AbobusState>();
        idle_state = new AbobusIdleState(game_manager, this);
        states.Add(idle_state);
        selected_state = new AbobusSelectedState(game_manager, this);
        states.Add(selected_state);
        movement_state = new AbobusMovementState(game_manager, this);
        states.Add(movement_state);
        disabled_state = new AbobusDisabledState(game_manager, this);
        states.Add(disabled_state);
        skill_performing_state = new AbobusSkillPerformingState(game_manager, this);
        states.Add(skill_performing_state);

        state = disabled_state;
    }

    public void MoveToHexCoordinates(HexCoordinates hc)
    {
        HexCoordinates old_coords = hex_coordinates;
        hex_coordinates = hc;
        hc = HexCoordinates.ToOffsetCoordinates(hc.X, hc.Z);
        Vector3 from_hex_coords = HexCoordinates.FromHexCoordinates(hc);
        from_hex_coords.y = GetComponentInParent<Transform>().localPosition.y;
        GetComponentInParent<Transform>().localPosition = from_hex_coords;

        GetComponentInParent<Transform>().localRotation = Quaternion.Euler(0, 120, 0);

        
        game_manager.RefreshHexCellState(old_coords);
        game_manager.RefreshHexCellState(hex_coordinates);
    }
    
}
