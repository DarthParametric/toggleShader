using UnityModManagerNet;

namespace NoFilmGrain
{
    public class Settings : UnityModManager.ModSettings
    {
        //settings go here
        public string lastModVersion;
        public string localizationFileName;
        public string modPath;
        public static bool disableFilmGrain = true; 
    }
}
