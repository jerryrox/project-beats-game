using PBGame.Networking.Maps;

namespace PBGame.Networking.API.Osu
{
    public class OsuAdaptor : IApiAdaptor {

        public string GetMapSortName(MapSortType sortType, bool isDescending)
        {
            string baseName = "";
            switch (sortType)
            {
                case MapSortType.Title: baseName = "title"; break;
                case MapSortType.Artist: baseName = "artist"; break;
                case MapSortType.Difficulty: baseName = "difficulty"; break;
                case MapSortType.Ranked:
                    if(isDescending)
                        return "";
                    baseName = "ranked";
                    break;
                case MapSortType.Rating: baseName = "rating"; break;
                case MapSortType.Plays: baseName = "plays"; break;
                case MapSortType.Favorites: baseName = "favourites"; break;
            }
            if(isDescending)
                return baseName + "_desc";
            return baseName + "_asc";
        }
    }
}
