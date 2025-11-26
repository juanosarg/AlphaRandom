using System.Collections.Generic;
using RimWorld;
using Verse;

namespace AlphaRandom
{
    public class CompProperties_TargetEffect_TriggerIncident : CompProperties
    {
        public List<IncidentCategoryDef> incidentTypes;
        public List<IncidentDef> blackList;
        public List<IncidentDef> whiteList;

        public CompProperties_TargetEffect_TriggerIncident()
        {
            compClass = typeof(CompTargetEffect_TriggerIncident);
        }
    }
}