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
    
    public void Refresh()
    {
        RefreshSelf();
    }

    public void Init(GameManager gm, GameManager.Team team_, HexCoordinates start_hc, String name)
    {
        abobus_name = name;
        game_manager = gm;
        team = team_;
        hex_coordinates = start_hc;
        MoveToHexCoordinates(start_hc);
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
