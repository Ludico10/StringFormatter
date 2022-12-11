﻿using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace String_Formatter
{
    internal class ExpressionCashe
    {
        private ConcurrentDictionary<string, Func<object, string>> cashe;

        public ExpressionCashe()
        {
            cashe = new ConcurrentDictionary<string, Func<object, string>>();
        }

        public string? FindElement(string key, object target)
        {
            Func<object, string>? toStr;
            if (cashe.TryGetValue(key, out toStr)) return toStr(target);
            else return null;
        }

        public string? AddElement(string key, object target)
        {
            if (target.GetType().GetProperty(key) != null || target.GetType().GetField(key) != null)
            {
                var objParam = Expression.Parameter(typeof(object), "obj");
                var pof = Expression.PropertyOrField(Expression.TypeAs(objParam, target.GetType()), key);
                var pofToStr = Expression.Call(pof, "ToString", null, null);
                var toStr = Expression.Lambda<Func<object, string>>(pofToStr, objParam).Compile();
                cashe.TryAdd(key, toStr);
                return toStr(target);
            }
            else return null;
        }
    }
}
