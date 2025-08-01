namespace Household.Domain.Banking.Tests;

using System;
using System.Collections;
using NUnit.Framework;
using Household.Domain.Banking;
public class TransactionTestData
{
    public static IEnumerable ValidTransactionCases
    {
        get
        {
            yield return new TestCaseData(
                new Transaction(100, TransactionType.Credit, "Salary deposit", DateTime.Now)
            )
            .SetName("Credit_Transaction_Should_Increase_Balance")
            .SetDescription("A credit transaction should increase the account balance by the transaction amount.")
            ;

            yield return new TestCaseData(
                new Transaction(50, TransactionType.Debit, "ATM withdrawal", DateTime.Now)
            )
            .SetName("Debit_Transaction_Should_Decrease_Balance")
            .SetDescription("A debit transaction should decrease the account balance by the transaction amount.")
            ;

            yield return new TestCaseData(
                new Transaction(0, TransactionType.Credit, "Zero amount", DateTime.Now)
            )
            .SetName("Zero_Amount_Transaction_Should_Not_Change_Balance")
            .SetDescription("A transaction with zero amount should not change the account balance.")
            ;

            yield return new TestCaseData(
                new Transaction(1000.50m, TransactionType.Credit, "Decimal amount", DateTime.Now)
            )
            .SetName("Decimal_Amount_Should_Be_Handled_Correctly")
            .SetDescription("A transaction with a decimal amount should be handled correctly and increase the balance by that amount.")
            ;

            yield return new TestCaseData(
                new Transaction(bankAccountId: Guid.NewGuid(), 1000.50m, TransactionType.Credit, "Decimal amount", DateTime.Now)
            )
            .SetName("Wrong_BankAccountId_Should_Throw_Exception")
            .SetDescription("A transaction with a different BankAccountId should throw an exception.")
            ;
        }
    }
}