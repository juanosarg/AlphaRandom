using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace AlphaRandom
{
    public class LootBox : ThingWithComps, IOpenable
    {

        public LootBoxExtension cachedLootBoxExtension;

        public LootBoxExtension GetExtension {
            get {              
                if(cachedLootBoxExtension is null)
                {
                    cachedLootBoxExtension= this.def.GetModExtension<LootBoxExtension>();
                }
                return cachedLootBoxExtension;                       
            }
        }

        public virtual int OpenTicks => 300;

        public virtual bool CanOpen => true;

        public virtual void Open()
        {
            
            ThingSetMakerParams parms = default(ThingSetMakerParams);

            parms.totalMarketValueRange = GetExtension.totalMarketValueRange;
            parms.minSingleItemMarketValuePct = GetExtension.minSingleItemMarketValuePct;
            parms.allowNonStackableDuplicates = GetExtension.allowNonStackableDuplicates;
            int fixStupidIntRangeStuff = GetExtension.countRange.RandomInRange;
            parms.countRange = new IntRange(fixStupidIntRangeStuff, fixStupidIntRangeStuff);
           
            List<Thing> list2 = GetExtension.thingSetMakerDef.root.Generate(parms);


            if (list2 != null)
            {
               

                foreach (Thing thing in list2)
                {
                    GenPlace.TryPlaceThing(thing, Position, Map, ThingPlaceMode.Near);
                }

                if (this.Spawned)
                {
                    this.Destroy();
                }

            }


        }


    }
}
