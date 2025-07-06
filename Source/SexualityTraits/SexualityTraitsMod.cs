using Mlie;
using UnityEngine;
using Verse;

namespace SexualityTraits;

internal class SexualityTraitsMod : Mod
{
    public static SexualityTraitsSettings Settings;
    public static string CurrentVersion;

    public SexualityTraitsMod(ModContentPack pack)
        : base(pack)
    {
        CurrentVersion = VersionFromManifest.GetVersionFromModMetaData(pack.ModMetaData);
        Settings = GetSettings<SexualityTraitsSettings>();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        base.DoSettingsWindowContents(inRect);
        Settings.DoSettingsWindowContents(inRect);
    }

    public override string SettingsCategory()
    {
        return "Sexuality Traits";
    }
}