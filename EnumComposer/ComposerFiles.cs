using IEnumComposer;
using System;
using System.IO;

namespace EnumComposer
{
    public class ComposerFiles
    {
        public void Compose(string inputFile, string outputFile, IEnumDbReader dbReader)
        {
            if (File.Exists(inputFile) == false)
            {
                throw new ApplicationException("Input file '" + inputFile + "' wasn't found.");
            }

            if (File.Exists(outputFile))
            {
                FileInfo fi = new FileInfo(outputFile);
                if (fi.IsReadOnly)
                {
                    throw new ApplicationException("Output file is '" + outputFile + "'read-only.");
                }
            }


            ComposerStrings composer = new ComposerStrings(dbReader);
            string sourceText = File.ReadAllText(inputFile);
            composer.Compose(sourceText);

            string contents = composer.GetResultFile();

            File.WriteAllText(outputFile, contents);
        }
    }
}