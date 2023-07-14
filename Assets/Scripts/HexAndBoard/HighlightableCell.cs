using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightableCell : Highlightable
{
    public Transform transform_;
    private HexCell hex_cell;
    private Renderer renderer_; 

    public Material highlighted_material;
    private Material default_material;
    public float highlighted_y_offset;

    void Awake() {
        renderer_ = GetComponentInChildren<Renderer>();
        hex_cell = GetComponentInChildren<HexCell>();
        default_material = renderer_.material;
    }
    override public void SetHighlighted(GayManager gm) {
        renderer_.material = highlighted_material;

        HexCoordinates hc = hex_cell.hex_coordinates;
        List<Abobus> abobi_on_this_cell = gm.GetListAbobiByHexCoordinates(hc);
        bool has_enemy = false;

        foreach (Abobus abobus in abobi_on_this_cell) {
            if (abobus.team != gm.chosen_abobus.team) {
                Debug.Log("HOHOL DETECTED");
                has_enemy = true;
                break;
            }
        }
        if (has_enemy) {
            renderer_.material.color = Color.red;
        }

        transform_.position += new Vector3(0, highlighted_y_offset, 0);
    }
    override public void SetDefault() {
        renderer_.material = default_material;
        transform_.position += new Vector3(0, -highlighted_y_offset, 0);}
}
