using System;
using System.Collections.Generic;
using UnityEngine;

public class LevverSkillMovement : IAction
{
    private HexCell applied_to;
    private Abobus abobus;
    private GameManager game_manager;

    public LevverSkillMovement(HexCell applied_to, Abobus abobus)
    {
        this.applied_to = applied_to;
        this.abobus = abobus;
        game_manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    public void Invoke()
    {
        Debug.Log("Levver invokes movement skill");

        if (game_manager.selected_abobus == null)
        {
            SelectAbobus select_abobus = new SelectAbobus(abobus.cell, abobus);
            select_abobus.Invoke();
        }

        List<HexCell> skill_turns = abobus.GetPossibleSkillTurns(applied_to);

        foreach (HexCell cell in skill_turns)
        {
            if (HexGrid.GetDistance(abobus.cell, cell) <= 1 && cell.state == HexCell.State.empty)
            {
                cell.state = HexCell.State.empty; // Используем состояние "empty" как "highlighted_green"
                cell.actions.AddLast(new SimpleMovement(cell, abobus));
                cell.actions.AddLast(new SimpleUnhighlight(cell, skill_turns));
                cell.actions.AddLast(new UnselectAbobus(cell, abobus));
            }
        }

        abobus.cell.actions.AddLast(new SimpleUnhighlight(abobus.cell, skill_turns));
        abobus.cell.actions.AddLast(new ClearActions<IAction>(abobus.cell, skill_turns));
        abobus.cell.actions.AddLast(new ReturnHighlights(abobus.cell, abobus));
        abobus.cell.actions.AddLast(new EndTurn(abobus.cell));
    }
}