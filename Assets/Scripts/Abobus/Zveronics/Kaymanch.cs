using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/*ВЫБРАН КАЙМАНЧ: зеленым подсвечиваются все гексы в радиусе 1 за исключением тех, на которых стоят другие звероники. 
Если КАЙМАНЧ граничит с союзным звероником, то гекс с этим союзником подсвечивается желтым. 
При клике на этот гекс желтым подсвечиваются свободные гексы, находящиеся в радиусе 2 от КАЙМАНЧА. 
После клика на один из подсвеченных гексов КАЙМАНЧ останется на изначальном месте, 
а кликнутый союзник переместится на выбранный гекс. Когда продолжение хода будет невозможно, 
на экран выводится символ «конец хода».*/
public class Kaymanch : Abobus
{

    override public void RefreshSelf()
    {
    }
    override public bool PerformSkill(HexCell from, HexCell to)
    {
        Debug.Log("Moving zveronic from " + from.hex_coordinates.ToString() + " to " + to.hex_coordinates.ToString());
        
        Abobus to_move = game_manager.GetAbobusByHexCoordinates(from.hex_coordinates);
        to_move.MoveToHexCoordinates(to.hex_coordinates);
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
        return GetPossibleTurns(hex_coordinates, RangeOneComponent.GetBasisTurns(), HexCell.State.empty);
    }

    override public List<HexCoordinates> GetPossibleSkillTriggerTurns()
    {
        List<HexCoordinates> ans = new List<HexCoordinates>();

        foreach (Vector3 turn in RangeOneComponent.GetBasisTurns()) {
            HexCoordinates candidate = HexCoordinates.FromXY(hex_coordinates.X + (int)turn[0], hex_coordinates.Y + (int)turn[1]);
            
            if (!game_manager.hex_grid.CheckHexCoordsOutOfBounds(candidate)) {
                HexCell cell_candidate = game_manager.hex_grid.GetCellByHexCoordinates(candidate);
                if ((cell_candidate.state == HexCell.State.abobus) && (game_manager.GetAbobusByHexCoordinates(candidate).team == team)) {
                    ans.Add(candidate);
                }
            }
        }
        return ans;
    }

    
    override public List<HexCoordinates> GetPossibleSkillTurns(HexCell from)
    {
        return GetPossibleTurns(hex_coordinates, RadiusTwoComponent.GetBasisTurns(), HexCell.State.empty);
    }

}