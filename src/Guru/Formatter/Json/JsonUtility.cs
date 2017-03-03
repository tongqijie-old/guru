﻿using System;
using System.Reflection;
using System.Collections;

namespace Guru.Formatter.Json
{
    internal static class JsonUtility
    {
        public static JsonObjectType GetJsonObjectType(Type type)
        {
            if (type == typeof(object))
            {
                return JsonObjectType.Runtime;
            }
            else if (typeof(ICollection).GetTypeInfo().IsAssignableFrom(type))
            {
                return JsonObjectType.Collection;
            }
            else if (type.GetTypeInfo().IsClass && type != typeof(string))
            {
                return JsonObjectType.Dictionary;
            }
            else
            {
                return JsonObjectType.Value;
            }
        }
    }
}