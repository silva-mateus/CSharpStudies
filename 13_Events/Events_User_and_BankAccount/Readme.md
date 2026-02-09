# Events - User and BankAccount
In the existing code, we have two classes: BankAccount and User.

When the user is created, their total Funds are calculated as the sum of cash and money in the bank account. This is implemented in the User class constructor.

When the balance in the bank account is decreased, the user needs to be notified, so the total Funds of the user can be decreased as well.

This code should work once the exercise is completed:

```csharp
var bankAccount = new BankAccount(10_000);
var user = new User(1000, bankAccount.Balance);
bankAccount.OnBalanceDecreased += user.ReduceFunds;

bankAccount.DecreaseBalance(100);
```

We create a BankAccount with a Balance equal to 10 000. We also create a user who has 1000 cash and 10 000 in this bank account, so the total Funds of the user will be 11 000.

Then, we call the DecreaseMethod on the bank account and remove 100 from its Balance.

The user object should be notified, and the total Funds of the user should be decreased by 100, so they should be 10900 once this code is executed.

Your job is to:

- Implement the DecreaseBalance method in the BankAccount class. It should reduce the Balance by the given price and notify the user that the balance has been decreased.

- Implement the ReduceFunds method in the User class, so it reduces the Funds of the user by the given amount.