using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEditor.PackageManager;


public class HexCell : MonoBehaviour {
	public HexCoordinates hex_coordinates;
    public GameManager game_manager;
    public Abobus abobus;
    public State state;
    public enum State {
        out_of_bounds, abobus, empty
    };

    public Deque<IAction> actions;
    public String debug_str;
    
    public void DeleteFromActions<T> () {
        List<IAction> to_remove = new List<IAction>();

        foreach (IAction act in actions) {
            if (act.GetType() == typeof(T)) {
                to_remove.Add(act);
            }
        }
        foreach (IAction act in to_remove) {
            actions.Remove(act);
        }
    }

    public override string ToString () {
        String ans = "";
        foreach (IAction action in actions ) {
            ans += action.DebugMessage() + '\n';
        }
		return ans;
	}

    void Awake ()
    {
        game_manager = GameObject.FindGameObjectsWithTag("GameController")[0].GetComponent<GameManager>();
        actions = new Deque<IAction>();
    }

    void Update () {
        debug_str = ToString();
    }
    public void Refresh ()
    {
        // skills.Clear();
        abobus = game_manager.GetAbobusByHexCoordinates(hex_coordinates);
        if (abobus) {
            state = State.abobus;
        } else if (game_manager.hex_grid.CheckHexCoordsOutOfBounds(hex_coordinates)) {
            state = State.out_of_bounds;
        } else {
            state = State.empty;
        }
        
    }

    public void React()
    {
        Debug.Log("Cell <color=yellow>" + hex_coordinates.ToString() + "</color> reacts:\n" + ToString());

        int size = actions.size;
        for (int i = 0; i < size; ++i) {
            actions.DequeueFirst().Invoke();
        }        
        
    }

}