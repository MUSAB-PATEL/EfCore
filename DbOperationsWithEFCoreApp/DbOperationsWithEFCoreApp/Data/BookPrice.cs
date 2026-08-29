namespace DbOperationsWithEFCoreApp.Data
{
    public class BookPrice
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int CurrencyId { get; set; }
        public double Amount { get; set; }

        public Book Book { get; set; }
        public Currency Currency { get; set; }
    }
}
