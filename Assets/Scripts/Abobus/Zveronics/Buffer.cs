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

    override public void RefreshSelf()
    {
    }
    override public bool PerformSkill(HexCell from, HexCell to)
    {
        Abobus to_move = game_manager.GetAbobusByHexCoordinates(from.hex_coordinates);
        to_move.MoveToHexCoordinates(to.hex_coordinates);
        MoveToHexCoordinates(from.hex_coordinates);
        
        return true;
    }
    private List<HexCoordinates> GetPossibleTurns(HexCoordinates from,  Vector3[] basis_turns, HexCell.State check)
    {
        List<HexCoordinates> ans = new List<HexCoordinates>();

        foreach (Vector3 turn in basis_turns) {
            HexCoordinates candidate = HexCoordinates.FromXY(from.X + (int)turn[0], from.Y + (int)turn[1]);
            
            if (!game_manager.hex_grid.CheckHexCoordsOutOfBounds(candidate)) {
                HexCell cell_candidate = game_manager.hex_grid.GetCellByHexCoordinates(candidate);
                if (cell_candidate.state == check) {
                    ans.Add(candidate);
                }
            }
        }
        return ans;
    }

    override public List<HexCoordinates> GetPossibleMovementTurns()
    {
        return GetPossibleTurns(hex_coordinates, RangeTwoComponent.GetBasisTurns(), HexCell.State.empty);
    }

    override public List<HexCoordinates> GetPossibleSkillTriggerTurns()
    {
        return GetPossibleTurns(hex_coordinates, RangeOneComponent.GetBasisTurns(), HexCell.State.abobus);
    }

    
    override public List<HexCoordinates> GetPossibleSkillTurns(HexCell from)
    {
        List<HexCoordinates> ans = GetPossibleTurns(from.hex_coordinates, RangeOneComponent.GetBasisTurns(), HexCell.State.empty);
        ans.Add(hex_coordinates);
        return ans;
    }

}