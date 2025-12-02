using System;

[Serializable]
public class UnitData
{
    public int Level { get; private set; }
    public bool isFly { get; private set; }

    public StatData StatData;
    public AbilityData AbilityData;
}
