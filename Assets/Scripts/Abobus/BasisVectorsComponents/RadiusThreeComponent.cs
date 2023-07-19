using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusThreeComponent
{
    private Vector3[] turns = 
           new Vector3[] {new Vector3(-3,  0,  3)
                        , new Vector3( 0, -3,  3)
                        , new Vector3( 3, -3,  0)
                        , new Vector3( 3,  0, -3)
                        , new Vector3( 0,  3, -3)
                        , new Vector3(-3,  3,  0)};

    public Vector3[] GetBasisTurns() { 
        return turns; 
    }
}
