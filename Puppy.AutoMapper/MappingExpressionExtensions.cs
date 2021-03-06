﻿#region	License

//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → TopCore </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TopCore </Project>
//     <File>
//         <Name> MappingExpressionExtensions.cs </Name>
//         <Created> 24 Apr 17 1:25:34 AM </Created>
//         <Key> ef316320-0a7a-4999-b90f-543679175c88 </Key>
//     </File>
//     <Summary>
//         MappingExpressionExtensions.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------

#endregion License

using AutoMapper;
using System.Linq;
using System.Reflection;

namespace Puppy.AutoMapper
{
    public static class MappingExpressionExtensions
    {
        public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            var sourceType = typeof(TSource);
            var destinationProperties = typeof(TDestination).GetProperties(flags);

            foreach (var property in destinationProperties)
            {
                var propInfoSrc = sourceType.GetProperties().FirstOrDefault(p => p.Name == property.Name);
                if (propInfoSrc == null)
                    expression.ForMember(property.Name, opt => opt.Ignore());
            }
            return expression;
        }
    }
}