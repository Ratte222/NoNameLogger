using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace NoNameLogger.Extensions
{
    public static class TypeUtilities
    {
        public static List<T> GetAllPublicConstantValues<T>(this Type type)
        {
            return type
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(T))
                .Select(x => (T)x.GetRawConstantValue())
                .ToList();
        }

        public static List<T> GetAllPublicGetProperty<T>(this Type type)
        {
            return type
                .GetFields(BindingFlags.Public | BindingFlags.GetProperty)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(T))
                .Select(x => (T)x.GetRawConstantValue())
                .ToList();
        }

        public static List<string> GetAllPublicInstance(this Type type)
        {
            return type
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(fi => fi.IsPublic && !fi.IsConstructor && !fi.IsAbstract && !fi.IsAbstract)
                .Select(x => x.Name)
                .ToList();
        }
    }
}
