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

        // Получаем список клеток для возможного применения способности (уже исключены клетки с Kaymanch)
        List<HexCell> all_skill_trigger_cells = buffer.GetPossibleSkillTriggerTurns();
        
        List<HexCell> skill_turns = new List<HexCell>();

        // Подсвечиваем клетки, которые можно использовать для способности
        foreach (HexCell cell in all_skill_trigger_cells)
        {
            cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.highlighted_blue);
            cell.actions.AddLast(new SimpleMovement(cell, game_manager.GetAbobusByHexCoordinates(applied_to.hex_coordinates)));
            cell.actions.AddLast(new SimpleMovement(applied_to, buffer));
            cell.actions.AddLast(new SimpleUnhighlight(cell, all_skill_trigger_cells));
            cell.actions.AddLast(new EndTurn(cell));
        }

        buffer.cell.actions.AddLast(new ClearActions<IAction>(buffer.cell, all_skill_trigger_cells));
        buffer.cell.actions.AddLast(new ReturnHighlights(buffer.cell, buffer));
    }
}


        // {
        //     // if (cell.abobus == null || !(cell.abobus.GetComponent<Kaymanch>()))
        //     // if !(cell.abobus is Kaymanch)
        //     // if (cell.abobus == null || (cell.abobus.GetComponent<Kaymanch>()))
        //     // if (cell.abobus == null)
        //     // (cell.abobus.GetType() == typeof(Kaymanch)
        //     if (cell.abobus == null || !(cell.abobus.GetComponent<Kaymanch>() == null))
        //     {
        //          all_skill_trigger_cells.Add(cell);
        //     }
        // }