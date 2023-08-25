// OTL Wizard console runtime

string[] arguments = Environment.GetCommandLineArgs();
string executablePath = arguments[0];


if(arguments.Length < 2)
{
    Console.WriteLine("OTL Wizard Console Runtime v1.0");
    Console.WriteLine("--------------------------------");
    Console.WriteLine("development by Bert Van Overmeir\n\n");
    Console.WriteLine("this application uses OTLWizard framework: " + EntryPoints.getVersion());
    return;
} else
{
    if(EntryPoints.getVersion().Equals("<framework not found>"))
    {
        Console.WriteLine("Error: framework not found");
        return;
    } else
    {
        string maincommand = arguments[1];
        if(maincommand.StartsWith(""))
        {

        }
    }
}
    

public static class EntryPoints
{
    public static string getVersion()
    {
        string currentversion = "<framework not found>";
        try
        {
            string[] lines2 = File.ReadAllLines("data\\version.dat", System.Text.Encoding.UTF8);
            foreach (string item in lines2)
            {
                currentversion = item;
            }
        } catch
        {}
        return currentversion;
    }
}