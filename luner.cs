using Terraria.ModLoader;

namespace luner
{
    public class luner : Mod
    {

        public static luner Instance { get; private set; }
        public override void Unload()
        {
            MrPlagueRaces.Common.Races.RaceLoader.Races.Clear();
            MrPlagueRaces.Common.Races.RaceLoader.RacesByFullNames.Clear();
        }
    }
}