﻿using Guru.Network.Abstractions;

namespace Guru.Network
{
    public static class HttpResponseExtensions
    {
        public static bool IsHttpOk(this IHttpResponse response)
        {
            return response != null && response.StatusCode == 200;
        }
    }
}