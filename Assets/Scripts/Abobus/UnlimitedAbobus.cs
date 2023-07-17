using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnlimitedAbobus : Abobus
{
    
    public abstract Vector3[] GetBasisTurns();
    override public List<HexCoordinates> GetPossibleTurns(System.Func<HexCoordinates, HexCell.State> checker, HexCoordinates? from = null)
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
                HexCell.State hex_cell_state = checker(candidate);
                if (hex_cell_state == HexCell.State.out_of_bounds) {
                    continue_ = false;
                } else {
                    ans.Add(candidate);
                    if (hex_cell_state == HexCell.State.abobus) {
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
