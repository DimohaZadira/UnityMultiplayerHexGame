using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MedverSkill : IAction
{
    private HexCell applied_to;
    private Medver initial_abobus;
    private GameManager game_manager;
    public MedverSkill(HexCell applied_to, Abobus initial_abobus)
    {
        this.applied_to = applied_to;
        this.initial_abobus = initial_abobus.GetComponent<Medver>();
        game_manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    public HexCell AppliedTo
    {
        get => applied_to;
        set => applied_to = value;
    }


    public string DebugMessage()
    {
        return "Medver skill";
    }


    private List<HexCell> GetNearbyIncludingSelf(HexCell from)
    {
        List<HexCell> ans = GetNearby(from);
        ans.Add(from);
        return ans;
    }
    private List<HexCell> GetNearby(HexCell from)
    {
        List<HexCell> ans = initial_abobus.GetPossibleTurns(from, RangeOneComponent.GetBasisTurns(), HexCell.State.abobus);
        return ans;
    }

    public void Invoke()
    {
        Debug.Log("Medver invokes skill");

        Abobus selected_abobus = applied_to.abobus;

        if (game_manager.selected_abobus == null)
        {
            SelectAbobus select_abobus = new SelectAbobus(applied_to, game_manager.GetAbobusByHexCoordinates(applied_to.hex_coordinates));
            select_abobus.Invoke();
        }
        else
        {
            throw new System.Exception("Не отработала чистка в PerformAction");
        }

        List<HexCell> initial_nearby_abobi = GetNearbyIncludingSelf(initial_abobus.cell);
        List<HexCell> selected_nearby_abobi = GetNearby(selected_abobus.cell);


        List<HexCell> skill_turns = new List<HexCell>();

        foreach (HexCell cell in initial_nearby_abobi)
        {
            if (selected_nearby_abobi.Contains(cell))
            {
                skill_turns.Add(cell);
            }
        }

        foreach (HexCell cell in skill_turns)
        {
            cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.highlighted_yellow);
            cell.actions.AddLast(new Swap(cell, selected_abobus));
            cell.actions.AddLast(new SimpleUnhighlight(cell, skill_turns));
            cell.actions.AddLast(new EndTurn(cell));
        }
        selected_abobus.cell.actions.AddLast(new SimpleUnhighlight(selected_abobus.cell, skill_turns));
        selected_abobus.cell.actions.AddLast(new MedverClearActions<IAction>(selected_abobus.cell, initial_nearby_abobi));
        selected_abobus.cell.actions.AddLast(new ReturnHighlights(initial_abobus.cell, initial_abobus));
        //selected_abobus.cell.actions.AddLast(new SelectAbobus(initial_abobus.cell, initial_abobus));
    }
}
