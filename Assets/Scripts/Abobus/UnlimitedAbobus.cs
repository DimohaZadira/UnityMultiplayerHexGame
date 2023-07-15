using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnlimitedAbobus : Abobus
{
    
    public abstract Vector3[] GetBasisTurns();
    override public List<HexCoordinates> GetPossibleTurns(System.Func<HexCoordinates,bool> checker, HexCoordinates? from = null)
    {
        HexCoordinates param = hex_coordinates;
        if (from is HexCoordinates hc) {
            param = hc;
        }
        List<HexCoordinates> ans = new List<HexCoordinates>();
        foreach (Vector3 turn in GetBasisTurns()) {
            int multiplier = 1;
            bool not_out_of_bounds = true;
            while (not_out_of_bounds) {
                HexCoordinates candidate = HexCoordinates.FromXY(param.X + multiplier * (int)turn[0], param.Y + multiplier * (int)turn[1]);
                if (checker(candidate)) {
                    ans.Add(candidate);
                } else {
                    not_out_of_bounds = false;
                }
                multiplier += 1;
            }
            
        }
        // Debug.Log(ans);
        return ans;
    }

}
