using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Highlightable: MonoBehaviour
{
    public bool highlighted;
    public void SwitchHighlight()
	{
		highlighted = !highlighted;
		if (highlighted) {
            SetHighlighted();
		} else {
            SetDefault();
		}
	}

    public abstract void SetHighlighted();
    public abstract void SetDefault();

}
