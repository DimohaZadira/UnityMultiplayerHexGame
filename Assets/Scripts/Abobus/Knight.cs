using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Abobus
{
    private Vector3[] turns = 
           new Vector3[] {new Vector3(-2, -1,  3)
                        , new Vector3(-1, -2,  3)
                        , new Vector3( 1, -3,  2)
                        , new Vector3( 2, -3,  1)
                        , new Vector3( 3, -2, -1)
                        , new Vector3( 3, -1, -2)
                        , new Vector3( 2,  1, -3)
                        , new Vector3( 1,  2, -3)
                        , new Vector3(-1,  3, -2)
                        , new Vector3(-1,  2, -1)
                        , new Vector3(-3,  2,  1)
                        , new Vector3(-3,  1,  2)};


    override public List<HexCoordinates> GetPossibleTurns(HexCoordinates? from)
    {
        HexCoordinates param = hex_coordinates;
        if (from is HexCoordinates hc) {
            param = hc;
        }
        List<HexCoordinates> ans = new List<HexCoordinates>();
        foreach (Vector3 turn in turns) {
            ans.Add(HexCoordinates.FromXY(param.X + (int)turn[0], param.Y + (int)turn[1]));
        }
        // Debug.Log(ans);
        return ans;
    }

}
