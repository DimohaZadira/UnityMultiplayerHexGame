using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : Skill
{
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
