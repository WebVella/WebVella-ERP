using Microsoft.Framework.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.ERP
{
    public static class Settings
    {
        public static string ConnectionString { get; private set; }
        public static string EncriptionKey { get; private set; }

        public static void Initialize(IConfiguration configuration)
        {
            ConnectionString = configuration["Settings:ConnectionString"];
            EncriptionKey = configuration["Settings:EncriptionKey"];
        }
    }
}
