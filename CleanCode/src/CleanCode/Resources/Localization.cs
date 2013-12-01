namespace CleanCode.Resources
{
    public class LocalizedStrings
    {

        public LocalizedStrings()
        {
            Common = new Common();
            Settings = new Settings();
        }
        public Common Common { get; set; }
        public Settings Settings { get; set; }
    }
}