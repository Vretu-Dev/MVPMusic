using Exiled.API.Interfaces;

namespace MVPMusic
{
    public class Translation : ITranslation
    {
        public string SettingLabel { get; set; } = "Choose music";
        public string HintDescription { get; set; } = "Select the song that will be played at the end of the round.";
    }
}
