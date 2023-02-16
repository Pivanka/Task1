namespace TransactionLibrary.Models.Output
{
    public class Service
    {
        public string Name { get; set; }
        public IEnumerable<Payer> Payers { get; set; }
        public decimal Total { get; set; }
    }
}
