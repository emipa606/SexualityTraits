using Mlie;
using UnityEngine;
using Verse;

namespace SexualityTraits;

internal class SexualityTraitsMod : Mod
{
    public static SexualityTraitsSettings settings;
    public static string CurrentVersion;

    public SexualityTraitsMod(ModContentPack pack)
        : base(pack)
    {
        CurrentVersion = VersionFromManifest.GetVersionFromModMetaData(pack.ModMetaData);
        settings = GetSettings<SexualityTraitsSettings>();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        base.DoSettingsWindowContents(inRect);
        settings.DoSettingsWindowContents(inRect);
    }

    public override string SettingsCategory()
    {
        return "Sexuality Traits";
    }
}