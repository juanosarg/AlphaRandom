
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Noise;
namespace AlphaRandom
{
    public class CompTargetEffect_TriggerIncident : CompUseEffect
    {
        protected CompProperties_TargetEffect_TriggerIncident Props => (CompProperties_TargetEffect_TriggerIncident)props;

        public override void DoEffect(Pawn usedBy)
        {
            for(int i = 0; i < Props.loops; i++)
            {
                IncidentDef incident = ChooseRandomIncident();
                Messages.Message("ARandom_IncidentCaused".Translate(parent.LabelShort, incident.label), MessageTypeDefOf.NeutralEvent);
                if (incident != null)
                {
                    var parms = StorytellerUtility.DefaultParmsNow(incident.category, parent.Map);
                    parms.forced = true;

                    parms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
                   
                    incident.Worker.TryExecute(parms);
                }
            }
            
        }

        public IncidentDef ChooseRandomIncident()
        {


            List<IncidentDef> candidates = new List<IncidentDef>();

            if (Props.anyIncident)
            {
                candidates.AddRange(DefDatabase<IncidentDef>.AllDefsListForReading);
            }

            if (Props.whiteList?.Count > 0)
            {
                candidates.AddRange(Props.whiteList);
            }

            if (Props.incidentTypes?.Count > 0)
            {
                foreach (IncidentDef incident in DefDatabase<IncidentDef>.AllDefsListForReading)
                {
                    if (incident.category != null && Props.incidentTypes.Contains(incident.category))
                    {
                        if (!candidates.Contains(incident))
                        {
                            candidates.Add(incident);
                        }

                    }
                }
            }

            if (Props.blacklist?.Count > 0)
            {
                candidates.RemoveAll(i => Props.blacklist.Contains(i));
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
