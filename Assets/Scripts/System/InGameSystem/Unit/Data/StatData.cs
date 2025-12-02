using System;

[Serializable]
public class StatData
{
    public int Speed;

    public float PhysicalPower;
    public float MagicPower;
    public float PhysicalDefense;
    public float MagicResistance;

    /// <summary>
    /// 움직이는 횟수
    /// </summary>
    public int Movement;

    /// <summary>
    /// 회피율
    /// </summary>
    public float Evasion;
}
