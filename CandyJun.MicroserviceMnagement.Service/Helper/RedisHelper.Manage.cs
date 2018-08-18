using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace CandyJun.MicroserviceMnagement.Service
{
    public partial class RedisHelper
    {
        private static readonly string ConnectString;
        private static Lazy<ConnectionMultiplexer> _lazyConnection;
        private static readonly Object MultiplexerLock = new Object();
        private static readonly IDatabase Cache;

        static RedisHelper()
        {
            var conn = CreateManager.Value;
            Cache = conn.GetDatabase(); //获取实例
        }

        private static Lazy<ConnectionMultiplexer> GetManager(string connectionString = null)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = GetConnectionString();
            }

            return new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(connectionString));
        }

        private static Lazy<ConnectionMultiplexer> CreateManager
        {
            get
            {
                if (_lazyConnection == null)
                {
                    lock (MultiplexerLock)
                    {
                        if (_lazyConnection != null) return _lazyConnection;

                        _lazyConnection = GetManager();
                        return _lazyConnection;
                    }
                }

                return _lazyConnection;
            }
        }

        public static string GetConnectionString()
        {
            return ConnectString;
        }
    }
}
