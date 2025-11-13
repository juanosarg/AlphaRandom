using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace AlphaRandom;

public class AlphaRandom_Mod : Mod
{
    public AlphaRandom_Mod(ModContentPack content) : base(content)
    {
      
        settings = GetSettings<AlphaRandom_Settings>();
    }

    public static AlphaRandom_Settings settings;

    public override string SettingsCategory()
    {
        return "Alpha Random";
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        settings.DoWindowContents(inRect);
    }

}
