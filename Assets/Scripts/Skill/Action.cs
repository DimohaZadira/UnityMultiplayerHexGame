using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
    public HexCell AppliedTo {
        get;
        set;
    }
    public abstract void Invoke();

    public abstract string DebugMessage();
}
