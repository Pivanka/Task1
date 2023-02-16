using TransactionLibrary.Models.Input;
using TransactionLibrary.Models.Output;

namespace TransactionLibrary.Strategy
{
    public abstract class IReader
    {
        public abstract InfoModel Run(string pathExtract, string pathLoad);

        protected IEnumerable<ResultModel> CreateResultModel(List<Transaction> transactions)
        {
            return from b in transactions
                   group b by b.Address.City into g
                   select new ResultModel
                   {
                       City = g.Key,
                       Services = from i in g
                                  group i by i.Service into c
                                  select new Service
                                  {
                                      Name = c.Key,
                                      Payers = g.Where(b => b.Service == c.Key).Select(p => new Payer
                                      {
                                          Name = p.FirstName + " " + p.LastName,
                                          Payment = p.Payment,
                                          Date = p.Date,
                                          AccountNumber = p.AccountNumber
                                      }),
                                      Total = g.Where(b => b.Service == c.Key).Select(x => x.Payment).Sum()
                                  },
                       Total = g.Select(x => x.Payment).Sum()
                   };
        }
    }
}
