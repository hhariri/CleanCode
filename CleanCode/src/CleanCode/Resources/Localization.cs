namespace CleanCode.Resources
{
    public class LocalizedStrings
    {

        public LocalizedStrings()
        {
            Common = new Common();
            Options = new Options();
        }
        public Common Common { get; set; }
        public Options Options { get; set; }
    }
}