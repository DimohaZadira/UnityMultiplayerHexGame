using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slong : Abobus
{
    private RangeOneComponent movement_cells;
    private RangeOneComponent skill_trigger_cells;
    private RadiusThreeComponent skill_cells;

    override public void Init()
    {
        movement_cells = new RangeOneComponent();
        skill_trigger_cells = new RangeOneComponent();
        skill_cells = new RadiusThreeComponent();
    }
    override public void PerformSkill(HexCell from, HexCell to)
    {
        Debug.Log("Moving zveronic from " + from.hex_coordinates.ToString() + " to " + to.hex_coordinates.ToString());
    }
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
        return GetPossibleTurns(from.hex_coordinates, skill_cells.GetBasisTurns(), HexCell.State.empty);
    }

}
