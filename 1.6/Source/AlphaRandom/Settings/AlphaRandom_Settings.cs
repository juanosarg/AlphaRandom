using RimWorld;
using UnityEngine;
using Verse;
using System;
using System.Collections.Generic;
using System.Linq;




namespace AlphaRandom
{


    public class AlphaRandom_Settings : ModSettings

    {

       
        public const float baseLootboxCommonality = 0.01f;
        public static float lootboxCommonality = baseLootboxCommonality;
       
        public static bool addLootboxesToRaiders = true;
        


        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref lootboxCommonality, "lootboxCommonality", baseLootboxCommonality, true);
            Scribe_Values.Look(ref addLootboxesToRaiders, "addLootboxesToRaiders", true, true);
          

        }

        public void DoWindowContents(Rect inRect)
        {
            Listing_Standard ls = new Listing_Standard();       

            ls.Begin(inRect);

            ls.CheckboxLabeled("ARandom_AddLootboxesToRaiders".Translate(), ref addLootboxesToRaiders, "ARandom_AddLootboxesToRaidersDesc".Translate());
            if (addLootboxesToRaiders)
            {
                var amountLabel = ls.LabelPlusButton("ARandom_LootboxCommonality".Translate() + ": " + lootboxCommonality.ToStringPercent(), "ARandom_LootboxCommonalityDesc".Translate());
                lootboxCommonality = (float)Math.Round(ls.Slider(lootboxCommonality, 0, 1), 2);

                if (ls.Settings_Button("ARandom_Reset".Translate(), new Rect(0f, amountLabel.position.y + 35, 250f, 29f)))
                {
                    lootboxCommonality = baseLootboxCommonality;
                }
            }

            


            ls.End();
          
            Write();
        }
    }
}
