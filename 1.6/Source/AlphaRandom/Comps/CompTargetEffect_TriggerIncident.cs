
using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.Noise;
namespace AlphaRandom
{
    public class CompTargetEffect_TriggerIncident : CompTargetEffect
    {
        protected CompProperties_TargetEffect_TriggerIncident Props => (CompProperties_TargetEffect_TriggerIncident)props;

        public override void DoEffectOn(Pawn user, Thing target)
        {
            IncidentDef incident = ChooseRandomIncident();
            if (incident != null) {
                var parms = StorytellerUtility.DefaultParmsNow(incident.category, parent.Map);
                parms.forced = true;
              
                incident.Worker.TryExecute(parms);
            }
        }

        public IncidentDef ChooseRandomIncident()
        {
           

            List<IncidentDef> candidates = new List<IncidentDef>();
         
            if (Props.whiteList?.Count > 0)
            {
                candidates.AddRange(Props.whiteList);
            }
            else
            {
            
                if (Props.incidentTypes?.Count > 0)
                {
                    foreach (IncidentDef incident in DefDatabase<IncidentDef>.AllDefsListForReading)
                    {
                        if (incident.category != null && Props.incidentTypes.Contains(incident.category))
                        {
                            candidates.Add(incident);
                        }
                    }
                }
            }


            if (Props.blackList?.Count > 0)
            {
                candidates.RemoveAll(i => Props.blackList.Contains(i));
            }

           
            if (candidates.Count == 0)
            {
                Log.Warning("[AlphaRandom] No incidents available to choose from after filtering.");
                return null;
            }

          
            return candidates.RandomElement();
        }
    }
}
