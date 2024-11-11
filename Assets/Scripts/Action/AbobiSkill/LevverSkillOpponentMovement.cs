using System;
using System.Collections.Generic;
using UnityEngine;

public class LevverSkillOpponentMovement : IAction
{
    private HexCell applied_to;
    private Abobus abobus;
    private GameManager game_manager;

    public LevverSkillOpponentMovement(HexCell applied_to, Abobus abobus)
    {
        this.applied_to = applied_to;
        this.abobus = abobus;
        game_manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    public void Invoke()
    {
        Debug.Log("Levver invokes opponent movement skill");

        List<HexCell> skill_turns = abobus.GetPossibleSkillTurns(applied_to);

        foreach (HexCell cell in skill_turns)
        {
            if (cell.abobus != null && cell.abobus != abobus)
            {
                HexCell destinationCell = GetDestinationCell(cell, abobus);

                if (destinationCell != null && destinationCell.state == HexCell.State.empty)
                {
                    destinationCell.state = HexCell.State.empty;

                    destinationCell.actions.AddLast(new PerformSkill(destinationCell, cell.abobus));
                    destinationCell.actions.AddLast(new SimpleMovement(destinationCell, game_manager.GetAbobusByHexCoordinates(cell.hex_coordinates)));
                    destinationCell.actions.AddLast(new SimpleUnhighlight(destinationCell, skill_turns));
                    destinationCell.actions.AddLast(new UnselectAbobus(destinationCell, abobus));
                }
            }
        }

        abobus.cell.actions.AddLast(new SimpleUnhighlight(abobus.cell, skill_turns));
        abobus.cell.actions.AddLast(new ClearActions<IAction>(abobus.cell, skill_turns));
        abobus.cell.actions.AddLast(new ReturnHighlights(abobus.cell, abobus));
        abobus.cell.actions.AddLast(new EndTurn(abobus.cell));
    }

    private HexCell GetDestinationCell(HexCell startCell, Abobus target_abobus)
    {
        int deltaX = startCell.hex_coordinates.X - abobus.cell.hex_coordinates.X;
        int deltaY = startCell.hex_coordinates.Y - abobus.cell.hex_coordinates.Y;

        Vector3 direction = new Vector3(deltaX, deltaY, 0);

        HexCoordinates destinationCoords = HexCoordinates.FromXY(
            startCell.hex_coordinates.X + (int)direction.x,
            startCell.hex_coordinates.Y + (int)direction.y
        );

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