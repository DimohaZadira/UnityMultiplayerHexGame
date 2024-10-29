using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedverSkill : IAction
{
    private Abobus abobus1; // звероник которого выбираем
    private Abobus abobus2; // звероник на которого тыкаем
    private HexCell cell1; //хекс 1-ого звероника
    private HexCell cell2; //хекс 2-ого звероника
    private GameManager game_manager;
    public MedverSkill(Abobus abobus2, Abobus abobus1)
    {
        this.abobus1 = abobus1;
        this.abobus2 = abobus2;

        cell1 = abobus1.cell;
        cell2 = abobus2.cell;

        game_manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    public HexCell AppliedTo
    {
        get => cell1;
        set => cell1 = value;
    }

    public HexCell AppliedFrom
    {
        get => cell2;
        set => cell2 = value;
    }


    public string DebugMessage()
    {
        return "Medver skill";
    }

    public void Invoke()
    {
        Debug.Log("Medver invokes skill");
        if (game_manager.selected_abobus == null) {
            SelectAbobus select_abobus = new SelectAbobus(abobus1.cell, abobus1);
            select_abobus.Invoke();
        }
        List<HexCell> skill_turns = abobus1.GetPossibleSkillTurns(cell1);
        foreach (HexCell cell in skill_turns) 
        {
            cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.highlighted_yellow);
            cell.actions.AddLast(new Swap(abobus1, abobus2));
            cell.actions.AddLast(new SimpleUnhighlight(abobus1.cell, skill_turns));
            cell.actions.AddLast(new UnselectAbobus(cell, abobus1));
            cell.actions.AddLast(new EndTurn(cell));
        }
        abobus1.cell.actions.AddLast(new SimpleUnhighlight(abobus1.cell, skill_turns));
        abobus1.cell.actions.AddLast(new ClearActions<IAction>(abobus1.cell, skill_turns));
        abobus1.cell.actions.AddLast(new ReturnHighlights(abobus1.cell, abobus1));
    }
}