using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace SexualityTraits;

[StaticConstructorOnStartup]
internal static class HarmonyInit
{
    static HarmonyInit()
    {
        new Harmony("SexualityTraits.Mod").PatchAll(Assembly.GetExecutingAssembly());
        SexualityTraitsMod.Settings.SexualityTraits =
        [
            ST_DefOf.ST_Straight,
            TraitDefOf.Gay,
            TraitDefOf.Bisexual,
            TraitDefOf.Asexual
        ];
    }
}