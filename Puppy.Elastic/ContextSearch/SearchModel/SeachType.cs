﻿namespace Puppy.Elastic.ContextSearch.SearchModel
{
    /// <summary>
    ///     There are different execution paths that can be done when executing a distributed search.
    ///     The distributed search operation needs to be scattered to all the relevant shards and
    ///     then all the results are gathered back. When doing scatter/gather type execution, there
    ///     are several ways to do that, specifically with search engines. One of the questions when
    ///     executing a distributed search is how much results to retrieve from each shard. For
    ///     example, if we have 10 shards, the 1st shard might hold the most relevant results from 0
    ///     till 10, with other shards results ranking below it. For this reason, when executing a
    ///     request, we will need to get results from 0 till 10 from all shards, sort them, and then
    ///     return the results if we want to ensure correct results. Another question, which relates
    ///     to the search engine, is the fact that each shard stands on its own. When a query is
    ///     executed on a specific shard, it does not take into account term frequencies and other
    ///     search engine information from the other shards. If we want to support accurate ranking,
    ///     we would need to first gather the term frequencies from all shards to calculate global
    ///     term frequencies, then execute the query on each shard using these globale frequencies.
    ///     Also, because of the need to sort the results, getting back a large document set, or even
    ///     scrolling it, while maintaing the correct sorting behavior can be a very expensive
    ///     operation. For large result set scrolling without sorting, the scan search type
    ///     (explained below) is also available. Elastic is very flexible and allows to control the
    ///     type of search to execute on a per search request basis. The type can be configured by
    ///     setting the search_type parameter in the query string.
    /// </summary>
    public enum SeachType
    {
        /// <summary>
        ///     The most naive (and possibly fastest) implementation is to simply execute the query
        ///     on all relevant shards and return the results. Each shard returns size results. Since
        ///     each shard already returns size hits, this type actually returns size times number of
        ///     shards results back to the caller.
        /// </summary>
        query_and_fetch,

        /// <summary>
        ///     The query is executed against all shards, but only enough information is returned
        ///     (not the document content). The results are then sorted and ranked, and based on it,
        ///     only the relevant shards are asked for the actual document content. The return number
        ///     of hits is exactly as specified in size, since they are the only ones that are
        ///     fetched. This is very handy when the index has a lot of shards (not replicas, shard
        ///     id groups).
        /// </summary>
        query_then_fetch,

        /// <summary>
        ///     Same as "Query And Fetch", except for an initial scatter phase which goes and
        ///     computes the distributed term frequencies for more accurate scoring.
        /// </summary>
        dfs_query_and_fetch

        //scan, // has its own implemention see the scan and scroll method
    }
}