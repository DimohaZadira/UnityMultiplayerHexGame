using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UncheckMovement : Action
{
    HexCell applied_to;
    public UncheckMovement (HexCell applied_to)
    {
        this.applied_to = applied_to;
    }
    public override void Invoke()
    {
        GameManager game_manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        Abobus abobus = game_manager.GetAbobusByHexCoordinates(applied_to.hex_coordinates);
        game_manager.ClearAllHighlightedCells();
        game_manager.ClearAllActions();
        game_manager.SetAllCheckActions();
        
    }
}
