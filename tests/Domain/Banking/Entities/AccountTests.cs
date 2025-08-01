namespace Household.Domain.Banking.Tests;

using System;
using Household.Domain.Banking;
using Household.Exceptions;

public class AccountTests
{
    private BankAccount account;

    [SetUp]
    public void Setup()
    {
        account = new BankAccount(Guid.NewGuid(), "123456789", "Test Bank", "Test Account");
    }

    [Test]
    public void TestTransaction()
    {
        // Existing test remains unchanged
        var initialBalance = account.Balance;
        var transaction = account.AddTransaction(100, TransactionType.Credit, "Deposit", DateTime.Now);

        Assert.That(transaction, Is.Not.Null);
        Assert.That(account.Transactions.Count, Is.EqualTo(1));
        Assert.That(account.Balance, Is.EqualTo(initialBalance + 100));
    }

    [Test]
    [TestCaseSource(typeof(TransactionTestData), nameof(TransactionTestData.ValidTransactionCases))]
    public void TestAddingTransaction(Transaction transaction)
    {
        // Arrange
        var initialBalance = account.Balance;
        decimal expectedBalanceChange = transaction.Type == TransactionType.Credit ? transaction.Amount : -transaction.Amount;
        var expectedBalance = initialBalance + expectedBalanceChange;

        // Act

        /* Handle cases where the balance would drop below zero and an exception is supposed to be thrown */
        if (expectedBalance < 0 && (!account.CanGoIntoOverdraft || (account.Balance + account.OverdraftLimit) < transaction.Amount))
        {
            Assert.Throws<InsufficientFundsException>(() => account.AddTransaction(transaction.Amount, transaction.Type, transaction.Description, transaction.Date));
            return; // Skip further assertions if exception is expected
        }

        /* Handle Transactions not belonging to this Account */
        if (transaction.BankAccountId != account.Id)
        {
            Assert.Throws<TransactionDoesNotBelongToAccountException>(() => account.AddTransaction(transaction));
            return; // Skip further assertions if exception is expected
        }

        // Add the transaction
        var addedTransaction = account.AddTransaction(transaction.Amount, transaction.Type, transaction.Description, transaction.Date);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(addedTransaction, Is.Not.Null);
            Assert.That(account.Transactions, Does.Contain(addedTransaction));
            Assert.That(account.Balance, Is.EqualTo(expectedBalance));
            Assert.That(account.Transactions, Has.Count.EqualTo(1));
        });
    }

    [Test]
    public void TestInsufficientBalanceException()
    {
        // Arrange
        var initialBalance = account.Balance;
        var transactionAmount = initialBalance + 100; // Attempting to debit more than current balance

        // Act & Assert
        if (!account.CanGoIntoOverdraft || (account.Balance + account.OverdraftLimit) < transactionAmount)
            // If overdraft is not allowed or insufficient, expect an exception
            Assert.Throws<InsufficientFundsException>(() => account.AddTransaction(transactionAmount, TransactionType.Debit, "Withdrawal", DateTime.Now));
    }

    [Test]
    public void TestAddingNullTransaction()
    {
        // Arrange & Act & Assert
        //Assert.Throws<ArgumentNullException>(() => account.AddTransaction(null));
    }

    [Test]
    public void TestAddingTransactionWithNegativeAmount()
    {
        // Arrange
        //var transaction = new Transaction(-100, TransactionType.Credit, "Negative amount", DateTime.Now);

        // Act & Assert
        //Assert.Throws<InvalidTransactionException>(() => account.AddTransaction(transaction));
    }
}