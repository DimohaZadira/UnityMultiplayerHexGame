using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightableCell : Highlightable
{
    public Transform transform_;
    private Renderer renderer_; 
    public Material highlighted_material;
    private Material default_material;
    public float highlighted_y_offset;
    void Awake() {
        renderer_ = GetComponentInChildren<Renderer>();
        default_material = renderer_.material;
    }
    override public void SetHighlighted() {
        renderer_.material = highlighted_material;
        transform_.position = new Vector3(transform_.position.x, transform_.position.y + highlighted_y_offset, transform_.position.z);
    }
    override public void SetDefault() {
        renderer_.material = default_material;
        transform_.position = new Vector3(transform_.position.x, transform_.position.y - highlighted_y_offset, transform_.position.z);
    }
}
