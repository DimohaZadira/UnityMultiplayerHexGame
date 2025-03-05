using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using TMPro;
using UnityEngine;

public class SohadSkill : IAction
{
    private HexCell applied_to;
    private Sohad sohad;
    private GameManager game_manager;
    public SohadSkill(HexCell applied_to, Sohad sohad)
    {
        this.applied_to = applied_to;
        this.sohad = sohad;
        game_manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }


    public HexCell AppliedTo
    {
        get => applied_to;
        set
        {
            applied_to = value;
        }
    }

    public string DebugMessage()
    {
        return "Sohad skill";
    }

    private static HighlightableCell HexCellToHighlightable(HexCell kek)
    {
        return kek.GetComponent<HighlightableCell>();
    }
    public void Invoke()
    {
        Debug.Log("Sohad invokes skill");
        HexCell before = sohad.cell;
        sohad.visited.Add(before);
        sohad.MoveToHexCoordinates(applied_to.hex_coordinates);
        List<HexCell> keks = sohad.GetPossibleSkillTriggerTurns();
        if (keks.Count == 0)
        {
            EndTurn end_turn = new EndTurn(applied_to);
            end_turn.Invoke();
            return;
        }
        HighlightCells highlight_action = new HighlightCells(applied_to, null, Utils.Map(keks, HexCellToHighlightable).ToList(), HighlightableCell.State.highlighted_yellow);
        highlight_action.Invoke();
        foreach (HexCell kek in keks)
        {
            kek.actions.AddLast(new SimpleUnhighlight(applied_to, null));
            kek.actions.AddLast(new HighlightCells(applied_to, null, new List<HighlightableCell>() { applied_to.GetComponent<HighlightableCell>() }, HighlightableCell.State.highlighted_blue));
            kek.actions.AddLast(new PerformSkill(kek, sohad));
        }
        applied_to.actions.AddLast(new UnhighlightOneCell(applied_to));
    }
}
