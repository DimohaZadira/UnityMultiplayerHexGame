using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbobusChosenState : AbobusState
{
    public AbobusChosenState(GayManager gm, Abobus abobus_) : base(gm, abobus_) {}
    override public void Enter()
    {
        if (abobus.state == abobus.idle_state) {
            abobus.transform.position += new Vector3(0, 10, 0);
        }
        List<HexCoordinates> coords_list = abobus.GetPossibleTurns(gay_manager.CellCheck);

        foreach (HexCoordinates hex_coords in coords_list) {
            HexCell hex_cell = gay_manager.hex_grid.GetCellByHexCoordinates(hex_coords);

            HighlightableCell.States state = gay_manager.SolveHighlightStateForHexCoordinates(hex_coords);
            hex_cell.GetComponent<HighlightableCell>().SetState(state);
        }

        abobus.state = abobus.chosen_state;
    }
    
    override public void HandleInput(HexCell hex_cell = null)
    {
        abobus.idle_state.Enter();
        abobus.MoveToHexCoordinates(hex_cell.hex_coordinates);
    }
    // override public void Meow() 
    // {
    //     Debug.Log("Meow from chosen state");
    // }

}