using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Abobus: MonoBehaviour 
{
    public Type action_type;
    public GameManager game_manager;
    public GameManager.Team team;
    public HexCell cell;

    public String abobus_name;
    public abstract List<HexCell> GetPossibleMovementTurns();
    public abstract List<HexCell> GetPossibleSkillTriggerTurns();
    public abstract List<HexCell> GetPossibleSkillTurns(HexCell from);
    
    public void Init(GameManager gm, GameManager.Team team_, HexCoordinates start_hc, String name)
    {
        abobus_name = name;
        game_manager = gm;
        team = team_;
        cell = game_manager.hex_grid.GetCellByHexCoordinates(start_hc);
        MoveToHexCoordinates(start_hc);
    }

    public void MoveToHexCoordinates(HexCoordinates hc)
    {
        HexCell old_cell = cell;
        cell = game_manager.hex_grid.GetCellByHexCoordinates(hc);
        hc = HexCoordinates.ToOffsetCoordinates(hc.X, hc.Z);
        Vector3 from_hex_coords = HexCoordinates.FromHexCoordinates(hc);
        from_hex_coords.y = GetComponentInParent<Transform>().localPosition.y;
        GetComponentInParent<Transform>().localPosition = from_hex_coords;

        GetComponentInParent<Transform>().localRotation = Quaternion.Euler(0, 120, 0);

        
        old_cell.Refresh();
        cell.Refresh();
    }
    
}
