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

    public bool romanceTweaksEnabled = true;

    public List<TraitDef> sexualityTraits;
    private float straightCommonality = 0.2f;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref straightCommonality, "straightCommonality", 0.2f);
        Scribe_Values.Look(ref gayCommonality, "gayCommonality", 0.2f);
        Scribe_Values.Look(ref bisexualCommonality, "bisexualCommonality", 0.5f);
        Scribe_Values.Look(ref asexualCommonality, "asexualCommonality", 0.1f);
        Scribe_Values.Look(ref romanceTweaksEnabled, "romanceTweaksEnabled", true);
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
        var listing_Standard = new Listing_Standard();
        listing_Standard.Begin(rect);
        listing_Standard.SliderLabeled("ST.StraightCommonality".Translate(), ref straightCommonality,
            straightCommonality.ToStringPercent());
        var num = 1E-05f;
        while (straightCommonality + gayCommonality + bisexualCommonality + asexualCommonality > 1f)
        {
            TryModify(ref gayCommonality, 0f - num);
            TryModify(ref bisexualCommonality, 0f - num);
            TryModify(ref asexualCommonality, 0f - num);
        }

        while (straightCommonality + gayCommonality + bisexualCommonality + asexualCommonality < 1f)
        {
            TryModify(ref gayCommonality, num);
            TryModify(ref bisexualCommonality, num);
            TryModify(ref asexualCommonality, num);
        }

        listing_Standard.SliderLabeled("ST.GayCommonality".Translate(), ref gayCommonality,
            gayCommonality.ToStringPercent());
        while (straightCommonality + gayCommonality + bisexualCommonality + asexualCommonality > 1f)
        {
            TryModify(ref straightCommonality, 0f - num);
            TryModify(ref bisexualCommonality, 0f - num);
            TryModify(ref asexualCommonality, 0f - num);
        }

        while (straightCommonality + gayCommonality + bisexualCommonality + asexualCommonality < 1f)
        {
            TryModify(ref straightCommonality, num);
            TryModify(ref bisexualCommonality, num);
            TryModify(ref asexualCommonality, num);
        }

        listing_Standard.SliderLabeled("ST.BisexualCommonality".Translate(), ref bisexualCommonality,
            bisexualCommonality.ToStringPercent());
        while (straightCommonality + gayCommonality + bisexualCommonality + asexualCommonality > 1f)
        {
            TryModify(ref straightCommonality, 0f - num);
            TryModify(ref gayCommonality, 0f - num);
            TryModify(ref asexualCommonality, 0f - num);
        }

        while (straightCommonality + gayCommonality + bisexualCommonality + asexualCommonality < 1f)
        {
            TryModify(ref straightCommonality, num);
            TryModify(ref gayCommonality, num);
            TryModify(ref asexualCommonality, num);
        }

        listing_Standard.SliderLabeled("ST.AsexualCommonality".Translate(), ref asexualCommonality,
            asexualCommonality.ToStringPercent());
        while (straightCommonality + gayCommonality + bisexualCommonality + asexualCommonality > 1f)
        {
            TryModify(ref straightCommonality, 0f - num);
            TryModify(ref gayCommonality, 0f - num);
            TryModify(ref bisexualCommonality, 0f - num);
        }

        while (straightCommonality + gayCommonality + bisexualCommonality + asexualCommonality < 1f)
        {
            TryModify(ref straightCommonality, num);
            TryModify(ref gayCommonality, num);
            TryModify(ref bisexualCommonality, num);
        }

        if (listing_Standard.ButtonText("Reset".Translate()))
        {
            straightCommonality = 0.2f;
            gayCommonality = 0.2f;
            bisexualCommonality = 0.5f;
            asexualCommonality = 0.1f;
        }

        listing_Standard.CheckboxLabeled("ST.EnableRomanceTweaks".Translate(), ref romanceTweaksEnabled,
            "ST.EnableRomanceTweaksTooltip".Translate());

        if (SexualityTraitsMod.CurrentVersion != null)
        {
            listing_Standard.Gap();
            GUI.contentColor = Color.gray;
            listing_Standard.Label("ST.CurrentModVersion".Translate(SexualityTraitsMod.CurrentVersion));
            GUI.contentColor = Color.white;
        }

        listing_Standard.End();
        Write();
    }

    private void TryModify(ref float field, float value)
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