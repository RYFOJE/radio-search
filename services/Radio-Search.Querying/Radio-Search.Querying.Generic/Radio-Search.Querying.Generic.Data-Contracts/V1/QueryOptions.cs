namespace Radio_Search.Querying.Generic.Data_Contracts.V1
{
    public abstract class QueryOptions
    {
        public int PageSize { get; set; } = 100;
        public int? LastSeenCursor { get; set; }
    }
}
