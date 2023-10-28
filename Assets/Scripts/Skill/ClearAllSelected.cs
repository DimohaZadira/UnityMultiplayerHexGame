using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearAllSelected : Action
{
    HexCell applied_to;
    GameManager game_manager;
    public ClearAllSelected (HexCell applied_to)
    {
        this.applied_to = applied_to;
        game_manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }
    public override void Invoke()
    {
        // Abobus abobus = game_manager.GetAbobusByHexCoordinates(applied_to.hex_coordinates);
        game_manager.ClearAllHighlightedCells();
        game_manager.ClearAllActions();
        game_manager.SetAllCheckActions();
        
    }
}
