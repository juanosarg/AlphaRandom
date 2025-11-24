using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using System.Net;

namespace AlphaRandom
{

    [DefOf]
    public static class InternalDefOf
    {

        static InternalDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(InternalDefOf));
        }

        public static ThingDef ARandom_FateManipulator_Benign;
        public static ThingDef ARandom_FateManipulator_Dark;
        public static ThingDef ARandom_FateManipulator_Chaotic;



    }
}