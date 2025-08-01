namespace Household.Exceptions;

public class TransactionDoesNotBelongToAccountException() : Exception("The transaction does not belong to the specified account.");