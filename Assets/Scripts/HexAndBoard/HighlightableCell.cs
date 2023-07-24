using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightableCell : MonoBehaviour
{
    public enum State {
        default_, highlighted_red, highlighted_blue, highlighted_green, highlighted_yellow
    };
    private State state;
    public bool is_highlighted;

    public State GetState()
    {
        return state;
    }

    public void SetState(State state_)
	{
        if (is_highlighted) {
            is_highlighted = false;
            state_ = State.default_;
        }
		state = state_;
        switch(state_) {
            case State.highlighted_red:
                highlighted_color = Color.red;
                SetHighlighted();
                break;
            case State.highlighted_blue:
                highlighted_color = Color.blue;
                SetHighlighted();
                break;
            case State.highlighted_green:
                highlighted_color = Color.green;
                SetHighlighted();
                break;
            case State.highlighted_yellow:
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

    // public Material highlighted_material;

    private Color highlighted_color;
    private Material default_material;
    private Color? default_color;
    private float default_y;
    public float highlighted_y_offset;

    void Awake() {
        is_highlighted = false;
        renderer_ = GetComponent<Renderer>();
        hex_cell = GetComponent<HexCell>();
        default_color = null;
    }
    private void SetHighlighted() {
        is_highlighted = true;
        // renderer_.material = highlighted_material;
        default_color = renderer_.material.color;
        default_y = hex_cell.transform.position.y;
        renderer_.material.color = highlighted_color;
        transform_.position = new Vector3(transform_.position.x, default_y + highlighted_y_offset, transform_.position.z);
    
    }
    private void SetDefault() {
        if (default_color is Color default_color_) {
            renderer_.material.color = default_color_;
        }
        transform_.position = new Vector3(transform_.position.x, default_y, transform_.position.z);
    }
}
