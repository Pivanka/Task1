using TransactionLibrary.Strategy.Context;
using TransactionLibrary.Models.Output;

namespace Task1
{
    public class Client
    {
        private static List<string> allFiles = new List<string>();
        private readonly static string extractPath = System.Configuration.ConfigurationManager.AppSettings["ExtractPath"];
        private readonly static string loadPath = System.Configuration.ConfigurationManager.AppSettings["LoadPath"];
        private static InfoModel Info = new InfoModel();

        public static void FileProcess()
        {
            DirectoryInfo di = new DirectoryInfo(extractPath);

            allFiles = di.GetFiles().Select(file => file.Name).ToList();

            Reader reader = new Reader();
            CreateDateDirectory();
            int i = 0;

            Task[] tasks = new Task[allFiles.Count];
            foreach (var item in allFiles)
            {
                var temp = reader.Run(extractPath + $"\\{item}", loadPath + "\\" + DateOnly.FromDateTime(DateTime.Now).ToString("MM/dd/yyyy").Replace(".", "-") + $"\\output{i}.json");
                Info.ParsedLines += temp.ParsedLines;
                Info.ParsedFiles += temp.ParsedFiles;
                Info.InvalidFiles.AddRange(temp.InvalidFiles);
                Info.FoundErrors += temp.FoundErrors;
                i++;
            }
        }

        private static void CreateDateDirectory()
        {
            try
            {
                string path = loadPath + "\\" + DateOnly.FromDateTime(DateTime.Now).ToString("MM/dd/yyyy");
                path = path.Replace(".", "-");
                // Determine whether the directory exists.
                if (Directory.Exists(path))
                {
                    return;
                }
                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(path);
                //Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }

        public static bool HasNewFiles()
        {
            DirectoryInfo di = new DirectoryInfo(extractPath);
            var files = di.GetFiles().Select(file => file.Name).ToList();
            bool hasNewFiles = false;
            if (files == allFiles)
            {
                hasNewFiles = true;
            }
            return hasNewFiles;
        }

        public static void CreateLogFile(string date)
        {
            var dateFormat = date.Replace(".", "-");
            string fileName = loadPath + "\\" + dateFormat + "\\meta.log";
            if (!File.Exists(fileName))
            {
                using StreamWriter writer = File.CreateText(fileName);

                writer.WriteLine("parsed_files: " + Info.ParsedFiles.ToString());
                writer.WriteLine("parsed_lines: " + Info.ParsedLines.ToString());
                writer.WriteLine("found_errors: " + Info.FoundErrors.ToString());
                writer.WriteLine("invalid_files :");
                foreach (var path in Info.InvalidFiles)
                {
                    writer.WriteLine(path);
                }
            }
        }
    }
}
