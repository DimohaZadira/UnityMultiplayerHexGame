using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MedverSkill : IAction
{
    private HexCell applied_to;
    private Medver initialAbobus;
    private GameManager game_manager;
    public MedverSkill(HexCell applied_to, Abobus initialAbobus)
    {
        this.applied_to = applied_to;
        this.initialAbobus = initialAbobus.GetComponent<Medver>();
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
        List<HexCell> ans = initialAbobus.GetPossibleTurns(from, RangeOneComponent.GetBasisTurns(), HexCell.State.abobus);
        return ans;
    }

    public void Invoke()
    {
        Debug.Log("Medver invokes skill");

        Abobus selectedAbobus = applied_to.abobus;

        if (game_manager.selected_abobus == null)
        {
            SelectAbobus select_abobus = new SelectAbobus(applied_to, game_manager.GetAbobusByHexCoordinates(applied_to.hex_coordinates));
            select_abobus.Invoke();
        }
        else
        {
            throw new System.Exception("Не отработала чистка в PerformAction");
        }

        List<HexCell> initialNearbyAbobi = GetNearbyIncludingSelf(initialAbobus.cell);
        List<HexCell> selectedNearbyAbobi = GetNearby(selectedAbobus.cell);


        List<HexCell> skillTurns = new List<HexCell>();

        foreach (HexCell cell in initialNearbyAbobi)
        {
            if (selectedNearbyAbobi.Contains(cell))
            {
                skillTurns.Add(cell);
            }
        }

        foreach (HexCell cell in skillTurns)
        {
            cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.highlighted_yellow);
            cell.actions.AddLast(new Swap(cell, selectedAbobus));
            cell.actions.AddLast(new SimpleUnhighlight(cell, skillTurns));
            cell.actions.AddLast(new EndTurn(cell));

        }
    }
}
