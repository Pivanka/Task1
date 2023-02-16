using Newtonsoft.Json;
using System.Globalization;
using System.Text.RegularExpressions;
using TransactionLibrary.Models.Input;
using TransactionLibrary.Models.Output;

namespace TransactionLibrary.Strategy
{
    public class TxtReader : IReader
    {
        public override InfoModel Run(string pathExtract, string pathLoad)
        {
            List<Transaction> transactions = new List<Transaction>();
            InfoModel result = new InfoModel();

            string line;

            using (StreamReader file = new StreamReader(pathExtract))
            {
                while ((line = file.ReadLine()) != null)
                {
                    string[] words = line.Split(", ");
                    try
                    {
                        transactions.Add(new Transaction
                        {
                            FirstName = words[0],
                            LastName = words[1],
                            Address = new Address
                            {
                                City = Regex.Replace(words[2], "[^A-Za-z0-9 ]", ""),
                                Street = words[3],
                                NumberBuilding = Int32.Parse(Regex.Replace(words[4], "[^A-Za-z0-9 ]", ""))
                            },
                            Payment = decimal.Parse(words[5].Replace(".", ",")),
                            Date = DateOnly.ParseExact(words[6], "yyyy-dd-mm", CultureInfo.InvariantCulture),
                            AccountNumber = Int64.Parse(words[7]),
                            Service = words[8]
                        });
                        result.ParsedLines++;
                    }
                    catch (Exception)
                    {
                        result.FoundErrors++;
                    }
                }
            }

            var viewModel = CreateResultModel(transactions);

            using (StreamWriter fileLoad = File.CreateText(pathLoad))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(fileLoad, viewModel);
            }

            result.ParsedFiles++;

            return result;
        }
    }
}
