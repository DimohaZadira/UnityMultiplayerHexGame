using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnlimitedAbobus : Abobus
{
    
    public abstract Vector3[] GetBasisTurns();
    override public List<HexCoordinates> GetPossibleTurns(System.Func<HexCoordinates,GayManager.HexCellState> checker, HexCoordinates? from = null)
    {
        HexCoordinates param = hex_coordinates;
        if (from is HexCoordinates hc) {
            param = hc;
        }
        List<HexCoordinates> ans = new List<HexCoordinates>();
        foreach (Vector3 turn in GetBasisTurns()) {
            int multiplier = 1;
            bool continue_ = true;
            while (continue_) {
                HexCoordinates candidate = HexCoordinates.FromXY(param.X + multiplier * (int)turn[0], param.Y + multiplier * (int)turn[1]);
                GayManager.HexCellState hex_cell_state = checker(candidate);
                if (hex_cell_state == GayManager.HexCellState.out_of_bounds) {
                    continue_ = false;
                } else {
                    ans.Add(candidate);
                    if (hex_cell_state == GayManager.HexCellState.abobus) {
                        continue_ = false;
                    }
                }
                
                multiplier += 1;
            }
            
        }
        // Debug.Log(ans);
        return ans;
    }

}
