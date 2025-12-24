namespace AppApi.DTO.ElasticSearch
{
    public class EsPageWithIncludes<TParent, TChild>
    {
        public List<TParent> Items { get; set; } = new();
        public long Total { get; set; }
        // key = parent _id, value = included matching children
        public Dictionary<string, List<TChild>> IncludedChildren { get; set; } = new();
    }
}