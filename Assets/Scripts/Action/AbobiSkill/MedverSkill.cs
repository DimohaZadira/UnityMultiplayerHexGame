using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MedverSkill : IAction
{
    private HexCell applied_to; // звероник которого выбираем
    private Abobus initialAbobus;
    private Abobus targetAbobus;
    private GameManager game_manager;
    public MedverSkill(HexCell applied_to, Abobus initialAbobus)
    {
        this.applied_to = applied_to;
        this.initialAbobus = initialAbobus;
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

    public void Invoke()
    {
        Debug.Log("Medver invokes skill");

        if (game_manager.selected_abobus == null) {
            SelectAbobus select_abobus = new SelectAbobus(applied_to, game_manager.GetAbobusByHexCoordinates(applied_to.hex_coordinates));
            select_abobus.Invoke();
        }

        Abobus initialAbobus = game_manager.GetAbobusByHexCoordinates(applied_to.hex_coordinates);

        List<HexCell> skillTurns = initialAbobus.GetPossibleSkillTurns(initialAbobus.cell);

        foreach (HexCell cell in skillTurns)
        {
            cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.highlighted_blue);

            Abobus targetAbobus = game_manager.GetAbobusByHexCoordinates(cell.hex_coordinates);
            
            cell.actions.AddFirst(new Swap(cell, initialAbobus, targetAbobus));
            cell.actions.AddLast(new SimpleUnhighlight(applied_to, skillTurns));
            cell.actions.AddLast(new UnselectAbobus(cell, initialAbobus));
            cell.actions.AddLast(new EndTurn(cell));

            initialAbobus.cell.actions.AddLast(new ClearActions<IAction>(initialAbobus.cell, skillTurns));
            
        }
    }
}
