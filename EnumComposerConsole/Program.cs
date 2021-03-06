using EnumComposer;
using IEnumComposer;
using System;

namespace EnumComposerConsole
{
    internal class Program
    {        
        private static Options _options;
        private static ConsoleColor _originalColor;

        private static int Main(string[] args)
        {
            _originalColor = Console.ForegroundColor;
            _options = new Options();

            try
            {
                if (CommandLine.Parser.Default.ParseArguments(args, _options) == false)
                {
                    return Quit(-1);
                }

                if (string.IsNullOrWhiteSpace(_options.OutputFile))
                {
                    _options.OutputFile = _options.InputFile;
                }

                return Main_Inner();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(DedbugLog.ExceptionMessage(ex));
                return Quit(-1);
            }

            //return Quit(0);
        }

        private static int Main_Inner()
        {
            if (_options.Verbose)
            {
                Console.WriteLine("Input file: {0}", _options.InputFile);
                Console.WriteLine("Output file: {0}", _options.OutputFile);
                Console.WriteLine("Server: {0}", _options.SqlServer);
                Console.WriteLine("Database: {0}", _options.SqlDatabase);
                Console.WriteLine("Press 'Y' to continue");
                //if (Console.ReadLine().ToUpper() != "Y")
                if (Console.ReadKey().Key != ConsoleKey.Y)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Aborted by the user");
                    return Quit(0);
                }
            }

            IEnumLog log = _options.Verbose ? log = new Log() : null;
            IEnumDbReader dbReader = new DbReader(_options.SqlServer, _options.SqlDatabase, log);

            ComposerFiles composer = new ComposerFiles();
            composer.Compose(_options.InputFile, _options.OutputFile, dbReader, log);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("");
            Console.WriteLine("Enumerations has been updated.");

            return Quit(0);
        }

        private static int Quit(int exitCode)
        {
            if (_options.Verbose)
            {
                Console.WriteLine("Press any key to quit.");
                if (_options.NoUser == false)
                {
                    Console.ReadKey();
                }
            }

            Console.ForegroundColor = _originalColor;

            return exitCode;
        }
    }
}