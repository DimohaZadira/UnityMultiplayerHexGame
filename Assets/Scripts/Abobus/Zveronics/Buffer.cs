using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffer : Abobus
{
    private RangeTwoComponent movement_cells;
    private RangeOneComponent skill_trigger_cells;
    private RangeOneComponent skill_cells;

    override public void Init()
    {
        movement_cells = new RangeTwoComponent();
        skill_trigger_cells = new RangeOneComponent();
        skill_cells = new RangeOneComponent();
    }
    override public bool PerformSkill(HexCell from, HexCell to)
    {
        Abobus to_move = gay_manager.GetAbobusByHexCoordinates(from.hex_coordinates);
        to_move.MoveToHexCoordinates(to.hex_coordinates);
        MoveToHexCoordinates(from.hex_coordinates);
        
        return true;
    }
    override public void PrePerformSkill(HexCell to) {}
    private List<HexCoordinates> GetPossibleTurns(HexCoordinates from,  Vector3[] basis_turns, HexCell.State check)
    {
        List<HexCoordinates> ans = new List<HexCoordinates>();

        foreach (Vector3 turn in basis_turns) {
            HexCoordinates candidate = HexCoordinates.FromXY(from.X + (int)turn[0], from.Y + (int)turn[1]);
            
            if (!gay_manager.hex_grid.CheckHexCoordsOutOfBounds(candidate)) {
                HexCell cell_candidate = gay_manager.hex_grid.GetCellByHexCoordinates(candidate);
                if (cell_candidate.state == check) {
                    ans.Add(candidate);
                }
            }
        }
        return ans;
    }

    override public List<HexCoordinates> GetPossibleMovementTurns()
    {
        return GetPossibleTurns(hex_coordinates, movement_cells.GetBasisTurns(), HexCell.State.empty);
    }

    override public List<HexCoordinates> GetPossibleSkillTriggerTurns()
    {
        return GetPossibleTurns(hex_coordinates, skill_trigger_cells.GetBasisTurns(), HexCell.State.abobus);
    }

    
    override public List<HexCoordinates> GetPossibleSkillTurns(HexCell from)
    {
        List<HexCoordinates> ans = GetPossibleTurns(from.hex_coordinates, skill_cells.GetBasisTurns(), HexCell.State.empty);
        ans.Add(hex_coordinates);
        return ans;
    }

}