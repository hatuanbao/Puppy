﻿using System;

namespace Puppy.Elastic.ContextAddDeleteUpdate.CoreTypeAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ElasticShort : ElasticNumber
    {
        public override string JsonString()
        {
            return JsonStringInternal("short");
        }
    }
}