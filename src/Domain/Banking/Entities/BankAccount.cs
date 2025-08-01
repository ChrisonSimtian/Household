namespace Household.Domain.Banking;

using System;
using System.Collections.Generic;
using Household.Exceptions;

/// <summary>
/// Represents a bank account in the banking domain.
/// </summary>
/// <param name="id"></param>
/// <param name="accountNumber"></param>
/// <param name="bankName"></param>
/// <param name="accountName"></param>
public class BankAccount(Guid id, string accountNumber, string bankName, string accountName)
{
    private readonly List<Transaction> _transactions = [];
    
    public Guid Id { get; private set; } = id;
    public string AccountNumber { get; private set; } = accountNumber;
    public string BankName { get; private set; } = bankName;
    public string AccountName { get; private set; } = accountName;

    public bool CanGoIntoOverdraft { get; private set; } = false;
    public decimal OverdraftLimit { get; private set; } = 0;

    // Instead of storing a Balance field, we calculate it from Transactions.
    public decimal Balance => CalculateBalance(_transactions);

    // Collection of transactions related to this account.
    
    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();


    public Transaction AddTransaction(Transaction transaction)
    {
        if (transaction == null)
            throw new ArgumentNullException(nameof(transaction), "Transaction cannot be null.");
        if (transaction.BankAccountId != Id)
            throw new TransactionDoesNotBelongToAccountException();
        if (Transactions.Any(p => p.Id == transaction.Id))
            throw new TransactionAlreadyExistsInAccountException();

        return AddTransaction(transaction.Amount, transaction.Type, transaction.Description, transaction.Date);
    }

    public Transaction AddTransaction(decimal amount, TransactionType type, string description) => AddTransaction(amount, type, description, DateTime.Now);

    /// <summary>
    /// Adds a transaction to the account.
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="type"></param>
    /// <param name="description"></param>
    /// <param name="date"></param>
    /// <returns></returns>
    /// <exception cref="InsufficientFundsException"></exception>
    public Transaction AddTransaction(decimal amount, TransactionType type, string description, DateTime date)
    {
        // Sample business rule: Check for sufficient funds on a debit.
        if (type == TransactionType.Debit && Balance < amount)
            /* Overdraft logic */
            if (!CanGoIntoOverdraft || (Balance + OverdraftLimit) < amount)
                throw new InsufficientFundsException();

        var transaction = new Transaction(Id, amount, type, description, date);
        _transactions.Add(transaction);

        return transaction;
    }

    // A helper method that computes the current balance based on the transactions history.
    private static decimal CalculateBalance(IEnumerable<Transaction> transactions)
    {
        decimal balance = 0;
        foreach (var transaction in transactions)
        {
            balance += transaction.Type == TransactionType.Credit ? transaction.Amount : -transaction.Amount;
        }
        return balance;
    }
}