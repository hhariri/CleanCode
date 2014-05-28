namespace CleanCode.Resources
{
    public class LocalizedStrings
    {

        public LocalizedStrings()
        {
            Warnings = new Warnings();
            Settings = new Settings();
        }
        public Warnings Warnings { get; set; }
        public Settings Settings { get; set; }
    }
}