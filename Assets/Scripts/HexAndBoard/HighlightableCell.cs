using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightableCell : MonoBehaviour
{
    public enum States {
        default_, highlighted_red, highlighted_blue, highlighted_green, highlighted_yellow
    };
    private States state;
    public bool is_highlighted;

    public void SetState(States state_)
	{
        if (is_highlighted) {
            is_highlighted = false;
            state_ = States.default_;
        }
		state = state_;
        switch(state_) {
            case States.highlighted_red:
                highlighted_color = Color.red;
                SetHighlighted();
                break;
            case States.highlighted_blue:
                highlighted_color = Color.blue;
                SetHighlighted();
                break;
            case States.highlighted_green:
                highlighted_color = Color.green;
                SetHighlighted();
                break;
            case States.highlighted_yellow:
                highlighted_color = Color.yellow;
                SetHighlighted();
                break;
            default:
                SetDefault();
                break;
        }
	}

    public Transform transform_;
    private HexCell hex_cell;
    private Renderer renderer_; 

    public Material highlighted_material;
    private Material default_material;

    private Color highlighted_color;
    private Color default_color;

    private Vector3 default_position;
    public float highlighted_y_offset;

    void Awake() {
        is_highlighted = false;
        renderer_ = GetComponentInChildren<Renderer>();
        hex_cell = GetComponentInChildren<HexCell>();
        default_material = renderer_.material;
        default_position = hex_cell.transform.position;
    }
    private void SetHighlighted() {
        is_highlighted = true;
        renderer_.material = highlighted_material;
        renderer_.material.color = highlighted_color;
        transform_.position += new Vector3(0, highlighted_y_offset, 0);
    }
    private void SetDefault() {
        renderer_.material = default_material;
        transform_.position += new Vector3(0, -highlighted_y_offset, 0);
    }
}
