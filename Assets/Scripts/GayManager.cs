using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GayManager : MonoBehaviour
{
    public GameObject hex_grid_go;
    private HexGrid hex_grid;
    private PlayerInput playerInput;
    private TouchControls touchControls;

    public enum Teams {
        blue, 
        yellow
    };
    private Dictionary<Teams, List<GameObject>> abobi;

    public Abobus chosen_abobus;
    
    public int x, z;

    static GameObject SpawnAbobus<T>(Object original, Vector2 hex_coords_vec, Teams team)
    where T:UnityEngine.Component
    {
        var hc = HexCoordinates.FromXZ((int)hex_coords_vec.x, (int)hex_coords_vec.y);
        GameObject abobus_go = Instantiate(original) as GameObject;
        abobus_go.AddComponent<T>();
        abobus_go.GetComponent<Abobus>().MoveToHexCoordinates(hc);
        abobus_go.GetComponent<Abobus>().team = team;

        return abobus_go;
    }
    // Start is called before the first frame update
    void Start()
    {
        chosen_abobus = null;
        abobi = new Dictionary<Teams, List<GameObject>>();
        foreach (Teams team in System.Enum.GetValues(typeof(Teams))) {
            abobi.Add(team, new List<GameObject>());
        }

        hex_grid = hex_grid_go.GetComponent<HexGrid>();
        hex_grid.CreateGrid();
        
        GameObject abobus_go = SpawnAbobus<Knight>(Resources.Load("Abobi/KnightPrefab"), new Vector2(0, 0), Teams.blue);
        abobi[Teams.blue].Add(abobus_go);

        abobus_go = SpawnAbobus<Rook>(Resources.Load("Abobi/RookPrefab"), new Vector2(1, 0), Teams.yellow);
        abobi[Teams.blue].Add(abobus_go);

        abobus_go = SpawnAbobus<Bishop>(Resources.Load("Abobi/BishopPrefab"), new Vector2(2, 0), Teams.yellow);
        abobi[Teams.blue].Add(abobus_go);
        
        abobus_go = SpawnAbobus<Pawn>(Resources.Load("Abobi/PawnPrefab"), new Vector2(3, 0), Teams.yellow);
        abobi[Teams.yellow].Add(abobus_go);
        
        abobus_go = SpawnAbobus<Queen>(Resources.Load("Abobi/QueenPrefab"), new Vector2(4, 0), Teams.yellow);
        abobi[Teams.yellow].Add(abobus_go);    

        playerInput = GetComponent<PlayerInput>();

        //touchControls.Player.Click.started += ctx => OnMouseClick(ctx);
        touchControls.Player.Click.performed += ctx => OnMouseClick(ctx);
        touchControls.Player.RightClick.performed += ctx => OnRightMouseClick(ctx);
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

    void Update() {
        // HexCoordinates hc = HexCoordinates.ToOffsetCoordinates(x,z);
        // abobus_go.transform.position = HexCoordinates.FromHexCoordinates(hc);  
    }

    public enum HexCellState {
        out_of_bounds, abobus, empty
    };

    HexCellState CellCheck (HexCoordinates hex_coords)
    {
        if (!hex_grid.CheckHexCoordsOutOfBounds(hex_coords)) {
            return HexCellState.out_of_bounds;
        }
        if (GetListAbobiByHexCoordinates(hex_coords).Count > 0) {
            return HexCellState.abobus;
        }
        return HexCellState.empty;

    }

    public void HandlePossibleTurns (Abobus abobus) 
    {
        List<HexCoordinates> coords_list = abobus.GetPossibleTurns(CellCheck);

        foreach (HexCoordinates hex_coords in coords_list) {
            HexCell hex_cell = hex_grid.GetCellByHexCoordinates(hex_coords);

            HighlightableCell.States state = SolveStateForHexCoordinates(hex_coords);
            hex_cell.GetComponent<HighlightableCell>().SetState(state);
        }
    }

    public List<Abobus> GetListAbobiByHexCoordinates (HexCoordinates hex_coordinates)
    {
        List<Abobus> ans = new List<Abobus>();
        foreach(List<GameObject> abobi_in_team in abobi.Values) {
            foreach(GameObject abobus_go in abobi_in_team) {
                if (abobus_go.GetComponent<Abobus>().hex_coordinates == hex_coordinates) {
                    ans.Add(abobus_go.GetComponent<Abobus>());
                }
            }
        }
        // Debug.Log("On cell " + hex_coordinates.ToString() + " detected " + ans.Count + " abobi");
        return ans;
    } 

    public HighlightableCell.States SolveStateForHexCoordinates(HexCoordinates hex_coordinates)
    {
        List<Abobus> abobi_on_this_cell = GetListAbobiByHexCoordinates(hex_coordinates);
        if (chosen_abobus == null) {
            return HighlightableCell.States.default_;
        }
        foreach (Abobus abobus in abobi_on_this_cell) {
            if (abobus.team != chosen_abobus.team) {
                Debug.Log("HOHOL DETECTED");
                return HighlightableCell.States.highlighted_red;
            }
        }
        foreach (Abobus abobus in abobi_on_this_cell) {
            if (abobus.team == chosen_abobus.team) {
                return HighlightableCell.States.highlighted_blue;
            }
        }
        return HighlightableCell.States.highlighted_green;
    }

    public void OnMouseClick(InputAction.CallbackContext value) {
        Ray inputRay = Camera.main.ScreenPointToRay(touchControls.Player.ClickPosition.ReadValue<Vector2>());
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit)) {
            GameObject hit_go = hit.collider.transform.gameObject;

            Abobus abobus = hit_go.GetComponent<Abobus>();
            if (abobus) {
                if (ReferenceEquals(abobus, chosen_abobus)) {
                    chosen_abobus = null;
                } else if (chosen_abobus) {
                    chosen_abobus.ChangeState();
                    HandlePossibleTurns(chosen_abobus);
                    chosen_abobus = abobus;
                } else {
                    chosen_abobus = abobus;
                }
                
                abobus.ChangeState();
                HandlePossibleTurns(abobus);
            }
            
        }
        
        
    }

    public void OnRightMouseClick(InputAction.CallbackContext value) {
        Ray inputRay = Camera.main.ScreenPointToRay(touchControls.Player.ClickPosition.ReadValue<Vector2>());
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit)) {
            GameObject hit_go = hit.collider.transform.gameObject;

            HexCell hex_cell = hit_go.GetComponent<HexCell>();
            if (hex_cell) {
                if (hex_cell.GetComponent<HighlightableCell>().is_highlighted) {
                    chosen_abobus.ChangeState();
                    HandlePossibleTurns(chosen_abobus);
                    chosen_abobus.MoveToHexCoordinates(hex_cell.hex_coordinates);
                    chosen_abobus = null;
                }
            }
        }
    }

}
