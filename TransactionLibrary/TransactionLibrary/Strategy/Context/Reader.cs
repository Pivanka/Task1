using TransactionLibrary.Models.Output;

namespace TransactionLibrary.Strategy.Context
{
    public class Reader
    {
        IReader reader;
        public InfoModel Run(string pathExtract, string pathLoad)
        {
            InfoModel info = new InfoModel();
            if (pathExtract.Contains(".csv"))
            {
                reader = new CsvReader();
                info = reader.Run(pathExtract, pathLoad);
            }
            else if (pathExtract.Contains(".txt"))
            {
                reader = new TxtReader();
                info = reader.Run(pathExtract, pathLoad);
            }
            else
            {
                info.InvalidFiles.Add(pathExtract);
            }
            return info;
        }
    }
}
