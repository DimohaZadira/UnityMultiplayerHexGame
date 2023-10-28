using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/*ВЫБРАН СОХАД: зеленым подсвечиваются все гексы в радиусе 2 за исключением тех, на которых стоят другие звероники. 
Если СОХАД граничит с любым другим звероником и по направлению от Сохада к этому зверонику следующий гекс свободен, 
то такой гекс подсвечивается желтым. После нажатия на этот гекс СОХАД переместится на него, 
и пройдет аналогичная проверка возможности повторного прыжка (как в шашках, когда происходит «перепрыгивание» 
через целую цепочку шашек противника). Таким образом, чтобы сделать СОХАДОМ серию прыжков через расположенных 
через гекс звероников (не важно, противников или союзников), нужно прокликать каждый промежуточный гекс. 
При этом СОХАД будет перемещаться прыжками через одного звероника до тих пор, пока не останется возможности «перепрыгнуть». 
Когда продолжение хода будет невозможно, на экран выводится символ «конец хода».*/
public class Sohad : Abobus
{
    
    private List<HexCoordinates> visited;
    public Sohad()
    {
        visited = new List<HexCoordinates>();
    }
    override public void RefreshSelf()
    {
        visited.Clear();
    }
    override public bool PerformSkill(HexCell from, HexCell to)
    {
        Debug.Log("Entered <color=yellow>PerformSkill</color>");
        visited.Add(hex_coordinates);
        MoveToHexCoordinates(to.hex_coordinates); 
        return !(GetSkillTurns(to.hex_coordinates).Count > 0);
    }

    override public List<HexCoordinates> GetPossibleMovementTurns()
    {
        List<HexCoordinates> ans = new List<HexCoordinates>();
        Vector3[] basis_turns = RangeOneComponent.GetBasisTurns();
        foreach (Vector3 turn in basis_turns) {
            for (int i = 1; i <= 2; ++i) {
                HexCoordinates candidate = HexCoordinates.FromXY(hex_coordinates.X + i * (int)turn[0], hex_coordinates.Y + i * (int)turn[1]);

                if (game_manager.hex_grid.CheckHexCoordsOutOfBounds(candidate)) {
                    break;
                }
                
                HexCell cell_candidate = game_manager.hex_grid.GetCellByHexCoordinates(candidate);
                
                if (cell_candidate.state == HexCell.State.empty) {
                    ans.Add(candidate);
                } else {
                    break;
                }
                
            }
        }

        return ans;
    }

    private List<HexCoordinates> GetSkillTurns(HexCoordinates hc)
    {
        List<HexCoordinates> ans = new List<HexCoordinates>();
        Vector3[] basis_turns = RangeOneComponent.GetBasisTurns();
        foreach (Vector3 turn in basis_turns) {
            HexCoordinates neighbour = HexCoordinates.FromXY(hc.X + (int)turn[0], hc.Y + (int)turn[1]);
            HexCoordinates candidate = HexCoordinates.FromXY(hc.X + 2 * (int)turn[0], hc.Y + 2 * (int)turn[1]);

            if (game_manager.hex_grid.CheckHexCoordsOutOfBounds(neighbour) || game_manager.hex_grid.CheckHexCoordsOutOfBounds(candidate)) {
                break;
            }
            
            HexCell cell_neighbour = game_manager.hex_grid.GetCellByHexCoordinates(neighbour);
            HexCell cell_candidate = game_manager.hex_grid.GetCellByHexCoordinates(candidate);
            if ( (cell_neighbour.state == HexCell.State.abobus)
              && (cell_candidate.state == HexCell.State.empty)
              && !visited.Contains(candidate)){
                ans.Add(candidate);
            }            
        }
        return ans;
    }
    override public List<HexCoordinates> GetPossibleSkillTriggerTurns()
    {
        return GetSkillTurns(hex_coordinates);
    }

    
    override public List<HexCoordinates> GetPossibleSkillTurns(HexCell from)
    {
        return GetSkillTurns(hex_coordinates);
    }

}