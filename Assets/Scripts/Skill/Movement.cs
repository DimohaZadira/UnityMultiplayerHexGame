using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Movement : Action
{
    private GameManager game_manager;
    public override string ToString () {
		return "Moving " + abobus.abobus_name + " abobus from " + from.hex_coordinates.ToString() + " to " + to.hex_coordinates.ToString();
	}

    public Movement (Abobus abobus, HexCell move_to, HexCell move_from)
    {
        from = move_from;
        to = move_to;
        this.abobus = abobus;
        game_manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }
    private HexCell from, to;
    private Abobus abobus; 
    public override void Invoke()
    {
        game_manager.ClearAllHighlightedCells();
        game_manager.ClearAllActions();
        abobus.MoveToHexCoordinates(to.hex_coordinates);
    }
}
