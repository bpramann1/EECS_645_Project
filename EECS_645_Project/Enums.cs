public enum BusTransactions
{
    ExclusiveRead,
    Read,
    Upgrade,
    Flush
}

public enum ProcessorStates
{
    Invalid,
    Exclusive,
    Modified,
    Owner,
    Shared
}