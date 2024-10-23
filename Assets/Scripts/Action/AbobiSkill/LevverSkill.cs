using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;

public class LevverSkill : IAction
{
    private HexCell applied_to;
    private Abobus abobus;
    private GameManager game_manager;
    public LevverSkill(HexCell applied_to, Abobus abobus)
    {
        this.applied_to = applied_to;
        this.abobus = abobus;
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
        return "Levver skill";
    }

    public void Invoke()
    {
        Debug.Log("Levver invokes skill");

        if (game_manager.selected_abobus == null)
        {
            SelectAbobus select_abobus = new SelectAbobus(abobus.cell, abobus);
            select_abobus.Invoke();
        }

        List<HexCell> skill_turns = GetPossibleJumpTurns();

        foreach (HexCell cell in skill_turns)
        {
            cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.highlighted_blue);

            Abobus target_abobus = game_manager.GetAbobusByHexCoordinates(cell.hex_coordinates);
            if (target_abobus != null)
            {
                HexCell destination = GetDestinationCell(cell, target_abobus);

                if (destination != null)
                {
                    cell.actions.AddLast(new SimpleMovement(destination, target_abobus));
                }
            }
            cell.actions.AddLast(new SimpleUnhighlight(applied_to, skill_turns));
            cell.actions.AddLast(new UnselectAbobus(cell, abobus));
            cell.actions.AddLast(new EndTurn(cell));
        }
        abobus.cell.actions.AddLast(new SimpleUnhighlight(abobus.cell, skill_turns));
        abobus.cell.actions.AddLast(new ClearActions<IAction>(abobus.cell, skill_turns));
        abobus.cell.actions.AddLast(new ReturnHighlights(abobus.cell, abobus));
    }

    private List<HexCell> GetPossibleJumpTurns()
    {
        Vector3[] basis_turns = RangeOneComponent.GetBasisTurns();
        List<HexCell> possible_turns = new List<HexCell>();

        foreach (Vector3 turn in basis_turns)
        {
            HexCoordinates neighborCoords = HexCoordinates.FromXY(
                abobus.cell.hex_coordinates.X + (int)turn.x,
                abobus.cell.hex_coordinates.Y + (int)turn.y);

            if (!game_manager.hex_grid.CheckHexCoordsOutOfBounds(neighborCoords))
            {
                HexCell neighborCell = game_manager.hex_grid.GetCellByHexCoordinates(neighborCoords);

                Abobus target_abobus = game_manager.GetAbobusByHexCoordinates(neighborCell.hex_coordinates);
                if (target_abobus != null)
                {
                    possible_turns.Add(neighborCell);
                }
            }
        }

        return possible_turns;
    }

    private HexCell GetDestinationCell(HexCell startCell, Abobus target_abobus)
    {
        int deltaX = startCell.hex_coordinates.X - abobus.cell.hex_coordinates.X;
        int deltaY = startCell.hex_coordinates.Y - abobus.cell.hex_coordinates.Y;
        
        Vector3 direction = new Vector3(deltaX, deltaY, 0);
        
        HexCoordinates destinationCoords = HexCoordinates.FromXY(
            startCell.hex_coordinates.X + (int)direction.x,
            startCell.hex_coordinates.Y + (int)direction.y);
            
        if (!game_manager.hex_grid.CheckHexCoordsOutOfBounds(destinationCoords))
        {
            HexCell destinationCell = game_manager.hex_grid.GetCellByHexCoordinates(destinationCoords);
            if (destinationCell.state == HexCell.State.empty)
            {
                return destinationCell;
            }
        }
    return null;
    }
}