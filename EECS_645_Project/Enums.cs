/* BusTransaction contains the possible
 * cache bus transactions */
public enum BusTransactions
{
    ExclusiveRead,
    Read,
    Upgrade,
    Flush
}

/* CacheIndexStates contains the possible MOESI
 * states, applied to a cache index */
public enum CacheIndexStates
{
    Invalid,
    Exclusive,
    Modified,
    Owner,
    Shared
}

/* ProcessorStates contains the possible MOESI
 * states */
public enum ProcessorStates
{
    Invalid,
    Exclusive,
    Modified,
    Owner,
    Shared
}
