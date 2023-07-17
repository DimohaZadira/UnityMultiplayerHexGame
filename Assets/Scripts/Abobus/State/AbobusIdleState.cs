using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbobusIdleState : AbobusState
{
    public AbobusIdleState(GayManager gm, Abobus abobus_) : base(gm, abobus_) {}
    override public void Enter() 
    {
        if (abobus.state == abobus.chosen_state) {
            // возвращаем клетки, которые были выбраны, в дефолтное состояние
            List<HexCoordinates> coords_list = abobus.GetPossibleTurns(gay_manager.CellCheck);

            foreach (HexCoordinates hex_coords in coords_list) {
                HexCell hex_cell = gay_manager.hex_grid.GetCellByHexCoordinates(hex_coords);
                hex_cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.States.default_);
            }
            abobus.transform.position += new Vector3(0, -10, 0);
        }
        abobus.state = abobus.idle_state;
    }
    
    override public void HandleInput(HexCell hex_cell = null)
    {
        abobus.chosen_state.Enter();
    }

}