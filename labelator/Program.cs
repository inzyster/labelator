using System;

namespace labelator
{
    class Program
    {
        static int Main(string[] args)
        {
            string inputFile = null;
            string outputFile = null;
#if DEBUG
            if (args.Length < 1)
            {
                inputFile = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "Documents/label1.txt");
            }
            if (args.Length < 2)
            {
                outputFile = "out.png";
            }

#else
            inputFile = args[0];
            outputFile = args[1];
#endif
            string[] input = System.IO.File.ReadAllLines(inputFile);
            Document doc = new Document();
            doc.Parse(input);
            doc.Render(outputFile);
            return 0;
        }
    }
}
