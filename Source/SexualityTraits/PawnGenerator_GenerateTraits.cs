using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace SexualityTraits;

[HarmonyPatch(typeof(PawnGenerator), "GenerateTraits")]
public static class PawnGenerator_GenerateTraits
{
    public static bool IgnoreThis;

    private static readonly MethodInfo defDatabaseRemoveMethodInfo =
        AccessTools.Method(typeof(DefDatabase<TraitDef>), "Remove");

    public static void Prefix()
    {
        foreach (var sexualityTrait in SexualityTraitsMod.Settings.SexualityTraits)
        {
            if (DefDatabase<TraitDef>.GetNamedSilentFail(sexualityTrait.defName) != null)
            {
                defDatabaseRemoveMethodInfo.Invoke(null, [sexualityTrait]);
            }
        }
    }

    public static void Postfix(Pawn pawn, PawnGenerationRequest request)
    {
        if (!pawn.story.traits.allTraits.Any(x => SexualityTraitsMod.Settings.SexualityTraits.Contains(x.def)))
        {
            var partner = pawn.relations.DirectRelations.Find(x =>
                LovePartnerRelationUtility.IsLovePartnerRelation(x.def) ||
                LovePartnerRelationUtility.IsExLovePartnerRelation(x.def));

            bool canHaveTrait(TraitDef t)
            {
                if (t == TraitDefOf.Gay)
                {
                    if (partner != null && partner.otherPawn.gender != pawn.gender)
                    {
                        return false;
                    }
                }
                else if (t == TraitDefOf.Asexual)
                {
                    if (partner != null)
                    {
                        return false;
                    }
                }
                else if (t == ST_DefOf.ST_Straight && partner != null && partner.otherPawn.gender == pawn.gender)
                {
                    return false;
                }

                return true;
            }

            if (SexualityTraitsMod.Settings.SexualityTraits.Where(canHaveTrait)
                .TryRandomElementByWeight(x => SexualityTraitsMod.Settings.GetCommonalityFor(x), out var result))
            {
                var trait = new Trait(result, PawnGenerator.RandomTraitDegree(result));
                pawn.story.traits.GainTrait(trait);
            }
        }

        if (!IgnoreThis && pawn.story.traits.HasTrait(TraitDefOf.Gay))
        {
            var directPawnRelation = pawn.relations.DirectRelations.Find(x =>
                LovePartnerRelationUtility.IsLovePartnerRelation(x.def) ||
                LovePartnerRelationUtility.IsExLovePartnerRelation(x.def));
            if (directPawnRelation == null)
            {
                request.AllowGay = true;
                PawnRelations.GeneratePawnRelations(pawn, ref request);
            }
        }

        foreach (var sexualityTrait in SexualityTraitsMod.Settings.SexualityTraits)
        {
            if (DefDatabase<TraitDef>.GetNamedSilentFail(sexualityTrait.defName) == null)
            {
                DefDatabase<TraitDef>.Add(sexualityTrait);
            }
        }
    }
}