namespace OTLWizard.Helpers
{
    public class ErrorContainer
    {
        public string file { get; set; }
        public string errormessage { get; set; }
        public string line { get; set; }
        public string type { get; set; }


        public ErrorContainer(string file, string line, string type, string errormessage)
        {
            this.file = file;
            this.line = line;
            this.type = type;
            this.errormessage = errormessage;
        }
    }
}
