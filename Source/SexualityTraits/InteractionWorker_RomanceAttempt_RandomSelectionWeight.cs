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
        if (!SexualityTraitsMod.settings.romanceTweaksEnabled)
        {
            return;
        }

        if (initiator.HasTrait(TraitDefOf.Gay) && (initiator.gender != recipient.gender ||
                                                   !recipient.HasTrait(TraitDefOf.Bisexual) &&
                                                   !recipient.HasTrait(TraitDefOf.Gay)))
        {
            __result = 0f;
            return;
        }

        if (recipient.HasTrait(TraitDefOf.Gay) && (initiator.gender != recipient.gender ||
                                                   !initiator.HasTrait(TraitDefOf.Bisexual) &&
                                                   !initiator.HasTrait(TraitDefOf.Gay)))
        {
            __result = 0f;
            return;
        }

        if (initiator.HasTrait(TraitDefOf.Bisexual) && initiator.gender != recipient.gender &&
            recipient.HasTrait(TraitDefOf.Gay))
        {
            __result = 0f;
            return;
        }

        if (initiator.HasTrait(TraitDefOf.Asexual) || recipient.HasTrait(TraitDefOf.Asexual))
        {
            __result = 0f;
            return;
        }

        if ((initiator.HasTrait(ST_DefOf.ST_Straight) || recipient.HasTrait(ST_DefOf.ST_Straight)) &&
            recipient.gender == initiator.gender)
        {
            __result = 0f;
        }
    }

    public static bool HasTrait(this Pawn pawn, TraitDef traitDef)
    {
        return traitDef != null && (pawn?.story?.traits?.HasTrait(traitDef)).GetValueOrDefault();
    }
}