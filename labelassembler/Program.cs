using System;

namespace labelassembler
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
                string fileName = "labels.json";
                if (System.Environment.GetEnvironmentVariable("OS").Contains("Windows") == false)
                {
                    fileName = System.IO.Path.Combine("Documents", fileName);
                }
                inputFile = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), fileName);
            }
            if (args.Length < 2)
            {
                outputFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(inputFile), "labels.png");
            }

#else
            inputConfigFile = args[0];
            outputFile = args[1];
#endif

            string inputContents = System.IO.File.ReadAllText(inputFile);
            Document doc = Newtonsoft.Json.JsonConvert.DeserializeObject<Document>(inputContents);
            doc.Process(outputFile, System.IO.Path.GetDirectoryName(inputFile));
            return 0;
        }
    }
}
