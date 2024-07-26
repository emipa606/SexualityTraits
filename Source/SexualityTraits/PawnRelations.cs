using System.Linq;
using RimWorld;
using Verse;

namespace SexualityTraits;

public static class PawnRelations
{
    private static readonly PawnRelationDef[] relationsGeneratableNonblood = DefDatabase<PawnRelationDef>
        .AllDefsListForReading.Where(rel => !rel.familyByBloodRelation && rel.generationChanceFactor > 0f).ToArray();

    public static void GeneratePawnRelations(Pawn pawn, ref PawnGenerationRequest request)
    {
        if (!pawn.RaceProps.Humanlike)
        {
            return;
        }

        var localReq = request;
        if (!pawn.kindDef.generateInitialNonFamilyRelations || !Rand.Chance(0.3f))
        {
            return;
        }

        var array = new Pawn[10];
        PawnGenerator_GenerateTraits.ignoreThis = true;
        for (var i = 0; i < 10; i++)
        {
            var kindDef = pawn.kindDef;
            var faction = pawn.Faction;
            Gender? gender = pawn.gender;
            array[i] = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kindDef, faction,
                PawnGenerationContext.NonPlayer, -1, false, false, false, true, false, 1f, false, true, false, true,
                true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, gender));
        }

        PawnGenerator_GenerateTraits.ignoreThis = false;
        var source = GenerateSamples(array, relationsGeneratableNonblood, 40);
        if (!source.TryRandomElementByWeight(
                x => x.Second.generationChanceFactor * x.Second.Worker.GenerationChance(pawn, x.First, localReq),
                out var result) || result.First == null)
        {
            return;
        }

        Find.WorldPawns.PassToWorld(result.First);
        result.Second.Worker.CreateRelation(pawn, result.First, ref request);
        if (!pawn.Faction.IsPlayer)
        {
            return;
        }

        pawn.relations.everSeenByPlayer = true;
        result.First.relations.everSeenByPlayer = true;
    }

    private static Pair<Pawn, PawnRelationDef>[] GenerateSamples(Pawn[] pawns, PawnRelationDef[] relations, int count)
    {
        var array = new Pair<Pawn, PawnRelationDef>[count];
        for (var i = 0; i < count; i++)
        {
            array[i] = new Pair<Pawn, PawnRelationDef>(pawns[Rand.Range(0, pawns.Length)],
                relations[Rand.Range(0, relations.Length)]);
        }

        return array;
    }
}