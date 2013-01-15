using System.Collections.Generic;

namespace GoogleAnalytics.App
{
    public class Transaction
    {
        public string OrderId { get; set; }
        public string StoreName { get; set; }
        public string Total { get; set; }
        public string Tax { get; set; }
        public string Shipping { get; set; }
        public List<TransactionItem> Items;
    }
}
