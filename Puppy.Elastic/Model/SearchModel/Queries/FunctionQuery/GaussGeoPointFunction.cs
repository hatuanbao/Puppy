using Puppy.Elastic.Model.GeoModel;
using Puppy.Elastic.Model.Units;

namespace Puppy.Elastic.Model.SearchModel.Queries.FunctionQuery
{
    public class GaussGeoPointFunction : GeoDecayBaseScoreFunction
    {
        public GaussGeoPointFunction(string field, GeoPoint origin, DistanceUnit scale) : base(field, origin, scale,
            "gauss")
        {
        }

        public override void WriteJson(ElasticJsonWriter elasticCrudJsonWriter)
        {
            WriteJsonBase(elasticCrudJsonWriter, WriteValues);
        }
    }
}