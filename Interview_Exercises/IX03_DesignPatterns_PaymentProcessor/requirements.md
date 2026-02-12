# IX03 - Design Patterns: Payment Processor

## Difficulty: Medium
## Estimated Time: 60-90 minutes
## Type: Create from scratch (with unit tests)

## Overview

Build a payment processing system using the **Strategy** and **Factory** design patterns. Each payment method has its own processing logic and validation rules. This exercise tests your ability to apply classic OOP design patterns in a realistic scenario.

## Requirements

### Data Models (provided)

- `PaymentMethod` enum: `CreditCard`, `BankTransfer`, `Cryptocurrency`
- `PaymentRequest` record: `Amount` (decimal), `Currency` (string), `PaymentMethod`, `PaymentDetails` (dictionary of string key-value pairs)
- `PaymentResult` record: `Success` (bool), `TransactionId` (string?), `ErrorMessage` (string?)

### Interfaces to Define

#### `IPaymentValidator`
- `ValidationResult Validate(PaymentRequest request)`
- `ValidationResult` has `IsValid` (bool) and `Errors` (list of strings)

#### `IPaymentStrategy`
- `PaymentResult ProcessPayment(PaymentRequest request)`
- `PaymentMethod SupportedMethod { get; }`

### Strategies to Implement

#### 1. `CreditCardPaymentStrategy`
- Validates that `PaymentDetails` contains `CardNumber` (16 digits), `ExpiryDate` (MM/YY format), and `CVV` (3 digits).
- Processing: return success with a generated transaction ID (e.g., `"CC-" + Guid`).
- Rejects amounts over 10,000.

#### 2. `BankTransferPaymentStrategy`
- Validates that `PaymentDetails` contains `IBAN` (starts with 2 letters followed by digits, min 15 chars) and `BankCode` (non-empty).
- Processing: return success with a generated transaction ID (e.g., `"BT-" + Guid`).
- Rejects amounts under 1.00.

#### 3. `CryptocurrencyPaymentStrategy`
- Validates that `PaymentDetails` contains `WalletAddress` (non-empty, min 26 chars) and `Network` (one of: "Bitcoin", "Ethereum", "Solana").
- Processing: return success with a generated transaction ID (e.g., `"CRYPTO-" + Guid`).

### Factory

#### `PaymentStrategyFactory`
- Constructor accepts `IEnumerable<IPaymentStrategy>`.
- `IPaymentStrategy GetStrategy(PaymentMethod method)` - returns the matching strategy or throws `NotSupportedException`.

### Orchestrator

#### `PaymentProcessor`
- Uses the factory to resolve the correct strategy.
- Calls the strategy's validator first; if validation fails, returns a failed `PaymentResult` with error details.
- If valid, processes the payment.

## Bonus (Optional)

- Implement a `CompositeValidator` that combines multiple `IPaymentValidator` instances and aggregates all errors.
- Add an `IPaymentLogger` interface and log each payment attempt.

## Unit Tests to Write

Create tests in `IX03_DesignPatterns_PaymentProcessor.Tests` that cover:

1. Each strategy processes a valid payment and returns success.
2. Each strategy rejects invalid payment details with appropriate errors.
3. Factory returns correct strategy for each payment method.
4. Factory throws `NotSupportedException` for unknown method.
5. PaymentProcessor integrates factory + validation correctly.

## Constraints

- Do NOT use any external payment libraries.
- All validation logic must be in the strategies (or validators), not in the processor.
- Use constructor injection throughout.

## Topics Covered

- Strategy Pattern
- Factory Pattern
- Interface segregation
- Input validation
- Unit testing
- Dependency Injection (manual)
