using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/*ВЫБРАН БАФФЕР: зеленым подсвечиваются все гексы в радиусе 2 за исключением тех, на которых стоят другие звероники. 
Если БАФФЕР граничит с другим звероником (кроме КАЙМАНЧА), то гекс с соседом подсвечивается желтым. 
При нажатии на него желтым подсвечиваются все гексы в радиусе 1 от выбранного. 
Если на каком-то из них стоит другой звероник, то такой ход невозможен (желтым этот гекс не подсвечивается). 
После нажатия на один из подсвеченных желтым гексов противник будет перемещен на выбранный гекс, а БАФФЕР – на гекс, 
на котором стоял противник до применения способности. Поменять местами БАФФЕРА и любого его соседа возможно. 
Когда продолжение хода будет невозможно, на экран выводится символ «конец хода».*/
public class Buffer : Abobus
{
    public override Type GetSkillType()
    {
        return typeof(BufferSkill);
    }

    override public List<HexCell> GetPossibleMovementTurns()
    {
        List<HexCell> possibleTurns = new List<HexCell>();
        HashSet<HexCoordinates> blockedCoordinates = new HashSet<HexCoordinates>();

        foreach (Vector3 turn in RangeOneComponent.GetBasisTurns())
        {
            HexCoordinates abobusNeighborCoords = HexCoordinates.FromXY(
                cell.hex_coordinates.X + (int)turn[0],
                cell.hex_coordinates.Y + (int)turn[1]
            );

            if (!game_manager.hex_grid.CheckHexCoordsOutOfBounds(abobusNeighborCoords))
            {
                HexCell abobusNeighborCell = game_manager.hex_grid.GetCellByHexCoordinates(abobusNeighborCoords);
                if (abobusNeighborCell.state == HexCell.State.abobus)
                {
                    HexCoordinates blockedCellCoords = HexCoordinates.FromXY(
                        cell.hex_coordinates.X + 2 * (int)turn[0],
                        cell.hex_coordinates.Y + 2 * (int)turn[1]
                    );
                    blockedCoordinates.Add(blockedCellCoords);
                }
            }
        }

        List<HexCell> standardMoves = GetPossibleTurns(cell, RangeTwoComponent.GetBasisTurns(), HexCell.State.empty);
        foreach (var moveCell in standardMoves)
        {
            if (!blockedCoordinates.Contains(moveCell.hex_coordinates))
            {
                possibleTurns.Add(moveCell);
            }
        }

        return possibleTurns;
    }

    override public List<HexCell> GetPossibleSkillTriggerTurns()
    {
        List<HexCell> ans = new List<HexCell>();

        foreach (Vector3 turn in RangeOneComponent.GetBasisTurns())
        {
            HexCoordinates candidate = HexCoordinates.FromXY(cell.hex_coordinates.X + (int)turn[0], cell.hex_coordinates.Y + (int)turn[1]);

            if (!game_manager.hex_grid.CheckHexCoordsOutOfBounds(candidate))
            {
                HexCell cell_candidate = game_manager.hex_grid.GetCellByHexCoordinates(candidate);
                if (cell_candidate.state == HexCell.State.abobus)
                {
                    if (cell_candidate.abobus != null && !(cell_candidate.abobus is Kaymanch))
                    {
                        ans.Add(cell_candidate);
                    } 
                }
            }
        }
        return ans;
    }

}