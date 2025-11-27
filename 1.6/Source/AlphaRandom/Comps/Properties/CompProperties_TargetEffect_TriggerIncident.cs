using System.Collections.Generic;
using RimWorld;
using Verse;

namespace AlphaRandom
{
    public class CompProperties_TargetEffect_TriggerIncident : CompProperties_UseEffect
    {
        public List<IncidentCategoryDef> incidentTypes;
        public List<IncidentDef> blacklist;
        public List<IncidentDef> whiteList;
        public bool anyIncident = false;
        public int loops = 1;

        public CompProperties_TargetEffect_TriggerIncident()
        {
            compClass = typeof(CompTargetEffect_TriggerIncident);
        }
    }
}