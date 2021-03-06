using Newtonsoft.Json.Linq;
using Puppy.Elastic.ContextSearch.SearchModel.AggModel.Buckets;
using System.Collections.Generic;

namespace Puppy.Elastic.ContextSearch.SearchModel.AggModel
{
    public class GeoDistanceBucketAggregationsResult : AggregationResult<GeoDistanceBucketAggregationsResult>
    {
        public List<GeoDistanceRangeBucket> Buckets { get; set; }

        public override GeoDistanceBucketAggregationsResult GetValueFromJToken(JToken result)
        {
            return result.ToObject<GeoDistanceBucketAggregationsResult>();
        }
    }
}