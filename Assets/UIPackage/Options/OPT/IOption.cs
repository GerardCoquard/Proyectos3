using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOption
{
    public OptionType type {get;}
    public void Reset();
}
public enum OptionType
{
    Audio,
    Graphics,
    Others
}
