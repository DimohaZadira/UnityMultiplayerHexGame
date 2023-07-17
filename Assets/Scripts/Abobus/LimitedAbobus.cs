using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LimitedAbobus : Abobus
{
    private Vector3[] turns;

    public abstract Vector3[] GetBasisTurns();
    override public List<HexCoordinates> GetPossibleTurns(System.Func<HexCoordinates, HexCell.State> checker, HexCoordinates? from = null)
    {
        HexCoordinates param = hex_coordinates;
        if (from is HexCoordinates hc) {
            param = hc;
        }
        List<HexCoordinates> ans = new List<HexCoordinates>();
        foreach (Vector3 turn in GetBasisTurns()) {
            HexCoordinates candidate = HexCoordinates.FromXY(param.X + (int)turn[0], param.Y + (int)turn[1]);
            HexCell.State hex_cell_state = checker(candidate);
            if (hex_cell_state != HexCell.State.out_of_bounds) {
                ans.Add(candidate);
            } 
        }
        // Debug.Log(ans);
        return ans;
    }

}
