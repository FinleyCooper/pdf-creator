namespace PdfCreator
{
    internal class Program
    {
        static int Main(string[] args)
        {
            string inputFilePath;

            if (args.Length == 0)
            {
                Console.Error.WriteLine("No input file specified, using SampleText.txt as a fallback...");
                inputFilePath = Path.Combine(AppContext.BaseDirectory, "SampleText.txt");
            }
            else
            {
                inputFilePath = args[0];

                if (!Path.IsPathRooted(inputFilePath))
                {
                    inputFilePath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, inputFilePath));
                }
            }


            if (!File.Exists(inputFilePath))
            {
                Console.Error.WriteLine($"The specified input file does not exist: {inputFilePath}");
                return -1;
            }

            string[] lines = File.ReadAllLines(inputFilePath);
            Parser parser = new Parser();
            Content content = parser.ParseFile(lines);
            PDFWriter writer = new PDFWriter();
            writer.Write(content);
            Console.WriteLine("PDF created successfully to output.pdf");
            return 0;
        }
    }
}