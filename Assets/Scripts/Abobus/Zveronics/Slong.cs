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
    public Slong ()
    {
        action_type = typeof(SlongSkill);
    }
    private List<HexCell> GetPossibleTurns(HexCell from,  Vector3[] basis_turns, HexCell.State check)
    {
        List<HexCell> ans = new List<HexCell>();

        foreach (Vector3 turn in basis_turns) {
            HexCoordinates candidate = HexCoordinates.FromXY(from.hex_coordinates.X + (int)turn[0], from.hex_coordinates.Y + (int)turn[1]);
            
            if (!game_manager.hex_grid.CheckHexCoordsOutOfBounds(candidate)) {
                HexCell cell_candidate = game_manager.hex_grid.GetCellByHexCoordinates(candidate);
                if (cell_candidate.state == check) {
                    ans.Add(cell_candidate);
                }
            }
        }
        return ans;
    }

    override public List<HexCell> GetPossibleMovementTurns()
    {
        return GetPossibleTurns(cell, RangeOneComponent.GetBasisTurns(), HexCell.State.empty);
    }

    override public List<HexCell> GetPossibleSkillTriggerTurns()
    {
        List<HexCell> ans = new List<HexCell>();

        foreach (Vector3 turn in RangeOneComponent.GetBasisTurns()) {
            HexCoordinates candidate = HexCoordinates.FromXY(cell.hex_coordinates.X + (int)turn[0], cell.hex_coordinates.Y + (int)turn[1]);
            
            if (!game_manager.hex_grid.CheckHexCoordsOutOfBounds(candidate)) {
                HexCell cell_candidate = game_manager.hex_grid.GetCellByHexCoordinates(candidate);
                if ((cell_candidate.state == HexCell.State.abobus) && (game_manager.GetAbobusByHexCoordinates(candidate).team != team)) {
                    
                    ans.Add(cell_candidate);
                }
            }
        }
        return ans;
    }

    
    override public List<HexCell> GetPossibleSkillTurns(HexCell from)
    {
        return GetPossibleTurns(from, RadiusThreeComponent.GetBasisTurns(), HexCell.State.empty);
    }

}
