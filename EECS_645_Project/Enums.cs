public enum BusTransactions
{
    ExclusiveRead,
    Read,
    Upgrade,
    Flush
}

public enum CacheIndexStates
{
    Invalid,
    Exclusive,
    Modified,
    Owner,
    Shared
}

public enum ProcessorStates
{
    Invalid,
    Exclusive,
    Modified,
    Owner,
    Shared
}