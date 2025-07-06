using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace SexualityTraits;

internal class SexualityTraitsSettings : ModSettings
{
    private float asexualCommonality = 0.1f;

    private float bisexualCommonality = 0.5f;

    private float gayCommonality = 0.2f;

    public bool RomanceTweaksEnabled = true;

    public List<TraitDef> SexualityTraits;
    private float straightCommonality = 0.2f;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref straightCommonality, "straightCommonality", 0.2f);
        Scribe_Values.Look(ref gayCommonality, "gayCommonality", 0.2f);
        Scribe_Values.Look(ref bisexualCommonality, "bisexualCommonality", 0.5f);
        Scribe_Values.Look(ref asexualCommonality, "asexualCommonality", 0.1f);
        Scribe_Values.Look(ref RomanceTweaksEnabled, "romanceTweaksEnabled", true);
    }

    public float GetCommonalityFor(TraitDef traitDef)
    {
        return traitDef.defName switch
        {
            "ST_Straight" => straightCommonality,
            "Bisexual" => bisexualCommonality,
            "Gay" => gayCommonality,
            "Asexual" => asexualCommonality,
            _ => 1f
        };
    }

    public void DoSettingsWindowContents(Rect inRect)
    {
        var rect = new Rect(inRect.x, inRect.y, inRect.width, inRect.height);
        var listingStandard = new Listing_Standard();
        listingStandard.Begin(rect);
        listingStandard.SliderLabeled("ST.StraightCommonality".Translate(), ref straightCommonality,
            straightCommonality.ToStringPercent());
        var num = 1E-05f;
        while (straightCommonality + gayCommonality + bisexualCommonality + asexualCommonality > 1f)
        {
            tryModify(ref gayCommonality, 0f - num);
            tryModify(ref bisexualCommonality, 0f - num);
            tryModify(ref asexualCommonality, 0f - num);
        }

        while (straightCommonality + gayCommonality + bisexualCommonality + asexualCommonality < 1f)
        {
            tryModify(ref gayCommonality, num);
            tryModify(ref bisexualCommonality, num);
            tryModify(ref asexualCommonality, num);
        }

        listingStandard.SliderLabeled("ST.GayCommonality".Translate(), ref gayCommonality,
            gayCommonality.ToStringPercent());
        while (straightCommonality + gayCommonality + bisexualCommonality + asexualCommonality > 1f)
        {
            tryModify(ref straightCommonality, 0f - num);
            tryModify(ref bisexualCommonality, 0f - num);
            tryModify(ref asexualCommonality, 0f - num);
        }

        while (straightCommonality + gayCommonality + bisexualCommonality + asexualCommonality < 1f)
        {
            tryModify(ref straightCommonality, num);
            tryModify(ref bisexualCommonality, num);
            tryModify(ref asexualCommonality, num);
        }

        listingStandard.SliderLabeled("ST.BisexualCommonality".Translate(), ref bisexualCommonality,
            bisexualCommonality.ToStringPercent());
        while (straightCommonality + gayCommonality + bisexualCommonality + asexualCommonality > 1f)
        {
            tryModify(ref straightCommonality, 0f - num);
            tryModify(ref gayCommonality, 0f - num);
            tryModify(ref asexualCommonality, 0f - num);
        }

        while (straightCommonality + gayCommonality + bisexualCommonality + asexualCommonality < 1f)
        {
            tryModify(ref straightCommonality, num);
            tryModify(ref gayCommonality, num);
            tryModify(ref asexualCommonality, num);
        }

        listingStandard.SliderLabeled("ST.AsexualCommonality".Translate(), ref asexualCommonality,
            asexualCommonality.ToStringPercent());
        while (straightCommonality + gayCommonality + bisexualCommonality + asexualCommonality > 1f)
        {
            tryModify(ref straightCommonality, 0f - num);
            tryModify(ref gayCommonality, 0f - num);
            tryModify(ref bisexualCommonality, 0f - num);
        }

        while (straightCommonality + gayCommonality + bisexualCommonality + asexualCommonality < 1f)
        {
            tryModify(ref straightCommonality, num);
            tryModify(ref gayCommonality, num);
            tryModify(ref bisexualCommonality, num);
        }

        if (listingStandard.ButtonText("Reset".Translate()))
        {
            straightCommonality = 0.2f;
            gayCommonality = 0.2f;
            bisexualCommonality = 0.5f;
            asexualCommonality = 0.1f;
        }

        listingStandard.CheckboxLabeled("ST.EnableRomanceTweaks".Translate(), ref RomanceTweaksEnabled,
            "ST.EnableRomanceTweaksTooltip".Translate());

        if (SexualityTraitsMod.CurrentVersion != null)
        {
            listingStandard.Gap();
            GUI.contentColor = Color.gray;
            listingStandard.Label("ST.CurrentModVersion".Translate(SexualityTraitsMod.CurrentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard.End();
        Write();
    }

    private static void tryModify(ref float field, float value)
    {
        if (value > 0f)
        {
            if (field < 1f)
            {
                field += value * field;
            }
        }
        else if (field > 0f)
        {
            field += value * field;
        }

        field = Mathf.Clamp01(field);
    }
}