using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*ВЫБРАН СЛОНГ: зеленым подсвечиваются все гексы в радиусе 1 за исключением тех, на которых стоят другие звероники. 
Если СЛОНГ граничит с противником, гекс, на котором стоит противник, подсвечивается желтым 
(если противник не КАЙМАНЧ, гекс с КАЙМАНЧЕМ не подсвечивается). 
Фигура звероника, стоящего на этом гексе, делается полупрозрачной и кликабельной. 
После клика на полупрозрачную фигуру (это значит, что СЛОНГ применяет свою способность на этого звероника) 
все клетки в радиусе 3 от выбранного противника тоже подсвечиваются желтым 
(это клетки, на которые можно переместить выбранного противника). 
Таким образом, чтобы применить способность, нужно сначала нажать на гекс с противником, куда будет перемещен 
СЛОНГ при завершении хода, а затем – на гекс, куда должен быть перемещен противник после применения способности. 
Когда продолжение хода будет невозможно, на экран выводится символ «конец хода».*/
public class Slong : Abobus
{
    override public bool PerformSkill(HexCell from, HexCell to)
    {
        Debug.Log("Moving zveronic from " + from.hex_coordinates.ToString() + " to " + to.hex_coordinates.ToString());
        
        Abobus to_move = gay_manager.GetAbobusByHexCoordinates(from.hex_coordinates);
        to_move.MoveToHexCoordinates(to.hex_coordinates);
        MoveToHexCoordinates(from.hex_coordinates);
        return true;
    }
    override public void PrePerformSkill(HexCell to) {
    }
    private List<HexCoordinates> GetPossibleTurns(HexCoordinates from,  Vector3[] basis_turns, HexCell.State check)
    {
        List<HexCoordinates> ans = new List<HexCoordinates>();

        foreach (Vector3 turn in basis_turns) {
            HexCoordinates candidate = HexCoordinates.FromXY(from.X + (int)turn[0], from.Y + (int)turn[1]);
            
            if (!gay_manager.hex_grid.CheckHexCoordsOutOfBounds(candidate)) {
                HexCell cell_candidate = gay_manager.hex_grid.GetCellByHexCoordinates(candidate);
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
            
            if (!gay_manager.hex_grid.CheckHexCoordsOutOfBounds(candidate)) {
                HexCell cell_candidate = gay_manager.hex_grid.GetCellByHexCoordinates(candidate);
                if ((cell_candidate.state == HexCell.State.abobus) && (gay_manager.GetAbobusByHexCoordinates(candidate).team != team)) {
                    
                    ans.Add(candidate);
                }
            }
        }
        return ans;
    }

    
    override public List<HexCoordinates> GetPossibleSkillTurns(HexCell from)
    {
        return GetPossibleTurns(from.hex_coordinates, RadiusThreeComponent.GetBasisTurns(), HexCell.State.empty);
    }

}
