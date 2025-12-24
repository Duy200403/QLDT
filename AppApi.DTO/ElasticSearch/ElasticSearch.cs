using System.Linq.Expressions;
using AppApi.Entities.Models.Base;
using Elasticsearch.Net;
using Nest;

namespace AppApi.DTO.ElasticSearch
{
    public class ElasticSearch<T> where T : class
    {
        private readonly IElasticClient _client;
        private readonly string _index;

        public ElasticSearch(IElasticClient client)
        {
            _client = client;
            _index = typeof(T).Name.ToLowerInvariant(); // emailconfig, emailrule, ...
        }
        public async Task<List<T>> GetResult()
        {
            if ((await _client.Indices.ExistsAsync(typeof(T).Name.ToLower())).Exists)
            {
                var response = await _client.SearchAsync<T>(s => s
                    .Index(_index));
                return response.Documents.ToList();
            }
            return null;
        }

        public async Task<(List<T> Items, long Total)> GetResult(string condition, int pageIndex, int pageSize, bool sortDesc = true, string sortField = "CreatedDate")
        {
            if (!(await _client.Indices.ExistsAsync(typeof(T).Name.ToLower())).Exists)
                return (new List<T>(), 0);

            var from = Math.Max(0, (pageIndex - 1) * pageSize);

            var response = await _client.SearchAsync<T>(s => s
                .Index(_index)
                .From(from) // From là Skip
                .Size(pageSize)
                .Query(q => q.QueryString(qs => qs
                    .Fields(f => f.Field("_all")) // hoặc MultiMatch nếu ES >=7
                    .Query(condition)
                ))
                .Sort(so => sortDesc
                    ? so.Descending(new Field(sortField))
                    : so.Ascending(new Field(sortField)))
            );

            return (response.Documents.ToList(), response.Total);
        }

        public async Task<(List<T> Items, long Total, Dictionary<string, TParent> Parents)> GetResultWithParent<TParent>(
                string condition,
                int pageIndex,
                int pageSize,
                string parentRelationName,                 // e.g. "emailconfig" (the parent relation name in the join mapping)
                bool sortDesc = true,
                string sortField = "CreatedDate",
                IEnumerable<string> childSearchFields = null, // e.g. new[] { "ruleName", "description" }
                bool useKeywordForTextSort = true)
            // where TChild : class
            where TParent : class
        {
            // var indexName = typeof(TChild).Name.ToLowerInvariant();
            var exists = await _client.Indices.ExistsAsync(_index);
            if (!exists.Exists) return (new List<T>(), 0, new Dictionary<string, TParent>());

            if (pageIndex < 1) pageIndex = 1;
            if (pageSize <= 0) pageSize = 10;
            var from = (pageIndex - 1) * pageSize;

            // ---- Child-level query
            Func<QueryContainerDescriptor<T>, QueryContainer> childQuery;
            if (string.IsNullOrWhiteSpace(condition))
            {
                childQuery = q => q.MatchAll();
            }
            else if (childSearchFields != null && childSearchFields.Any())
            {
                childQuery = q => q.MultiMatch(mm => mm
                    .Query(condition)
                    .Type(TextQueryType.BestFields)
                    .Fields(fds =>
                    {
                        var d = fds;
                        foreach (var name in childSearchFields)
                            d = d.Field(name);
                        return d;
                    }));
            }
            else
            {
                // Generic query_string when you don't specify fields
                childQuery = q => q.QueryString(qs => qs
                    .Query(condition)
                    .DefaultOperator(Operator.And));
            }

            // ---- Sorting target
            var sortTarget = sortField;
            if (useKeywordForTextSort && !sortField.EndsWith(".keyword", StringComparison.OrdinalIgnoreCase))
                sortTarget = $"{sortField}.keyword";

            // ---- Search: child with its parent via has_parent + inner_hits
            var response = await _client.SearchAsync<T>(s => s
                .Index(_index)
                .From(from)
                .Size(pageSize)
                .TrackTotalHits(true)
                .Query(q => childQuery(q) && q.HasParent<TParent>(hp => hp
                    .ParentType(parentRelationName)        // relation name defined in your join mapping
                    .Query(pq => pq.MatchAll())            // or put conditions on the parent here
                    .InnerHits(ih => ih
                        .Name("parent")                    // we'll read this back below
                        .Size(1)
                    )
                ))
                .Sort(ss => sortDesc
                    ? ss.Descending(new Field(sortTarget))
                    : ss.Ascending(new Field(sortTarget)))
            );

            if (!response.IsValid)
                throw new Exception($"Elasticsearch query failed: {response.ServerError?.Error?.Reason ?? response.OriginalException?.Message}");

            var items = response.Documents.ToList();
            var total = response.Total;

            // Map: child _id -> parent object
            var parents = new Dictionary<string, TParent>(capacity: response.Hits.Count);
            foreach (var hit in response.Hits)
            {
                TParent parent = default;
                if (hit.InnerHits != null && hit.InnerHits.TryGetValue("parent", out var ih))
                    parent = ih.Documents<TParent>().FirstOrDefault();   // NEST 7: use Documents<T>()

                parents[hit.Id] = parent;
            }

            return (items, total, parents);
        }

        public async Task<EsPageWithIncludes<T, TChild>> GetResultWithNestedInclude<TChild>(
            string condition,
            int pageIndex,
            int pageSize,
            Expression<Func<T, object>> nestedPath,  // e.g. p => p.Rules
            string childMatchField,                   // e.g. "rules.name"
            string sortField = "CreatedDate",
            bool sortDesc = true,
            int innerHitsSize = 50,
            IEnumerable<string> searchFields = null,
            bool useKeywordForTextSort = true)
            // where T : class
            where TChild : class
        {
            // var indexName = typeof(T).Name.ToLowerInvariant();
            var exists = await _client.Indices.ExistsAsync(_index);
            if (!exists.Exists) return new EsPageWithIncludes<T, TChild>();

            if (pageIndex < 1) pageIndex = 1;
            if (pageSize <= 0) pageSize = 10;
            var from = (pageIndex - 1) * pageSize;

            // Build parent-level query
            Func<QueryContainerDescriptor<T>, QueryContainer> parentQuery;
            if (string.IsNullOrWhiteSpace(condition))
            {
                parentQuery = q => q.MatchAll();
            }
            else if (searchFields != null && searchFields.Any())
            {
                parentQuery = q => q.MultiMatch(mm => mm
                    .Query(condition)
                    .Type(TextQueryType.BestFields)
                    .Fields(fds =>
                    {
                        var d = fds;
                        foreach (var name in searchFields.Where(s => !string.IsNullOrWhiteSpace(s)))
                            d = d.Field(name);
                        return d;
                    })
                );
            }
            else
            {
                parentQuery = q => q.QueryString(qs => qs.Query(condition).DefaultOperator(Operator.And));
            }

            string sortTarget = sortField;
            if (useKeywordForTextSort && !sortField.EndsWith(".keyword", StringComparison.OrdinalIgnoreCase))
                sortTarget = $"{sortField}.keyword";

            // Query parents + request inner hits for nested children
            var response = await _client.SearchAsync<T>(s => s
                .Index(_index)
                .From(from)
                .Size(pageSize)
                .TrackTotalHits(true)
                .Query(q => parentQuery(q) && q.Nested(n => n
                    .Path(nestedPath)
                    // You can broaden/narrow the child criteria; example below uses condition on a child field
                    .Query(nq => string.IsNullOrWhiteSpace(condition)
                        ? nq.MatchAll()
                        : nq.QueryString(qs => qs
                            .Fields(f => f.Field(childMatchField)) // e.g. "rules.name"
                            .Query(condition)
                        )
                    )
                    .InnerHits(ih => ih
                        .Name("children")  // key used to read inner hits later
                        .Size(innerHitsSize)
                    )
                ))
                .Sort(ss => sortDesc
                    ? ss.Descending(new Field(sortTarget))
                    : ss.Ascending(new Field(sortTarget)))
            );

            if (!response.IsValid)
                throw new Exception($"Elasticsearch query failed: {response.ServerError?.Error?.Reason ?? response.OriginalException?.Message}");

            var page = new EsPageWithIncludes<T, TChild>
            {
                Items = response.Documents.ToList(),
                Total = response.Total
            };

            // Collect included nested children per parent (_id)
            foreach (var hit in response.Hits)
            {
                var kids = new List<TChild>();

                if (hit.InnerHits != null && hit.InnerHits.TryGetValue("children", out var ih))
                {
                    // NEST deserializes inner hits; just take the typed sources
                    kids.AddRange(ih.Documents<TChild>());
                }

                page.IncludedChildren[hit.Id] = kids;
            }

            return page;
        }

        public async Task<T> GetByIdAsync(
            Guid id,
            IEnumerable<string> sourceIncludes = null,
            IEnumerable<string> sourceExcludes = null,
            string routing = null)
        {
            if (string.IsNullOrWhiteSpace(id.ToString("N"))) return null;

            var resp = await _client.GetAsync<T>(id.ToString("N"), g => g
                .Index(_index)
                .Routing(routing)
                // NEST 7 needs Fields, not IEnumerable<string>
                .SourceIncludes(sourceIncludes != null ? Infer.Fields(sourceIncludes.ToArray()) : null)
                .SourceExcludes(sourceExcludes != null ? Infer.Fields(sourceExcludes.ToArray()) : null)
            );

            return resp.Found ? resp.Source : null;
        }

        public async Task<string> AddAsync(T model, Guid id, bool waitForRefresh = false, string routing = null)
        {
            var resp = await _client.IndexAsync(model, d => d
                .Index(_index)                                           // make index explicit
                .Id(new Id(id.ToString("N")))                      // omit to let ES auto-generate
                .Routing(routing)                                        // needed for parent-child join (route to parent’s id)
                .Refresh(waitForRefresh ? Refresh.WaitFor : Refresh.False)
            );

            if (!resp.IsValid)
                throw new Exception($"Index failed: {resp.ServerError?.Error?.Reason ?? resp.OriginalException?.Message}");

            return resp.Id; // ES-generated or the one you passed
        }

        public async Task<string> UpsertAsync<TPartial>(TPartial partial, Guid id,
            bool waitForRefresh = false, string routing = null)
            where TPartial : class
        {
            var resp = await _client.UpdateAsync<T, TPartial>(id.ToString("N"), u => u
                .Index(_index)
                .Doc(partial)                   // only the fields present in partial
                .DocAsUpsert(true)              // create if not exists
                .Routing(routing)
                .Refresh(waitForRefresh ? Refresh.WaitFor : Refresh.False));

            if (!resp.IsValid)
                throw new Exception($"Upsert failed: {resp.ServerError?.Error?.Reason ?? resp.OriginalException?.Message}");

            return resp.Id;
        }

        public async Task<bool> DeleteByIdAsync(Guid id, bool waitForRefresh = false, string routing = null)
        {
            if (string.IsNullOrWhiteSpace(id.ToString("N"))) return false;

            var resp = await _client.DeleteAsync<T>(id.ToString("N"), d => d
                .Index(_index)
                .Routing(routing) // required if you use parent–child; routing = parentId
                .Refresh(waitForRefresh ? Refresh.WaitFor : Refresh.False));

            if (!resp.IsValid) throw new Exception($"ES delete failed: {resp.ServerError?.Error?.Reason ?? resp.OriginalException?.Message}");
            // Deleted or NotFound are both “ok” states depending on your policy:
            return resp.Result == Result.Deleted || resp.Result == Result.NotFound;
        }

        public async Task<long> DeleteByQueryAsync(
            Func<QueryContainerDescriptor<T>, QueryContainer> query,
            bool waitForRefresh = false,
            long? slices = 5,            // parallel workers (null to let ES decide)
            long? scrollSize = 1000,     // batch size per scroll
            Time timeout = null)         // e.g., new Time("2m")
        {
            var resp = await _client.DeleteByQueryAsync<T>(d => d
                .Index(_index)
                .Query(query)
                .Conflicts(Conflicts.Proceed)
                .Refresh(waitForRefresh)
                .WaitForCompletion(true)
                .Slices(slices)
                .ScrollSize(scrollSize)               // ← instead of BatchSize(...)
                .Timeout(timeout ?? new Time("2m"))
            );

            if (!resp.IsValid)
                throw new Exception($"ES delete_by_query failed: {resp.ServerError?.Error?.Reason ?? resp.OriginalException?.Message}");

            return resp.Deleted; // number of docs deleted
        }

        public async Task<int> DeleteManyByIdsAsync(IEnumerable<Guid> ids, bool waitForRefresh = false, string routing = null)
        {
            var idList = ids?.Where(s => !string.IsNullOrWhiteSpace(s.ToString("N"))).ToList() ?? new();
            if (idList.Count == 0) return 0;

            var resp = await _client.BulkAsync(b =>
            {
                b.Index(_index)
                .Refresh(waitForRefresh ? Refresh.WaitFor : Refresh.False);

                foreach (var id in idList)
                    b.Delete<T>(d => d.Id(id.ToString("N")).Routing(routing));

                return b;
            });

            if (!resp.IsValid) throw new Exception($"ES bulk delete failed: {resp.ServerError?.Error?.Reason ?? resp.OriginalException?.Message}");

            // count successful deletes
            return resp.ItemsWithErrors.Count() == 0 ? idList.Count : idList.Count - resp.ItemsWithErrors.Count();
        }
    }
}