using HarmonyLib;
using RimWorld;
using Verse;

namespace SexualityTraits;

[HarmonyPatch(typeof(InteractionWorker_RomanceAttempt), nameof(InteractionWorker_RomanceAttempt.RandomSelectionWeight))]
public static class InteractionWorker_RomanceAttempt_RandomSelectionWeight
{
    [HarmonyPriority(0)]
    public static void Postfix(Pawn initiator, Pawn recipient, ref float __result)
    {
        if (!SexualityTraitsMod.Settings.RomanceTweaksEnabled)
        {
            return;
        }

        if (initiator.hasTrait(TraitDefOf.Gay) && (initiator.gender != recipient.gender ||
                                                   !recipient.hasTrait(TraitDefOf.Bisexual) &&
                                                   !recipient.hasTrait(TraitDefOf.Gay)) ||
            recipient.hasTrait(TraitDefOf.Gay) && (initiator.gender != recipient.gender ||
                                                   !initiator.hasTrait(TraitDefOf.Bisexual) &&
                                                   !initiator.hasTrait(TraitDefOf.Gay)) ||
            initiator.hasTrait(TraitDefOf.Bisexual) && initiator.gender != recipient.gender &&
            recipient.hasTrait(TraitDefOf.Gay) || initiator.hasTrait(TraitDefOf.Asexual) ||
            recipient.hasTrait(TraitDefOf.Asexual))
        {
            __result = 0f;
            return;
        }

        if ((initiator.hasTrait(ST_DefOf.ST_Straight) || recipient.hasTrait(ST_DefOf.ST_Straight)) &&
            recipient.gender == initiator.gender)
        {
            __result = 0f;
        }
    }

    private static bool hasTrait(this Pawn pawn, TraitDef traitDef)
    {
        return traitDef != null && (pawn?.story?.traits?.HasTrait(traitDef)).GetValueOrDefault();
    }
}