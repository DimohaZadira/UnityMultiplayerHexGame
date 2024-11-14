using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*ВЫБРАН БАФФЕР: зеленым подсвечиваются все гексы в радиусе 2 за исключением тех, на которых стоят другие звероники. 
Если БАФФЕР граничит с другим звероником (кроме КАЙМАНЧА), то гекс с соседом подсвечивается желтым. 
При нажатии на него желтым подсвечиваются все гексы в радиусе 1 от выбранного. 
Если на каком-то из них стоит другой звероник, то такой ход невозможен (желтым этот гекс не подсвечивается). 
После нажатия на один из подсвеченных желтым гексов противник будет перемещен на выбранный гекс, а БАФФЕР – на гекс, 
на котором стоял противник до применения способности. Поменять местами БАФФЕРА и любого его соседа возможно. 
Когда продолжение хода будет невозможно, на экран выводится символ «конец хода».*/
public class Buffer : Abobus
{
    public Buffer()
    {
        action_type = typeof(BufferSkill);
    }

    override public List<HexCell> GetPossibleMovementTurns()
    {
        return GetPossibleTurns(cell, RangeTwoComponent.GetBasisTurns(), HexCell.State.empty);
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
                if ((cell_candidate.state == HexCell.State.abobus) && (game_manager.GetAbobusByHexCoordinates(candidate).team != team))
                {
                    // ans.Add(cell_candidate);

                    if (cell_candidate.abobus != null && !(cell_candidate.abobus is Kaymanch))
                    {
                        ans.Add(cell_candidate);
                    } 
                }
            }
        }
        return ans;
    }

    // public List<HexCell> GetPossibleSkillTurns(HexCell from)
    // {
    //     return GetPossibleTurns(from, RadiusTwoComponent.GetBasisTurns(), HexCell.State.empty);
    // }

}