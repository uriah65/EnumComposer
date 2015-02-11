﻿using CommandLine;
using CommandLine.Text;

namespace EnumComposerConsole
{
    // Define a class to receive parsed values from the command line
    public class Options
    {
        [Option('i', "InputCs", Required = true, HelpText = "C# file to be processed by EmunComposer.")]
        public string InputFile { get; set; }

        [Option('o', "OutputCs", Required = false, HelpText = "C# file generated by EnumComposer, if missing then InputCs file will be used.")]
        public string OutputFile { get; set; }

        [Option('s', "Server", Required = false, HelpText = "Name of SQL server, or a marker: #OledDb, #ODBC, #SQL.")]
        public string SqlServer { get; set; }

        [Option('d', "Database", Required = false, HelpText = "Name of the SQL database, or connection string for the marker.")]
        public string SqlDatabase { get; set; }

        [Option('v', "Verbose", DefaultValue = false, HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

        [Option('u', "NoUser", DefaultValue = false, HelpText = "Automatically accept all user prompts in the verbose mode.")]
        public bool NoUser { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            HelpText help = HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
            help.AddPreOptionsLine("ARGUMENTS:");
            help.AddPostOptionsLine("NOTES:");
            help.AddPostOptionsLine("");
            help.AddPostOptionsLine("- If OutputCs file is not specified, input file InputCs will be overwritten.");
            help.AddPostOptionsLine("");
            help.AddPostOptionsLine("- If both Server/Database attributes are not specified, data source must be set up in EnumSqlCnn attribute.");
            help.AddPostOptionsLine("");
            help.AddPostOptionsLine("- For more details visit https://github.com/uriah65/EnumComposer/wiki.");
            help.AddPostOptionsLine("");
            help.AddPostOptionsLine("");
            return help;
        }
    }
}