using EnumComposer;
using IEnumComposer;
using System;

namespace EnumComposerConsole
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            ConsoleColor originalColor = Console.ForegroundColor;

            var options = new Options();
            if (CommandLine.Parser.Default.ParseArguments(args, options) == false)
            {
                Console.ReadKey();
                return -1;
            }

            if (options.Verbose)
            {
                Console.WriteLine("Input file: {0}", options.InputFile);
                Console.WriteLine("Output file: {0}", options.OutputFile);
                Console.WriteLine("SQL Server: {0}", options.SqlServer);
                Console.WriteLine("Database: {0}", options.SqlDatabase);
                Console.WriteLine("Press Y to continue");
                if (Console.ReadKey().Key != ConsoleKey.Y)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Aborted by the user");
                    return 0;
                }
            }

            try
            {
                IEnumLog log = options.Verbose ? log = new Log() : null;
                IEnumDbReader dbReader = new EnumSqlDbReader(options.SqlServer, options.SqlDatabase);

                ComposerFiles composer = new ComposerFiles();
                composer.Compose(options.InputFile, options.OutputFile, dbReader, log);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("");
                Console.WriteLine("Enumerations has been updated.");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
            }

            if (options.Verbose)
            {
                Console.WriteLine("Press a key to quit.");
                Console.ReadKey();
            }

            Console.ForegroundColor = originalColor;

            return 0;
        }
    }
}