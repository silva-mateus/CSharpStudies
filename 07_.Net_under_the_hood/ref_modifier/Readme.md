# "ref" modifier - FastForwardToSummer
Implement the FastForwardToSummer method, which takes a DateTime object and potentially modifies it (but the method does not return anything!).

- If this date is after the first day of summer of its year, then it should not be modified.
- If it is before the first day of the summer of its year, it shall be modified so it represents the first day of the summer of its year.

Let's assume the first day of summer is the 21st of June.

For example:

- If the input date was the 14th of August 2024, then it should not be modified because it is after the first day of the summer of 2024
- If the input date was the 14th of February 2025, then it should be assigned the 21st of June because it is before the first day of summer of 2025

Tips:

- Remember that DateTime is a value type.
- You can check if a date is before or after another date using the comparison operators. For example, if date1 > date2, it means date1 is after date2