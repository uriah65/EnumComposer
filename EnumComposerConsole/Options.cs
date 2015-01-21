﻿using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnumComposerConsole
{
    // Define a class to receive parsed values from the command line
    public class Options
    {
        [Option('i', "input", Required = true, HelpText = "Input C# file that contains source Enumerations.")]
        public string InputFile { get; set; }

        [Option('o', "output", Required = true, HelpText = "Output C# file that will be generated Enumerations.")]
        public string OutputFile { get; set; }

        [Option('s', "sqlserver", Required = true, HelpText = "Name of SQL server.")]
        public string SqlServer { get; set; }

        [Option('d', "database", Required = true, HelpText = "Name of the database.")]
        public string SqlDatabase { get; set; }

        [Option('v', "verbose", DefaultValue = true, HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
