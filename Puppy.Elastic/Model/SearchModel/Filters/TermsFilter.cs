﻿using Puppy.Elastic.Utils;
using System.Collections.Generic;

namespace Puppy.Elastic.Model.SearchModel.Filters
{
    public class TermsFilter : IFilter
    {
        private readonly string _term;
        private readonly List<object> _termValues;

        public TermsFilter(string term, List<object> termValues)
        {
            _term = term;
            _termValues = termValues;
        }

        public void WriteJson(ElasticJsonWriter elasticCrudJsonWriter)
        {
            elasticCrudJsonWriter.JsonWriter.WritePropertyName("terms");
            elasticCrudJsonWriter.JsonWriter.WriteStartObject();

            JsonHelper.WriteListValue(_term, _termValues, elasticCrudJsonWriter);

            elasticCrudJsonWriter.JsonWriter.WriteEndObject();
        }
    }
}