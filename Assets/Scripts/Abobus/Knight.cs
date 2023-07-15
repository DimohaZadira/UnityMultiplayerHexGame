using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : LimitedAbobus
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
                        , new Vector3(-2,  3, -1)
                        , new Vector3(-3,  2,  1)
                        , new Vector3(-3,  1,  2)};

    override public Vector3[] GetBasisTurns() { 
        Debug.Log("Getting basis knight turns");
        return turns; 
    }
}
