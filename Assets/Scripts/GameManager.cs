using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public GameObject hex_grid_go;
    [HideInInspector]
    public HexGrid hex_grid;
    private PlayerInput playerInput;
    private TouchControls touchControls;
    public TextMeshProUGUI team_turn_text;
    public Button end_turn_button;

    public enum Team {
        blue = 0, 
        yellow = 1
    };

    public static int teams_num; 
    public Team team_turn;
    private static int cur_turn = -1;

    
    public void ClearAllHighlightedCells()
    {
        foreach (HexCell hex_cell in hex_grid.GetAllCells()) {
            hex_cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.default_);
        }
    }

    public void ClearAllActions()
    {
        foreach (HexCell hex_cell in hex_grid.GetAllCells()) {
            hex_cell.actions.Clear();
        }
    }

    public void SetAllCheckMovements()
    {
        foreach (HexCell hex_cell in hex_grid.GetAllCells()) {
            if (hex_cell.abobus) {
                hex_cell.actions.Add(new CheckMovement(hex_cell));
            }
        }
    }


    public void SwitchTurn()
    {
        ClearAllHighlightedCells();
        ClearAllActions();
        SetAllCheckMovements();

        cur_turn = (cur_turn + 1) % teams_num;
        team_turn = (Team)cur_turn;
        team_turn_text.text = team_turn.ToString();
        
        // EnableAbobi(team_turn);
        Debug.Log($"<color=yellow>New turn!</color>");
    }

    private Dictionary<Team, List<GameObject>> abobi;


    void SpawnAbobi ()
    {
        GameObject abobus_go = SpawnAbobus<Slong>(Resources.Load("Abobi/KingPrefab"), new Vector2(5, 9), Team.blue, new string("cyan"));
        abobus_go.GetComponentInChildren<Renderer>().material.color = Color.cyan;
        abobi[Team.blue].Add(abobus_go);
        RefreshHexCellState(abobus_go.GetComponent<Abobus>().hex_coordinates);
        
        abobus_go = SpawnAbobus<Slong>(Resources.Load("Abobi/KingPrefab"), new Vector2(5, 10), Team.blue,"blue");
        abobus_go.GetComponentInChildren<Renderer>().material.color = Color.blue;
        abobi[Team.blue].Add(abobus_go);
        RefreshHexCellState(abobus_go.GetComponent<Abobus>().hex_coordinates);
        
        abobus_go = SpawnAbobus<Slong>(Resources.Load("Abobi/KingPrefab"), new Vector2(4, 9), Team.yellow, "yellow");
        abobus_go.GetComponentInChildren<Renderer>().material.color = Color.yellow;
        abobi[Team.yellow].Add(abobus_go);  
        RefreshHexCellState(abobus_go.GetComponent<Abobus>().hex_coordinates);
    }
    GameObject SpawnAbobus<T>(UnityEngine.Object original, Vector2 hex_coords_vec, Team team, String name)
    where T:UnityEngine.Component
    {
        if (!typeof(T).IsSubclassOf(typeof(Abobus))) {
            Debug.Log("Trying to spawn abobus from non-abobus-derived component");
            return null;
        }
        var hc = HexCoordinates.FromXZ((int)hex_coords_vec.x, (int)hex_coords_vec.y);
        GameObject abobus_go = Instantiate(original) as GameObject;
        
        abobus_go.AddComponent<T>();
        abobus_go.GetComponent<Abobus>().Init(this, team, hc, name);

        abobus_go.AddComponent<Animator>();
        abobus_go.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Animations/AbobusAnimCont") as RuntimeAnimatorController;



        return abobus_go;
    }
    
    void Start()
    {
        end_turn_button.GetComponent<Button>().onClick.AddListener(SwitchTurn);
        teams_num = System.Enum.GetNames(typeof(Team)).Length;

        abobi = new Dictionary<Team, List<GameObject>>();
        foreach (Team team in System.Enum.GetValues(typeof(Team))) {
            abobi.Add(team, new List<GameObject>());
        }

        hex_grid = hex_grid_go.GetComponent<HexGrid>();
        hex_grid.CreateGrid(9, 9);
        
        SpawnAbobi();

        playerInput = GetComponent<PlayerInput>();

        touchControls.Player.Click.performed += ctx => OnMouseClick(ctx);
        touchControls.Player.RightClick.performed += ctx => OnMouseClick(ctx);

        SwitchTurn();
    }


    void Awake() {
        touchControls = new TouchControls();
    }
    void OnEnable() {
        touchControls.Enable();
    }
    void OnDisable() {
        touchControls.Disable();
    }

    public void DisableAbobi (Team team, Abobus except = null)
    {
        foreach(GameObject abobus_in_team in abobi[team]) {
            if (abobus_in_team.GetComponent<Abobus>() != except) {
                Abobus abobus_in_team_comp = abobus_in_team.GetComponent<Abobus>();
            } 
        }
    }

    public void EnableAbobi (Team team)
    {
        foreach(GameObject abobus_in_team in abobi[team]) {
            Abobus abobus = abobus_in_team.GetComponent<Abobus>();
            abobus.Refresh();
        }
    }

    public List<Abobus> GetAllAbobi()
    {
        List<Abobus> ans = new List<Abobus>();
        foreach(List<GameObject> abobi_in_team in abobi.Values) {
            foreach(GameObject abobus_go in abobi_in_team) {
                ans.Add(abobus_go.GetComponent<Abobus>());
            }
        }
        return ans;
    }

    public Abobus GetAbobusByHexCoordinates(HexCoordinates hex_coordinates)
    {
        List<Abobus> abobi = GetAllAbobi();
        foreach (Abobus abobus in abobi) {
            if (abobus.hex_coordinates == hex_coordinates) {
                return abobus;
            }
        }
        return null;
    }

    public void RefreshHexCellState (HexCoordinates hex_coordinates) {
        HexCell hex_cell = hex_grid.GetCellByHexCoordinates(hex_coordinates);
        hex_cell.Refresh();
    }

    // public void OnMouseClick(InputAction.CallbackContext value) {
    //     Ray inputRay = Camera.main.ScreenPointToRay(touchControls.Player.ClickPosition.ReadValue<Vector2>());
    //     RaycastHit hit;
    //     if (Physics.Raycast(inputRay, out hit)) {
    //         GameObject hit_go = hit.collider.transform.gameObject;

    //         Abobus abobus = hit_go.GetComponent<Abobus>();
    //         if (abobus && abobus.team == team_turn) {
    //             if (ReferenceEquals(abobus, chosen_abobus)) {
    //                 // Debug.Log("Meow");
    //                 chosen_abobus.React();
    //                 chosen_abobus = null;
    //             } else if (chosen_abobus) {
    //                 chosen_abobus.React();
    //                 chosen_abobus = abobus;
    //                 abobus.React();
    //             } else {
    //                 chosen_abobus = abobus;
    //                 abobus.React();
    //             }
    //         }
    //     }
    // }

    public void OnMouseClick(InputAction.CallbackContext value) {
        Ray inputRay = Camera.main.ScreenPointToRay(touchControls.Player.ClickPosition.ReadValue<Vector2>());
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit)) {
            GameObject hit_go = hit.collider.transform.gameObject;

            HexCell hex_cell = hit_go.GetComponent<HexCell>();
            if (!hex_cell) {
                Abobus abobus = hit_go.GetComponent<Abobus>();
                if (abobus) {
                    hex_cell = hex_grid.GetCellByHexCoordinates(abobus.hex_coordinates);
                }
            }
            if (hex_cell) {
                hex_cell.React();
            }
        }
    }

}
