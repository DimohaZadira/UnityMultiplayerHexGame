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
{    private HashSet<HexCoordinates> visited;

    public Sohad()
    {
        visited = new HashSet<HexCoordinates>();
        action_type = typeof(SohadSkill);
    }

    override public List<HexCell> GetPossibleMovementTurns()
    {
        List<HexCell> ans = new List<HexCell>();
        Vector3[] basis_turns = RangeOneComponent.GetBasisTurns();
        visited.Clear(); // Очищаем посещённые клетки перед расчётом

        foreach (Vector3 turn in basis_turns)
        {
            for (int i = 1; i <= 2; ++i)
            {
                HexCoordinates candidateCoords = HexCoordinates.FromXZ(
                    cell.hex_coordinates.X + i * (int)turn.x, 
                    cell.hex_coordinates.Z + i * (int)turn.z
                );

                if (game_manager.hex_grid.CheckHexCoordsOutOfBounds(candidateCoords))
                {
                    break; // Выходим, если клетка за пределами сетки
                }

                HexCell candidateCell = game_manager.hex_grid.GetCellByHexCoordinates(candidateCoords);

                if (candidateCell != null && candidateCell.state == HexCell.State.empty)
                {
                    if (!visited.Contains(candidateCoords))
                    {
                        ans.Add(candidateCell);
                        visited.Add(candidateCoords); // Отмечаем как посещённую
                    }
                }
                else
                {
                    break; // Прерываем цепочку, если клетка занята
                }
            }
        }

        return ans;
    }

    override public List<HexCell> GetPossibleSkillTriggerTurns()
    {
        return GetSkillTurns(cell); // Подсветка клеток для скилла
    }

    override public List<HexCell> GetPossibleSkillTurns(HexCell from)
    {
        return GetSkillTurns(from); // Логика прыжка за фигуры
    }

    private List<HexCell> GetSkillTurns(HexCell from)
    {
        List<HexCell> ans = new List<HexCell>();
        Vector3[] basis_turns = RangeOneComponent.GetBasisTurns();

        foreach (Vector3 turn in basis_turns)
        {
            // Рассчитываем соседнюю клетку и клетку за ней
            HexCoordinates neighbourCoords = HexCoordinates.FromXZ(
                from.hex_coordinates.X + (int)turn.x,
                from.hex_coordinates.Z + (int)turn.z
            );
            HexCoordinates jumpCoords = HexCoordinates.FromXZ(
                neighbourCoords.X + (int)turn.x,
                neighbourCoords.Z + (int)turn.z
            );

            // Пропускаем клетки, если они за пределами карты
            if (game_manager.hex_grid.CheckHexCoordsOutOfBounds(neighbourCoords) ||
                game_manager.hex_grid.CheckHexCoordsOutOfBounds(jumpCoords))
            {
                continue;
            }

            HexCell neighbourCell = game_manager.hex_grid.GetCellByHexCoordinates(neighbourCoords);
            HexCell jumpCell = game_manager.hex_grid.GetCellByHexCoordinates(jumpCoords);

            // Проверяем, что соседняя клетка содержит фигуру, а клетка за ней пуста
            if (neighbourCell != null && neighbourCell.abobus != null && 
                jumpCell != null && jumpCell.state == HexCell.State.empty &&
                !visited.Contains(jumpCoords))
            {
                ans.Add(jumpCell); // Добавляем клетку за фигурой
                visited.Add(jumpCoords);
            }
        }

        return ans;
    }
}






/*
public class Sohad : Abobus
{
    
    private List<HexCoordinates> visited;
    public Sohad()
    {
        visited = new List<HexCoordinates>();
        action_type = typeof(SohadSkill);
    }

    override public List<HexCell> GetPossibleMovementTurns()
{
    List<HexCell> ans = new List<HexCell>(); // Доступные для хода клетки
    Vector3[] basis_turns = RangeOneComponent.GetBasisTurns(); // Базовые смещения радиуса 1

    foreach (Vector3 turn in basis_turns) 
    {
        // Шаг 1: Проверяем соседнюю клетку (радиус 1)
        HexCoordinates neighbour = HexCoordinates.FromXZ(
            cell.hex_coordinates.X + (int)turn.x, 
            cell.hex_coordinates.Z + (int)turn.z
        );

        if (game_manager.hex_grid.CheckHexCoordsOutOfBounds(neighbour))
        {
            continue; // Если соседняя клетка за пределами сетки, пропускаем
        }

        HexCell neighbourCell = game_manager.hex_grid.GetCellByHexCoordinates(neighbour);

        // Шаг 2: Если соседняя клетка пуста, добавляем её (зелёная подсветка)
        if (neighbourCell != null && neighbourCell.state == HexCell.State.empty)
        {
            Debug.Log($"Adding green move: {neighbourCell.name}");
            ans.Add(neighbourCell);
            continue; // Переходим к следующей клетке, т.к. она уже обработана
        }

        // Шаг 3: Если соседняя клетка занята другим персонажем
        if (neighbourCell != null && neighbourCell.abobus != null)
        {
            // Проверяем клетку за соседней (радиус 2)
            HexCoordinates behindNeighbour = HexCoordinates.FromXZ(
                neighbour.X + (int)turn.x, 
                neighbour.Z + (int)turn.z
            );

            if (!game_manager.hex_grid.CheckHexCoordsOutOfBounds(behindNeighbour))
            {
                HexCell behindCell = game_manager.hex_grid.GetCellByHexCoordinates(behindNeighbour);

                // Если клетка за соседней фигурой пуста, добавляем её (жёлтая подсветка)
                if (behindCell != null && behindCell.state == HexCell.State.empty)
                {
                    Debug.Log($"Adding yellow move: {behindCell.name}");
                    ans.Add(behindCell);
                }
            }
        }
    }

    Debug.Log($"Total possible moves: {ans.Count}");
    return ans;
}

    private List<HexCell> GetSkillTurns(HexCell hc)
{
    List<HexCell> ans = new List<HexCell>();
    Vector3[] basis_turns = RangeOneComponent.GetBasisTurns();
    foreach (Vector3 turn in basis_turns)
    {
        // Рассчитываем соседнюю клетку и целевую клетку
        HexCoordinates neighbour = HexCoordinates.FromXZ(
            hc.hex_coordinates.X + (int)turn.x, 
            hc.hex_coordinates.Z + (int)turn.z
        );
        HexCoordinates candidate = HexCoordinates.FromXZ(
            hc.hex_coordinates.X + 2 * (int)turn.x, 
            hc.hex_coordinates.Z + 2 * (int)turn.z
        );

        // Проверяем, не выходят ли координаты за пределы сетки
        if (game_manager.hex_grid.CheckHexCoordsOutOfBounds(neighbour) || 
            game_manager.hex_grid.CheckHexCoordsOutOfBounds(candidate))
        {
            continue; // Пропускаем текущую итерацию
        }

        // Получаем объекты клеток
        HexCell cell_neighbour = game_manager.hex_grid.GetCellByHexCoordinates(neighbour);
        HexCell cell_candidate = game_manager.hex_grid.GetCellByHexCoordinates(candidate);

        // Проверяем состояния клеток
        if (cell_neighbour != null && cell_candidate != null)
        {
            if (cell_neighbour.state == HexCell.State.abobus && // В соседней клетке персонаж
                cell_candidate.state == HexCell.State.empty &&  // Целевая клетка пуста
                !visited.Contains(cell_candidate.hex_coordinates)) // Проверяем координаты
            {
                ans.Add(cell_candidate);
                Debug.Log($"Valid move: neighbour={cell_neighbour.name}, candidate={cell_candidate.name}");

                // Добавляем в список посещённых
                visited.Add(cell_candidate.hex_coordinates);
            }
        }
    }

    return ans;
}
*/

/*
    override public List<HexCell> GetPossibleSkillTriggerTurns()
    {
        return GetSkillTurns(cell);
    }

    
    override public List<HexCell> GetPossibleSkillTurns(HexCell from)
    {
        return GetSkillTurns(cell);
    }

*/