using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public abstract class AbilityBase : MonoBehaviour
{
    public virtual bool UseState { get; set; }
}
