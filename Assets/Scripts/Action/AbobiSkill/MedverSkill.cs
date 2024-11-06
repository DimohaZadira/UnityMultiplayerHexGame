using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MedverSkill : IAction
{
    private HexCell applied_to; 
    private Abobus initialAbobus;
    private Abobus targetAbobus;
    private GameManager game_manager;
    List<HexCell> skillTurns;
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

    //копипаста из Medver.cs
    private List<HexCell> GetOneCellsArround(HexCell from)
    {
    List<HexCell> nearbyCells = new List<HexCell>();
    Vector3[] basisTurns = RangeOneComponent.GetBasisTurns();

    foreach (Vector3 turn in basisTurns)
    {
        HexCoordinates candidate = HexCoordinates.FromXY(from.hex_coordinates.X + (int)turn[0], from.hex_coordinates.Y + (int)turn[1]);
        
        if (!game_manager.hex_grid.CheckHexCoordsOutOfBounds(candidate))
        {
            HexCell cell_candidate = game_manager.hex_grid.GetCellByHexCoordinates(candidate);
            
            //клетки с состоянием empty или abobus
            if (cell_candidate.state == HexCell.State.empty || cell_candidate.state == HexCell.State.abobus)
            {
                nearbyCells.Add(cell_candidate);
            }
        }
    }
    return nearbyCells;
    }

    public void Invoke()
    {
        Debug.Log("Medver invokes skill");

        Abobus selectedAbobus = applied_to.abobus;

        if (game_manager.selected_abobus == null) {
            SelectAbobus select_abobus = new SelectAbobus(applied_to, game_manager.GetAbobusByHexCoordinates(applied_to.hex_coordinates));
            select_abobus.Invoke();
        }

        List<HexCell> skillTurns = initialAbobus.GetPossibleSkillTurns(applied_to);
        List<HexCell> validMoves = GetOneCellsArround(selectedAbobus.cell);

        foreach (HexCell cell in skillTurns)
        {

            if(validMoves.Contains(cell)){
                cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.highlighted_yellow);

                Abobus targetAbobus = game_manager.GetAbobusByHexCoordinates(cell.hex_coordinates);
                
                cell.actions.AddFirst(new Swap(cell, selectedAbobus, targetAbobus));

                cell.actions.AddLast(new SimpleUnhighlight(applied_to, skillTurns));
                cell.actions.AddLast(new EndTurn(cell));
            }
        }
    }
}
