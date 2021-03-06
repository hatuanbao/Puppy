using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Puppy.Elastic.ContextSearch.SearchModel.AggModel.Buckets;
using System.Collections.Generic;

namespace Puppy.Elastic.ContextSearch.SearchModel.AggModel
{
    public class FiltersBucketAggregationsResult : AggregationResult<FiltersBucketAggregationsResult>
    {
        public List<BaseBucket> Buckets { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JToken> SubAggregations { get; set; }

        public T GetSubAggregationsFromJTokenName<T>(string name)
        {
            return SubAggregations[name].ToObject<T>();
        }

        public T GetSingleMetricSubAggregationValue<T>(string name)
        {
            return SubAggregations[name]["value"].Value<T>();
        }

        public override FiltersBucketAggregationsResult GetValueFromJToken(JToken result)
        {
            return result.ToObject<FiltersBucketAggregationsResult>();
        }
    }
}