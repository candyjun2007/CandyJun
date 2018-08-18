using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CandyJun.MicroserviceMnagement.Service
{
    public partial class RedisHelper
    {
        public const string DefaultOrder = "desc";

        #region Keys
        public static bool KeyExists(string key)
        {
            var bResult = Cache.KeyExists(key);
            return bResult;
        }

        public static bool SetExpire(string key, DateTime datetime)
        {
            return Cache.KeyExpire(key, datetime);
        }

        public static bool SetExpire(string key, int timeout = 0)
        {
            return Cache.KeyExpire(key, DateTime.Now.AddSeconds(timeout));
        }

        public static bool Set<T>(string key, T t, int timeout = 0)
        {
            var value = JsonConvert.SerializeObject(t);
            bool bResult = Cache.StringSet(key, value);
            if (timeout > 0)
            {
                Cache.KeyExpire(key, DateTime.Now.AddSeconds(timeout));
            }
            return bResult;
        }

        public static bool KeyDelete(string key)
        {
            return Cache.KeyDelete(key);
        }

        public static bool KeyRename(string oldKey, string newKey)
        {
            return Cache.KeyRename(oldKey, newKey);
        }
        #endregion

        #region Hashes
        public static bool IsExist(string hashId, string key)
        {
            return Cache.HashExists(hashId, key);
        }

        public static bool SetHash<T>(string hashId, string key, T t)
        {
            var value = JsonConvert.SerializeObject(t);
            return Cache.HashSet(hashId, key, value);
        }

        public static bool Remove(string hashId, string key)
        {
            return Cache.HashDelete(hashId, key);
        }

        public static long StringIncrement(string hashId, string key, long value = 1)
        {
            return Cache.HashIncrement(hashId, key, value);
        }

        public static T Get<T>(string hashId, string key)
        {
            string value = Cache.HashGet(hashId, key);
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static long GetHashCount(string hashId)
        {
            var length = Cache.HashLength(hashId);
            return length;
        }

        public static string Get(string hashId, string key)
        {
            string value = Cache.HashGet(hashId, key).ToString();
            return value;
        }

        public static List<T> GetAll<T>(string hashId)
        {
            var result = new List<T>();
            var list = Cache.HashGetAll(hashId).ToList();
            if (list.Count > 0)
            {
                list.ForEach(x =>
                {
                    var value = JsonConvert.DeserializeObject<T>(x.Value);
                    result.Add(value);
                });
            }
            return result;
        }

        public static List<string> GetAllFields(string hashId)
        {
            var result = new List<string>();
            var list = Cache.HashKeys(hashId).ToList();
            if (list.Count > 0)
            {
                list.ForEach(x =>
                {
                    var value = JsonConvert.DeserializeObject<string>(x);
                    result.Add(value);
                });
            }
            return result;
        }
        #endregion

        #region Sorted Sets
        public static bool SortedSetItemIsExist(string setId, string item)
        {
            var value = GetItemScoreFromSortedSet(setId, item);
            if (value != null)
            {
                return true;
            }
            return false;
        }

        public static bool SortedSetAdd(string setId, string item, double score, int timeout = 0)
        {
            return Cache.SortedSetAdd(setId, item, score);
        }

        public static List<string> GetSortedSetRangeByRank(string setId, long fromRank, long toRank, string order = DefaultOrder)
        {
            var result = new List<string>();
            var list = Cache.SortedSetRangeByRank(setId, fromRank, toRank, order == Order.Descending.ToString().ToLower() ? Order.Descending : Order.Ascending).ToList();
            if (list.Any())
            {
                list.ForEach(x =>
                {
                    var value = JsonConvert.DeserializeObject<string>(x);
                    result.Add(value);
                });
            }
            return result;
        }

        public static Dictionary<string, double> GetSortedSetRangeByRankWithScores(string setId, long fromRank, long toRank, string order = DefaultOrder)
        {
            var result = new Dictionary<string, double>();
            var list = Cache.SortedSetRangeByRankWithScores(setId, fromRank, toRank, order == Order.Descending.ToString().ToLower() ? Order.Descending : Order.Ascending).ToList();
            if (list.Any())
            {
                list.ForEach(x =>
                {
                    result.Add(x.Element, x.Score);
                });
            }
            return result;
        }

        public static List<string> GetSortedSetRangeByValue(string setId, long minValue, long maxValue)
        {
            var result = new List<string>();
            var list = Cache.SortedSetRangeByValue(setId, minValue, maxValue).ToList();
            if (list.Any())
            {
                list.ForEach(x =>
                {
                    var value = JsonConvert.DeserializeObject<string>(x);
                    result.Add(value);
                });
            }
            return result;
        }

        public static long GetSortedSetLength(string setId)
        {
            return Cache.SortedSetLength(setId);
        }

        public static long GetSortedSetLength(string setId, double minValue, double maxValue)
        {
            return Cache.SortedSetLength(setId, minValue, maxValue);
        }

        public static long? GetItemRankFromSortedSet(string setId, string item, string order = DefaultOrder)
        {
            return Cache.SortedSetRank(setId, item, order == Order.Descending.ToString().ToLower() ? Order.Descending : Order.Ascending);
        }

        public static double? GetItemScoreFromSortedSet(string setId, string item)
        {
            return Cache.SortedSetScore(setId, item);
        }

        public static double SetSortedSetItemIncrement(string setId, string item, double score = 1)
        {
            return Cache.SortedSetIncrement(setId, item, score);
        }

        public static double SortedSetItemDecrement(string setId, string item, double score = -1)
        {
            return Cache.SortedSetDecrement(setId, item, score);
        }

        public static bool RemoveItemFromSortedSet(string setId, string item)
        {
            return Cache.SortedSetRemove(setId, item);
        }

        public static long RemoveByRankFromSortedSet(string setId, long fromRank, long toRank)
        {
            return Cache.SortedSetRemoveRangeByRank(setId, fromRank, toRank);
        }

        public static long RemoveByScoreFromSortedSet(string setId, double minValue, double maxValue)
        {
            return Cache.SortedSetRemoveRangeByScore(setId, minValue, maxValue);
        }

        public static long RemoveByLexFromSortedSet(string setId, int minValue, int maxValue)
        {
            //TODO： Don't know its meaning
            //return Cache.SortedSetRemoveRangeByValue(setId, minValue, maxValue);
            return 0;
        }
        #endregion

        #region Lists

        public static long AddList<T>(string listId, T t)
        {
            var value = JsonConvert.SerializeObject(t);
            return Cache.ListRightPush(listId, value);
        }

        public static List<T> GetList<T>(string listId, long start = 0, long stop = -1)
        {
            var result = new List<T>();
            var list = Cache.ListRange(listId, start, stop).ToList();
            if (list.Count > 0)
            {
                list.ForEach(x =>
                {
                    var value = JsonConvert.DeserializeObject<T>(x);
                    result.Add(value);
                });
            }
            return result;
        }
        #endregion

        #region Strings

        public static string Get(string key)
        {
            string value = Cache.StringGet(key);
            return value;
        }

        public static T StringGet<T>(string key)
        {
            string value = Cache.StringGet(key);
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static double StringIncrement(string key, double value)
        {
            return Cache.StringIncrement(key, value);
        }

        public static long StringAppend(string key, string value)
        {
            return Cache.StringAppend(value, value, CommandFlags.None);
        }
        #endregion
    }
}
