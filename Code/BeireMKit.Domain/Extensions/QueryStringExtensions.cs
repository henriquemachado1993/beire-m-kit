﻿using Newtonsoft.Json;
using System.Reflection;
using System.Web;

namespace BeireMKit.Domain.Extensions
{
    public static class QueryStringExtensions
    {
        public static string ToQueryString<T>(this T obj)
        {
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                      .Where(p => p.GetValue(obj) != null);

            var queryString = string.Join("&", properties.Select(p =>
            {
                var value = p.GetValue(obj);
                return $"{HttpUtility.UrlEncode(p.Name)}={HttpUtility.UrlEncode(value?.ToString())}";
            }));

            return queryString;
        }

        public static string ToQueryStringWithJsonProperty<T>(this T obj)
        {
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                      .Where(p => p.GetValue(obj) != null);

            var queryString = string.Join("&", properties.Select(p =>
            {
                var jsonPropAttribute = p.GetCustomAttribute<JsonPropertyAttribute>();
                var propertyName = jsonPropAttribute != null ? jsonPropAttribute.PropertyName : p.Name;
                var value = HttpUtility.UrlEncode(p.GetValue(obj)?.ToString());
                return $"{propertyName}={value}";
            }));

            return queryString;
        }
    }
}
