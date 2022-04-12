﻿namespace StackExchange.Redis
{
    internal enum RedisCommand
    {
        NONE, // must be first for "zero reasons"

        APPEND,
        ASKING,
        AUTH,

        BGREWRITEAOF,
        BGSAVE,
        BITCOUNT,
        BITOP,
        BITPOS,
        BLPOP,
        BRPOP,
        BRPOPLPUSH,

        CLIENT,
        CLUSTER,
        CONFIG,
        COPY,

        DBSIZE,
        DEBUG,
        DECR,
        DECRBY,
        DEL,
        DISCARD,
        DUMP,

        ECHO,
        EVAL,
        EVALSHA,
        EXEC,
        EXISTS,
        EXPIRE,
        EXPIREAT,

        FLUSHALL,
        FLUSHDB,

        GEOADD,
        GEODIST,
        GEOHASH,
        GEOPOS,
        GEORADIUS,
        GEORADIUSBYMEMBER,

        GET,
        GETBIT,
        GETDEL,
        GETEX,
        GETRANGE,
        GETSET,

        HDEL,
        HEXISTS,
        HGET,
        HGETALL,
        HINCRBY,
        HINCRBYFLOAT,
        HKEYS,
        HLEN,
        HMGET,
        HMSET,
        HSCAN,
        HSET,
        HSETNX,
        HSTRLEN,
        HVALS,

        INCR,
        INCRBY,
        INCRBYFLOAT,
        INFO,

        KEYS,

        LASTSAVE,
        LATENCY,
        LINDEX,
        LINSERT,
        LLEN,
        LMOVE,
        LPOP,
        LPOS,
        LPUSH,
        LPUSHX,
        LRANGE,
        LREM,
        LSET,
        LTRIM,

        MEMORY,
        MGET,
        MIGRATE,
        MONITOR,
        MOVE,
        MSET,
        MSETNX,
        MULTI,

        OBJECT,

        PERSIST,
        PEXPIRE,
        PEXPIREAT,
        PFADD,
        PFCOUNT,
        PFMERGE,
        PING,
        PSETEX,
        PSUBSCRIBE,
        PTTL,
        PUBLISH,
        PUBSUB,
        PUNSUBSCRIBE,

        QUIT,

        RANDOMKEY,
        READONLY,
        READWRITE,
        RENAME,
        RENAMENX,
        REPLICAOF,
        RESTORE,
        ROLE,
        RPOP,
        RPOPLPUSH,
        RPUSH,
        RPUSHX,

        SADD,
        SAVE,
        SCAN,
        SCARD,
        SCRIPT,
        SDIFF,
        SDIFFSTORE,
        SELECT,
        SENTINEL,
        SET,
        SETBIT,
        SETEX,
        SETNX,
        SETRANGE,
        SHUTDOWN,
        SINTER,
        SINTERCARD,
        SINTERSTORE,
        SISMEMBER,
        SLAVEOF,
        SLOWLOG,
        SMEMBERS,
        SMISMEMBER,
        SMOVE,
        SORT,
        SPOP,
        SRANDMEMBER,
        SREM,
        STRLEN,
        SUBSCRIBE,
        SUNION,
        SUNIONSTORE,
        SSCAN,
        SWAPDB,
        SYNC,

        TIME,
        TOUCH,
        TTL,
        TYPE,

        UNLINK,
        UNSUBSCRIBE,
        UNWATCH,

        WATCH,

        XACK,
        XADD,
        XCLAIM,
        XDEL,
        XGROUP,
        XINFO,
        XLEN,
        XPENDING,
        XRANGE,
        XREAD,
        XREADGROUP,
        XREVRANGE,
        XTRIM,

        ZADD,
        ZCARD,
        ZCOUNT,
        ZINCRBY,
        ZINTERSTORE,
        ZLEXCOUNT,
        ZPOPMAX,
        ZPOPMIN,
        ZRANDMEMBER,
        ZRANGE,
        ZRANGEBYLEX,
        ZRANGEBYSCORE,
        ZRANGESTORE,
        ZRANK,
        ZREM,
        ZREMRANGEBYLEX,
        ZREMRANGEBYRANK,
        ZREMRANGEBYSCORE,
        ZREVRANGE,
        ZREVRANGEBYLEX,
        ZREVRANGEBYSCORE,
        ZREVRANK,
        ZSCAN,
        ZSCORE,
        ZUNIONSTORE,

        UNKNOWN,
    }
}
