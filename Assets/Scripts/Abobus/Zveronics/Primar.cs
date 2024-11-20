using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*Выбран ПРИМАР: зеленым подсвечиваются все гексы в радиусе 1 за исключением тех, на которых стоят другие звероники. 
Если ПРИМАР граничит с любым другим звероником, то гекс соседа подсвечивается желтым. 
После клика на этот гекс желтым подсвечиваются все граничащие с выбранным гексы, на которых стоят другие звероники. 
Зеленым подсвечиваются все остальные гексы в радиусе 1 от последнего выбранного. 
Таким образом, для применения способности нужно по очереди кликать на цепочку звероников, стоящих рядом, 
и «спрыгнуть» на свободный граничащий гекс в любую сторону с каждого из них. 
После клика на подсвеченную зеленым клетку ход заканчивается.*/
public class Primar : Abobus
{
    public List<HexCoordinates> visited;
    public Primar()
    {
        visited = new List<HexCoordinates>();
    }

    override public List<HexCell> GetPossibleMovementTurns()
    {
        return GetPossibleTurns(cell, RangeOneComponent.GetBasisTurns(), HexCell.State.empty);
    }

    override public List<HexCell> GetPossibleSkillTriggerTurns()
    {
        return GetPossibleTurns(cell, RangeOneComponent.GetBasisTurns(), HexCell.State.abobus);
    }

}
