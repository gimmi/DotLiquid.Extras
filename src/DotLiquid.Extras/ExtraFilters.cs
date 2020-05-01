﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DotLiquid.Extras
{
    public static class ExtraFilters
    {
        public static IEnumerable<object> Where(object any, string key, object value) => ToEnum(any).Where(x => Equals(GetFieldVal(x, key), value));

        public static IEnumerable<IDictionary<string,object>> InnerJoin(object outerAny, object innerAny, string predicate)
        {
            var split = predicate.Split(new[]{'='}, StringSplitOptions.RemoveEmptyEntries);
            var outerKey = split.First().Trim();
            var innerKey = split.Last().Trim();
            var outerEnum = ToEnum(outerAny);
            var innerEnum = ToEnum(innerAny);
            return outerEnum.Join(
                innerEnum,
                outer => GetFieldVal(outer, outerKey),
                inner => GetFieldVal(inner, innerKey),
                MergeDicts
            );
        }

        private static object GetFieldVal(object any, string key)
        {
            if (any is IDictionary<string, object> dict && dict.TryGetValue(key, out var val))
            {
                return val;
            }

            return null;
        }

        private static IEnumerable<object> ToEnum(object any)
        {
            return any switch {
                null => new object[0],
                string _ => new [] {any},
                IDictionary<string, object> dict => dict.Values,
                IEnumerable<object> en => en,
                IEnumerable en => en.Cast<object>(),
                _ => new[] {any}
            };
        }

        private static IDictionary<string, object> ToDict(object any)
        {
            return any switch {
                null => new Hash(),
                IDictionary<string, object> dict => dict,
                _ => new Hash()
            };
        }

        private static IDictionary<string, object> MergeDicts(object leftAny, object rightAny)
        {
            var leftDict = ToDict(leftAny);
            var rightDict = ToDict(rightAny);

            var outDict = new Hash();
            outDict.Merge(leftDict);
            outDict.Merge(rightDict);
            return outDict;
        }
    }
}