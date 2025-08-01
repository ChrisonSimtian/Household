namespace Household.Domain.Banking;

using System;

/// <summary>
/// Represents a financial transaction in a bank account.
/// </summary>
public class Transaction
{
    public Guid Id { get; private set; }
    public Guid BankAccountId { get; private set; }
    public decimal Amount { get; private set; }
    public TransactionType Type { get; private set; }
    public string Description { get; private set; }
    public DateTime Date { get; private set; }

    public Transaction(Guid id, Guid bankAccountId, decimal amount, TransactionType type, string description, DateTime date)
    {
        Id = id;
        BankAccountId = bankAccountId;
        Amount = amount;
        Type = type;
        Description = description;
        Date = date;
    }

    public Transaction(Guid bankAccountId, decimal amount, TransactionType type, string description, DateTime date)
        : this(Guid.NewGuid(), bankAccountId, amount, type, description, date)
    {
    }

    public Transaction(decimal amount, TransactionType type, string description, DateTime date)
        : this(Guid.NewGuid(), Guid.Empty, amount, type, description, date)
    {
    }

    public override string ToString()
    {
        return $"{Date.ToShortDateString()} - {Type}: {Amount:C} ({Description})";
    }
}