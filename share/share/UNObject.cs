namespace share
{
    public class UTotal
    {
        public int Id { get; set; }
        public string DebtorName { get; set; }
        public string LenderName { get; set; }
        public double Amount { get; set; }
    }

    public class UUser
    {
        public string id { get; set; }
        public string email { get; set; }
    }

    public class SimpleUGroup
    {
        public int id { get; set; }
        public int name { get; set; }
        public int password { get; set; }
        public string uUserId { get; set; }
        public string[] uUser { get; set; }
        public string[] uDebts { get; set; }
        public string[] uEvents { get; set; }
        public string[] uMembers { get; set; }
        public string[] uTotals { get; set; }
    }
}