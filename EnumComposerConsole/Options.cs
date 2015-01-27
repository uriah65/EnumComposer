using CommandLine;
using CommandLine.Text;

namespace EnumComposerConsole
{
    // Define a class to receive parsed values from the command line
    public class Options
    {
        [Option('i', "input", Required = true, HelpText = "Input C# file that contains source Enumerations.")]
        public string InputFile { get; set; }

        [Option('o', "output", Required = false, HelpText = "Output C# file that will be generated Enumerations.")]
        public string OutputFile { get; set; }

        [Option('s', "sqlserver", Required = false, HelpText = "Name of SQL server.")]
        public string SqlServer { get; set; }

        [Option('d', "database", Required = false, HelpText = "Name of the database.")]
        public string SqlDatabase { get; set; }

        [Option('v', "verbose", DefaultValue = false, HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            HelpText help = HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
            help.AddPreOptionsLine("ARGUMENTS:");
            help.AddPostOptionsLine("USEAGE:");
            help.AddPostOptionsLine("");
            help.AddPostOptionsLine("If output file is not specified, input file will be used instead.");
            help.AddPostOptionsLine("If sqlserver/database are not specified, EnumSqlCnn attribute should be set up in C# code.");
            help.AddPostOptionsLine("");
            help.AddPostOptionsLine("");
            return help;
        }
    }
}