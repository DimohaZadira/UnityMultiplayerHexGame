using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : UnlimitedAbobus
{
    private Vector3[] turns = 
           new Vector3[] {new Vector3( 1, -2,  1)
                        , new Vector3( 2, -1, -1)
                        , new Vector3( 1,  1, -2)
                        , new Vector3(-1,  2, -1)
                        , new Vector3(-2,  1,  1)
                        , new Vector3(-1, -1,  2)};
    
    override public Vector3[] GetBasisTurns() { 
        Debug.Log("Getting basis bishop turns");
        return turns; 
    }

}
