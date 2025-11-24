using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace AlphaRandom
{
    public class InertFateManipulator: ThingWithComps
    {
        public List<ThingDef> possibleThings = new List<ThingDef>() { InternalDefOf.ARandom_FateManipulator_Dark,InternalDefOf.ARandom_FateManipulator_Chaotic,
        InternalDefOf.ARandom_FateManipulator_Benign};

        public IntRange ticksToChange = new IntRange(240, 2400);

        public int tickCounter = 0;
        public int nextChange = 0;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);

            nextChange = ticksToChange.RandomInRange;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref nextChange, "nextChange", 0);
        }

        public override void TickRare()
        {
            base.TickRare();

            tickCounter++;

            if (tickCounter > nextChange)
            {
                if (this.Map != null)
                {
                    Thing newManipulator = ThingMaker.MakeThing(possibleThings.RandomElement());
                    Messages.Message("ARandom_NewManipulator".Translate(newManipulator.Label), MessageTypeDefOf.NeutralEvent);
                    GenPlace.TryPlaceThing(newManipulator, Position, Map, ThingPlaceMode.Near);
                    this.stackCount--;
                    if (this.stackCount <= 0)
                    {
                        Destroy();
                    }
                }


                tickCounter = 0;
                nextChange = ticksToChange.RandomInRange;
            }

        }

    }
}
