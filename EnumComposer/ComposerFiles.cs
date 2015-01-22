using IEnumComposer;
using System;
using System.IO;

namespace EnumComposer
{
    public class ComposerFiles
    {
        public void Compose(string inputFile, string outputFile, IEnumDbReader dbReader, IEnumLog log = null)
        {
            if (File.Exists(inputFile) == false)
            {
                throw new ApplicationException("Input file wasn't found. File path: '" + inputFile + "'.");
            }

            if (File.Exists(outputFile))
            {
                FileInfo fi = new FileInfo(outputFile);
                if (fi.IsReadOnly)
                {
                    throw new ApplicationException("Output file is read-only. File path: '" + outputFile + "'.");
                }
            }

            ComposerStrings composer = new ComposerStrings(dbReader, log);
            string sourceText = File.ReadAllText(inputFile);
            composer.Compose(sourceText);

            string contents = composer.GetResultFile();

            File.WriteAllText(outputFile, contents);
        }
    }
}