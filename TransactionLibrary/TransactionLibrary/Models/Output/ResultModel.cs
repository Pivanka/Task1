namespace TransactionLibrary.Models.Output
{
    public class ResultModel
    {
        public string City { get; set; }
        public IEnumerable<Service> Services { get; set; }
        public decimal Total { get; set; }
    }
}
