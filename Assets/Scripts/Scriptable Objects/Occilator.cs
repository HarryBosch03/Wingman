using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Tools/Occilator")]
public sealed class Occilator : ScriptableObject
{
    public float frequency;
    public float amplitude;
    public float hOffset;
    public float vOffset;
    public bool abs;
    public bool inv;

    public float Get() => Get(Time.time);

    public float Get(float t)
    {
        float val = Get(t, frequency, amplitude, hOffset, vOffset);

        if (abs) val = Mathf.Abs(val);
        if (inv) val = -val;

        return val;
    }

    public static float Get(float t, float frequency = 1, float amplitude = 1, float hOffset = 0, float vOffset = 0)
    {
        return Mathf.Sin((t - hOffset) * frequency * Mathf.PI) * amplitude + vOffset;
    }

    public static implicit operator float (Occilator occilator)
    {
        if (!occilator) return 0.0f;
        else return occilator.Get();
    }
}
