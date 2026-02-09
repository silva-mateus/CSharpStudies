# Custom exception - InvalidTransactionException
Implement a custom exception class called InvalidTransactionException according to the following requirements:

1) It should have a public get-only TransactionData property of the TransactionData type

2) It should have three basic constructors that any exception should have

3) It should have two extra constructors - one setting the message and the TransactionData and one setting the message, TransactionData and InnerException (please keep the parameters of the constructors in the given order)