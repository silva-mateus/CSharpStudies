# HotelBooking Class

## Description
Define a public `HotelBooking` class with fields accessible from outside the class.

## Requirements

### Fields
- `GuestName` (string)
- `StartDate` (DateTime)
- `EndDate` (DateTime)

### Constructor Parameters (in order)
1. `guestName` (string)
2. `startDate` (DateTime)
3. `lengthOfStayInDays` (int)

### Implementation Details
- Calculate `EndDate` in the constructor using `StartDate.AddDays(lengthOfStayInDays)`

## Topics Covered
- Class definition
- Fields
- Constructors
- DateTime manipulation