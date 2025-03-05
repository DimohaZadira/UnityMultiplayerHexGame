using System;
using System.Collections.Generic;
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
    public List<HexCell> visited;
    public Sohad()
    {
        visited = new List<HexCell>();    
    }
    public override Type GetSkillType()
    {
        return typeof(SohadSkill);
    }

    override public List<HexCell> GetPossibleMovementTurns()
    {
        List<HexCell> ans = new List<HexCell>();
        Vector3[] basis_turns = RangeOneComponent.GetBasisTurns();
        foreach (Vector3 turn in basis_turns)
        {
            for (int i = 1; i <= 2; ++i)
            {
                HexCoordinates candidate = HexCoordinates.FromXY(cell.hex_coordinates.X + i * (int)turn[0], cell.hex_coordinates.Y + i * (int)turn[1]);

                if (game_manager.hex_grid.CheckHexCoordsOutOfBounds(candidate))
                {
                    break;
                }

                HexCell cell_candidate = game_manager.hex_grid.GetCellByHexCoordinates(candidate);

                if (cell_candidate.state == HexCell.State.empty)
                {
                    ans.Add(cell_candidate);
                }
                else
                {
                    break;
                }

            }
        }

        return ans;
    }

    public List<HexCell> GetSkillTurns(HexCell hc)
    {
        List<HexCell> ans = new List<HexCell>();
        Vector3[] basis_turns = RangeOneComponent.GetBasisTurns();
        foreach (Vector3 turn in basis_turns)
        {
            HexCoordinates neighbour = HexCoordinates.FromXY(hc.hex_coordinates.X + (int)turn[0], hc.hex_coordinates.Y + (int)turn[1]);
            HexCoordinates candidate = HexCoordinates.FromXY(hc.hex_coordinates.X + 2 * (int)turn[0], hc.hex_coordinates.Y + 2 * (int)turn[1]);

            if (game_manager.hex_grid.CheckHexCoordsOutOfBounds(neighbour) || game_manager.hex_grid.CheckHexCoordsOutOfBounds(candidate))
            {
                break;
            }

            HexCell cell_neighbour = game_manager.hex_grid.GetCellByHexCoordinates(neighbour);
            HexCell cell_candidate = game_manager.hex_grid.GetCellByHexCoordinates(candidate);
            if ((cell_neighbour.state == HexCell.State.abobus)
              && (cell_candidate.state == HexCell.State.empty)
              && !visited.Contains(cell_candidate))
            {
                ans.Add(cell_candidate);
            }
        }
        return ans;
    }
    override public List<HexCell> GetPossibleSkillTriggerTurns()
    {
        return GetSkillTurns(cell);
    }

}