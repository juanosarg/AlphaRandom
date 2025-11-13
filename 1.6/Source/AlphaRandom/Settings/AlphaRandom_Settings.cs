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

        public const int minWeaponTraitsBase = 1;
        public const int maxWeaponTraitsBase = 3;
        public static int minWeaponTraits = minWeaponTraitsBase;
        public static int maxWeaponTraits = maxWeaponTraitsBase;
        public static bool sendWeaponPods = false;
        public static bool addWeaponsToMoreMercs = false;
        public static bool addWeaponsToAllRaids = false;
        public static bool makeRaidWeaponsBiocoded = true;
        public static bool makeRaidWeaponsDestroyedOnDrop = false;
        public static bool makeMoreCommonAsQuestRewards = true;


        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref minWeaponTraits, "minWeaponTraits", minWeaponTraitsBase, true);
            Scribe_Values.Look(ref maxWeaponTraits, "maxWeaponTraits", maxWeaponTraitsBase, true);
            Scribe_Values.Look(ref sendWeaponPods, "sendWeaponPods", false, true);
            Scribe_Values.Look(ref addWeaponsToMoreMercs, "addWeaponsToMoreMercs", false, true);
            Scribe_Values.Look(ref addWeaponsToAllRaids, "addWeaponsToAllRaids", false, true);
            Scribe_Values.Look(ref makeRaidWeaponsBiocoded, "makeRaidWeaponsBiocoded", true, true);
            Scribe_Values.Look(ref makeRaidWeaponsDestroyedOnDrop, "makeRaidWeaponsDestroyedOnDrop", false, true);

            Scribe_Values.Look(ref makeMoreCommonAsQuestRewards, "makeMoreCommonAsQuestRewards", false, true);


        }

        public void DoWindowContents(Rect inRect)
        {
            Listing_Standard ls = new Listing_Standard();       

            ls.Begin(inRect);

            var minLabel = ls.LabelPlusButton("AArmoury_MinWeaponTraits".Translate() + ": " + minWeaponTraits, "AArmoury_MinWeaponTraitsDesc".Translate());
            minWeaponTraits = (int)Math.Round(ls.Slider(minWeaponTraits, 1, maxWeaponTraits-1), 0);

            if (ls.Settings_Button("AArmoury_Reset".Translate(), new Rect(0f, minLabel.position.y + 35, 250f, 29f)))
            {
                minWeaponTraits = minWeaponTraitsBase;
            }

            var maxLabel = ls.LabelPlusButton("AArmoury_MaxWeaponTraits".Translate() + ": " + maxWeaponTraits, "AArmoury_MaxWeaponTraitsDesc".Translate());
            maxWeaponTraits = (int)Math.Round(ls.Slider(maxWeaponTraits, minWeaponTraits+1, 10f), 0);

            if (ls.Settings_Button("AArmoury_Reset".Translate(), new Rect(0f, maxLabel.position.y + 35, 250f, 29f)))
            {
                maxWeaponTraits = maxWeaponTraitsBase;
            }

            ls.CheckboxLabeled("AArmoury_SendWeaponPods".Translate(), ref sendWeaponPods, "AArmoury_SendWeaponPodsDesc".Translate());
            ls.CheckboxLabeled("AArmoury_AddWeaponsToMoreMercs".Translate(), ref addWeaponsToMoreMercs, "AArmoury_AddWeaponsToMoreMercsDesc".Translate());
            ls.CheckboxLabeled("AArmoury_AddWeaponsToAllRaids".Translate(), ref addWeaponsToAllRaids, "AArmoury_AddWeaponsToAllRaidsDesc".Translate());
            if (addWeaponsToAllRaids)
            {
                ls.CheckboxLabeled("AArmoury_MakeRaidWeaponsBiocoded".Translate(), ref makeRaidWeaponsBiocoded, "AArmoury_MakeRaidWeaponsBiocodedDesc".Translate());
                ls.CheckboxLabeled("AArmoury_MakeRaidWeaponsDestroyedOnDrop".Translate(), ref makeRaidWeaponsDestroyedOnDrop, "AArmoury_MakeRaidWeaponsDestroyedOnDropDesc".Translate());

            }
            ls.CheckboxLabeled("AArmoury_MakeMoreCommonAsQuestRewards".Translate(), ref makeMoreCommonAsQuestRewards, "AArmoury_MakeMoreCommonAsQuestRewardsDesc".Translate());


            ls.End();
          
            Write();
        }
    }
}
