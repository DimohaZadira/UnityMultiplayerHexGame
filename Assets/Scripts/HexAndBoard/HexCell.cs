using UnityEngine;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using Unity.VisualScripting;

public class HexCell : MonoBehaviour {
	public HexCoordinates hex_coordinates;
    public GameManager game_manager;
    public Abobus abobus;
    public State state;
    public enum State {
        out_of_bounds, abobus, empty
    };

    public List<Skill> skills;

    void Awake ()
    {
        game_manager = GameObject.FindGameObjectsWithTag("GameController")[0].GetComponent<GameManager>();
        skills = new List<Skill>();
    }

    public void Refresh ()
    {
        skills.Clear();
        switch (state) {
            case State.empty:
                break;
            case State.abobus:
                Abobus abobus = game_manager.GetAbobusByHexCoordinates(hex_coordinates);
                
                foreach (HexCoordinates turn_coords in abobus.GetPossibleMovementTurns()) {
                    HexCell hc = game_manager.hex_grid.GetCellByHexCoordinates(turn_coords);
                    Movement mov = new Movement(abobus, hc, this);
                    hc.skills.Add(mov);
                }
                break;
        }
    }

    public void React()
    {
        Debug.Log("Cell <color=yellow>" + hex_coordinates.ToString() + "</color> reacts!");
        HighlightableCell highlightable_cell = GetComponent<HighlightableCell>();



        if (highlightable_cell.is_highlighted) {
            foreach (Skill skill in skills) {
                skill.Invoke();
            }
        } else {
            foreach (HexCoordinates turn_coords in abobus.GetPossibleMovementTurns()) {
                HexCell hc = game_manager.hex_grid.GetCellByHexCoordinates(turn_coords);
                HighlightableCell turn_highlightable_cell = hc.GetComponent<HighlightableCell>();
                turn_highlightable_cell.SetState(HighlightableCell.State.highlighted_green);
            }
        }
        
        
    }

}