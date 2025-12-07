using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using VEF;
using Verse;


namespace AlphaRandom
{
    [HarmonyPatch(typeof(IncidentWorker_RaidEnemy), "GenerateRaidLoot")]
    public static class AlphaRandom_IncidentWorker_RaidEnemy_GenerateRaidLoot_Patch
    {
        public static void Postfix(IncidentParms parms, List<Pawn> pawns)
        {
            if (AlphaRandom_Settings.addLootboxesToRaiders)
            {
                foreach (Pawn pawn in pawns)
                {
                    if (pawn.equipment != null)
                    {
                        if (Rand.Chance(AlphaRandom_Settings.lootboxCommonality))
                        {
                            ThingWithComps thing = (ThingWithComps)ThingMaker.MakeThing(DefDatabase<ThingDef>.AllDefs.Where(x => x.thingCategories?.Contains(InternalDefOf.ARandom_LootBoxes) == true).RandomElement());
                            pawn.inventory.innerContainer.TryAdd(thing);
                        }

                        
                    }


                }
            }


        }
    }
}
