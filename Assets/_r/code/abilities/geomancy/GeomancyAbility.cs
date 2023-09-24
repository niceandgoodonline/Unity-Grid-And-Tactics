using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GeomancyAbility", menuName = "Ability/Geomancy/GeomancyAbility", order = 1)]
public class GeomancyAbility : AbilityData
{
    public delegate void selfDelegate(GeomancyAbility self);
    public static event selfDelegate Register;
    public static void __Register(GeomancyAbility self)
    {
        if (Register != null) Register(self);
    }

    public override void InitData(SaveLoadData _notUsed)
    {
        __Register(this);
    }
}
