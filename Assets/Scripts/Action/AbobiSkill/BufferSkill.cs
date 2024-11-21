using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BufferSkill : IAction
{
    private HexCell applied_to;
    private Buffer buffer;
    private GameManager game_manager;

    public BufferSkill(HexCell applied_to, Abobus buffer)
    {
        this.applied_to = applied_to;
        this.buffer = buffer.GetComponent<Buffer>();
        game_manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    public HexCell AppliedTo
    {
        get => applied_to;
        set => applied_to = value;
    }

    public string DebugMessage()
    {
        return "Buffer skill invoked";
    }

    public void Invoke()
    {
        Debug.Log("Buffer invokes skill");

        if (game_manager.selected_abobus == null)
        {
            SelectAbobus select_abobus = new SelectAbobus(buffer.cell, buffer);
            select_abobus.Invoke();
        }

        Abobus targetAbobus = applied_to.abobus;

        List<HexCell> skill_trigger_cells = targetAbobus.GetPossibleTurns(targetAbobus.cell, RangeOneComponent.GetBasisTurns(), HexCell.State.empty);

        foreach (HexCell cell in skill_trigger_cells)
        {
            if (cell.abobus == null)
            {
                cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.highlighted_yellow);
                cell.actions.AddLast(new SimpleMovement(cell, game_manager.GetAbobusByHexCoordinates(applied_to.hex_coordinates)));
                cell.actions.AddLast(new SimpleMovement(applied_to, buffer));
                cell.actions.AddLast(new SimpleUnhighlight(cell, skill_trigger_cells));
                cell.actions.AddLast(new EndTurn(cell));
            }
        }
        buffer.cell.actions.AddLast(new UnselectAbobus(buffer.cell, buffer));
        buffer.cell.actions.AddLast(new UnhighlightOneCell(targetAbobus.cell));
        buffer.cell.actions.AddLast(new SimpleUnhighlight(buffer.cell, skill_trigger_cells));
        buffer.cell.actions.AddLast(new ClearActions<IAction>(buffer.cell, skill_trigger_cells));
        buffer.cell.actions.AddLast(new ReturnHighlights(buffer.cell, buffer));
    }
}