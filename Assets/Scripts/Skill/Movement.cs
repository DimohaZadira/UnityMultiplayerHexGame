using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : Action
{
    public override string ToString () {
		return "Moving " + abobus.abobus_name + " abobus from " + from.hex_coordinates.ToString() + " to " + to.hex_coordinates.ToString();
	}

    public Movement (Abobus abobus, HexCell move_to, HexCell move_from)
    {
        from = move_from;
        to = move_to;
        this.abobus = abobus;
    }
    private HexCell from, to;
    private Abobus abobus; 
    public override void Invoke()
    {
        abobus.MoveToHexCoordinates(to.hex_coordinates);
    }
}
