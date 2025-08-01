namespace Household.Domain.Banking;

public enum TransactionType
{
    /// <summary>
    /// Money coming in (deposit)
    /// </summary>
    Credit,

    /// <summary>
    /// Money going out (withdrawal)
    /// </summary>
    Debit
}