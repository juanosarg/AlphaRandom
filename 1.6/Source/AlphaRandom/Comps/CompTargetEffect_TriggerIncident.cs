
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using Verse;
using Verse.Noise;
namespace AlphaRandom
{
    public class CompTargetEffect_TriggerIncident : CompUseEffect
    {
        protected CompProperties_TargetEffect_TriggerIncident Props => (CompProperties_TargetEffect_TriggerIncident)props;

        public override void DoEffect(Pawn usedBy)
        {
            if (Rand.Chance(Props.archotechBoonChance))
            {
                ChooseRandomBoon(usedBy);
            }
            else
            {
                for (int i = 0; i < Props.loops; i++)
                {
                    IncidentDef incident = ChooseRandomIncident();
                    Messages.Message("ARandom_IncidentCaused".Translate(parent.LabelShort, incident.label), usedBy, MessageTypeDefOf.NeutralEvent);
                    if (incident != null)
                    {
                        var parms = StorytellerUtility.DefaultParmsNow(incident.category, parent.Map);
                        parms.forced = true;

                        parms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;

                        incident.Worker.TryExecute(parms);
                    }
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

        public void ChooseRandomBoon(Pawn pawn)
        {
            IntRange intRange = new IntRange(1, 10);
            switch (intRange.RandomInRange)
            {
                case 1:
                    SkillRecord skill = pawn.skills.skills.Where(x => !x.PermanentlyDisabled).RandomElement();
                    if (skill.Level < 20)
                    {
                        skill.Level += 2;
                    }
                    Messages.Message("ARandom_BoonSkillsLearned".Translate(pawn.NameFullColored, skill.def.LabelCap), pawn, MessageTypeDefOf.PositiveEvent);
                    break;
                case 2:
                    InspirationDef randomAvailableInspirationDef = pawn.mindState.inspirationHandler.GetRandomAvailableInspirationDef();
                    if (randomAvailableInspirationDef != null)
                    {
                        pawn.mindState.inspirationHandler.TryStartInspiration(randomAvailableInspirationDef);
                        Messages.Message("ARandom_BoonInspiration".Translate(pawn.NameFullColored), pawn, MessageTypeDefOf.PositiveEvent);
                    }
                    break;
                case 3:
                    pawn.health.AddHediff(InternalDefOf.ARandom_ArchotechBoon);
                    Messages.Message("ARandom_BoonHediff".Translate(pawn.NameFullColored), pawn, MessageTypeDefOf.PositiveEvent);

                    break;
                case 4:
                    if (ModLister.BiotechInstalled)
                    {
                        GeneDef gene = DefDatabase<GeneDef>.AllDefsListForReading.Where(x => x.biostatMet < 0 && !pawn.genes.HasActiveGene(x)).RandomElement();
                        pawn.genes.AddGene(gene, true);
                        Messages.Message("ARandom_BoonGene".Translate(pawn.NameFullColored, gene.LabelCap), pawn, MessageTypeDefOf.PositiveEvent);
                    }
                    else
                    {
                        pawn.health.AddHediff(InternalDefOf.ARandom_ArchotechBoon);
                        Messages.Message("ARandom_BoonHediff".Translate(pawn.NameFullColored), pawn, MessageTypeDefOf.PositiveEvent);
                    }


                    break;
                case 5:
                    List<string> blackListedTraits = new List<string>();
                    List<BlackListedTraitsDef> allBlackListedTraits = DefDatabase<BlackListedTraitsDef>.AllDefsListForReading;
                    foreach (BlackListedTraitsDef individualList in allBlackListedTraits)
                    {
                        blackListedTraits.AddRange(individualList.blackListedTraits);
                    }

                    List<Trait> allTraitListWithoutBlacklist = pawn.story.traits.allTraits.Where(x => !blackListedTraits.Contains(x.def.defName)).ToList();

                    List<Trait> traitsToRemove = new List<Trait>();

                    int numberToRemove = 5;
                    if (numberToRemove > allTraitListWithoutBlacklist.Count()) { numberToRemove = allTraitListWithoutBlacklist.Count(); }

                    for (int i = 0; i < numberToRemove; i++)
                    {
                        traitsToRemove.Add(allTraitListWithoutBlacklist.Where(x => !traitsToRemove.Contains(x)).RandomElement());

                    }
                    foreach (Trait trait in traitsToRemove)
                    {
                        pawn.story.traits.RemoveTrait(trait);
                    }

                    for (int i = 0; i < 5; i++)
                    {
                        TraitDef addedTrait = DefDatabase<TraitDef>.AllDefsListForReading.Where(x => !blackListedTraits.Contains(x.defName) && !pawn.story.traits.HasTrait(x)).RandomElement();

                        pawn.story.traits.GainTrait(new Trait(addedTrait, addedTrait.degreeDatas.RandomElement().degree, forced: true));

                    }
                    Messages.Message("ARandom_BoonTraits".Translate(pawn.NameFullColored), pawn, MessageTypeDefOf.PositiveEvent);

                    break;

                case 6:
                    pawn.Map.fogGrid.ClearAllFog();
                    Messages.Message("ARandom_BoonSight".Translate(pawn.NameFullColored), pawn, MessageTypeDefOf.PositiveEvent);

                    break;
                case 7:

                    foreach (Apparel apparel in pawn.apparel.WornApparel)
                    {
                        if (apparel.Stuff != null)
                        {
                            apparel.SetStuffDirect(ThingDefOf.Gold);
                            StatDefOf.MaxHitPoints.Worker.ClearCacheForThing(apparel);
                            apparel.HitPoints = apparel.MaxHitPoints;
                        }

                    }
                    Messages.Message("ARandom_BoonMidasTouch".Translate(pawn.NameFullColored), pawn, MessageTypeDefOf.PositiveEvent);


                    break;

                case 8:

                    List<Thing> filthList = pawn.Map.listerFilthInHomeArea.FilthInHomeArea;
                   
                    for (int i = 0; i < filthList.Count; i++)
                    {
                        filthList[i].DeSpawn();
                        
                    }
                    List<Thing> repairablesList = pawn.Map.listerBuildingsRepairable.RepairableBuildings(Faction.OfPlayerSilentFail);

                    for (int i = 0; i < repairablesList.Count; i++)
                    {
                        StatDefOf.MaxHitPoints.Worker.ClearCacheForThing(repairablesList[i]);
                        repairablesList[i].HitPoints = repairablesList[i].MaxHitPoints;

                    }
                    Messages.Message("ARandom_BoonCleaningRepairing".Translate(), pawn, MessageTypeDefOf.PositiveEvent);



                    break;
                case 9:

                    foreach (Thing equipment in pawn.equipment.AllEquipmentListForReading)
                    {
                        if (equipment.TryGetComp<CompQuality>() != null)
                        {
                            CompQuality comp = equipment.TryGetComp<CompQuality>();
                            if (comp.Quality <= QualityCategory.Legendary)
                            {
                                comp.SetQuality(comp.Quality + 1,null);
                            }
                        }

                    }
                    Messages.Message("ARandom_BoonQuality".Translate(pawn.NameFullColored), pawn, MessageTypeDefOf.PositiveEvent);


                    break;
                case 10:

                    for(int i= 0; i < 6; i++)
                    {
                        Pawn chicken = PawnGenerator.GeneratePawn(new PawnGenerationRequest(InternalDefOf.Chicken, pawn.Faction, PawnGenerationContext.NonPlayer, null, fixedBiologicalAge: 2));
                        GenSpawn.Spawn(chicken, parent.Position, pawn.Map, WipeMode.VanishOrMoveAside);
                        if (CellFinder.TryFindRandomSpawnCellForPawnNear(parent.Position, pawn.Map, out var result, 5, (IntVec3 c) => GenSight.LineOfSight(parent.Position, c, pawn.Map, skipFirstCell: true)))
                        {
                            chicken.rotationTracker.FaceCell(result);
                            PawnFlyer pawnFlyer = PawnFlyer.MakeFlyer(ThingDefOf.PawnFlyer_Stun, chicken, result, null, null);
                            if (pawnFlyer != null)
                            {
                                GenSpawn.Spawn(pawnFlyer, result, pawn.Map);
                            }
                        }

                    }
                    Messages.Message("ARandom_BoonChickens".Translate(pawn.NameFullColored), pawn, MessageTypeDefOf.PositiveEvent);


                    break;
                default:
                    break;
            }

        }

    }
}
