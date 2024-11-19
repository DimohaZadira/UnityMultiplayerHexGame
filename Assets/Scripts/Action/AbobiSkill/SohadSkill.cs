using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;

public class SohadSkill : IAction
{
    private HexCell applied_to;
    private Abobus abobus;
    private GameManager game_manager;
    private HashSet<HexCell> visited_cells;
    public SohadSkill (HexCell applied_to, Abobus abobus)
    {
        this.applied_to = applied_to;
        this.abobus = abobus;
        visited_cells = new HashSet<HexCell>();
        game_manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    
    public HexCell AppliedTo { 
        get => applied_to; 
        set {
            applied_to = value;
        } 
    }
    
    public string DebugMessage()
    {
        return "Slong skill";
    }


    public void Invoke()
    {
        Debug.Log("Slong invokes skill");
        
        // Получаем список смещений из RangeTwoComponent
        Vector3[] turns = RangeTwoComponent.GetBasisTurns();

        //List<HexCell> skill_turns = abobus.GetPossibleSkillTurns(applied_to);
       
        // Используем координаты для вычисления доступных клеток
        List<HexCell> skill_turns = new List<HexCell>();

        foreach (Vector3 turn in turns)
        {
            /*
            HexCoordinates targetCoordinates = applied_to.hex_coordinates + turn;
            HexCell targetCell = game_manager.GetAbobusByHexCoordinates(targetCoordinates); 
            */
            HexCoordinates targetCoordinates = applied_to.hex_coordinates + turn;

            // Получаем Abobus, а затем его клетку
            Abobus targetAbobus = game_manager.GetAbobusByHexCoordinates(targetCoordinates);
            HexCell targetCell = targetAbobus?.cell; // Получаем клетку из Abobus (если Abobus не null)
            if (targetCell != null && !visited_cells.Contains(targetCell))
            {
                skill_turns.Add(targetCell);
            }
        }

        foreach (HexCell cell in skill_turns) {
            // Подсвечиваем клетки
            cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.highlighted_yellow);

            // Добавляем действия в клетку
            cell.actions.AddLast(new SimpleMovement(cell, game_manager.GetAbobusByHexCoordinates(applied_to.hex_coordinates)));
            cell.actions.AddLast(new SimpleMovement(applied_to, abobus));
            cell.actions.AddLast(new SimpleUnhighlight(applied_to, skill_turns));
            cell.actions.AddLast(new ClearActions<SohadSkill>(applied_to, skill_turns));
        }

        // Добавляем текущую клетку в список посещённых
        visited_cells.Add(applied_to);
    }}