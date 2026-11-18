namespace AdminBackOffice.Models
{
    public sealed class ItemRecord
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string Size { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public byte[] RowVersion { get; set; }
    }

    public sealed class AgentRecord
    {
        public int AgentID { get; set; }
        public string AgentName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}
