using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slong : Abobus
{
    private RangeOneComponent movement_cells;
    private RangeOneComponent skill_cells;
    override public void Init()
    {
        movement_cells = new RangeOneComponent();
        skill_cells = new RangeOneComponent();
    }
    override public List<HexCoordinates> GetPossibleMovementTurns()
    {
        List<HexCoordinates> ans = new List<HexCoordinates>();
        foreach (Vector3 turn in movement_cells.GetBasisTurns()) {
            HexCoordinates candidate = HexCoordinates.FromXY(hex_coordinates.X + (int)turn[0], hex_coordinates.Y + (int)turn[1]);
            
            if (!gay_manager.hex_grid.CheckHexCoordsOutOfBounds(candidate)) {
                HexCell cell_candidate = gay_manager.hex_grid.GetCellByHexCoordinates(candidate);
                if (cell_candidate.state == HexCell.State.empty) {
                    // cell_candidate.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.highlighted_green);
                    ans.Add(candidate);
                }
            }
        }
        // Debug.Log(ans);
        return ans;
    }
    override public List<HexCoordinates> GetPossibleSkillTurns()
    {
        List<HexCoordinates> ans = new List<HexCoordinates>();
        foreach (Vector3 turn in movement_cells.GetBasisTurns()) {
            HexCoordinates candidate = HexCoordinates.FromXY(hex_coordinates.X + (int)turn[0], hex_coordinates.Y + (int)turn[1]);
            
            if (!gay_manager.hex_grid.CheckHexCoordsOutOfBounds(candidate)) {
                HexCell cell_candidate = gay_manager.hex_grid.GetCellByHexCoordinates(candidate);
                if (cell_candidate.state == HexCell.State.abobus) {
                    // cell_candidate.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.highlighted_yellow);
                    ans.Add(candidate);
                }
            }
        }
        // Debug.Log(ans);
        return ans;
    }
    // override public List<HexCoordinates> GetPossibleTurns()
    // {
    //     List<HexCoordinates> ans = GetPossibleMovementTurns();
    //     ans.AddRange(GetPossibleSkillTurns());
    //     return ans;
    // }
}
