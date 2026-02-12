# JD06 - Code Review Exercise

## Difficulty: Medium
## Estimated Time: 45-60 minutes
## Type: Written review (no coding)

## Overview

A "pull request" consisting of 5 C# files is provided. These files implement a `UserService` with CRUD, authentication, and caching. The code compiles and superficially works, but contains numerous issues across security, performance, design, and correctness.

Your task is to review the code as if it were a real pull request from a colleague, and write a structured code review document.

## Instructions

1. Read all 5 files in the `PullRequest/` folder carefully.
2. Identify at least **15 issues** across the categories below.
3. Write your review in `review.md`, organized by category.
4. For each issue, cite the **file name and line number**, explain the problem, and suggest a fix.

## Review Categories

Your review should cover:

| Category | What to look for |
|----------|-----------------|
| **Bugs** | Logic errors, off-by-ones, incorrect conditions, wrong return values |
| **Security** | SQL injection, hardcoded secrets, missing auth, info leakage, password handling |
| **Performance** | N+1 queries, unnecessary allocations, missing caching, sync-over-async |
| **Design** | SOLID violations, tight coupling, god classes, naming, magic strings/numbers |
| **Error Handling** | Missing null checks, swallowed exceptions, missing validation, unhelpful messages |
| **Testing Gaps** | What tests should exist but are missing? What's hard to test and why? |

## Deliverable

### `review.md`
A structured document with at least 15 issues. Use this format for each:

```markdown
### Issue [number]: [short title]
- **Category**: [Bug / Security / Performance / Design / Error Handling / Testing Gap]
- **File**: [filename.cs]
- **Line**: [line number(s)]
- **Severity**: [Critical / High / Medium / Low]
- **Problem**: [description of the issue]
- **Suggestion**: [how to fix it]
```

## Constraints

- Do NOT modify the code files -- this is a review-only exercise.
- Focus on actionable feedback, not style preferences.
- Prioritize issues by severity (Critical > High > Medium > Low).

## Topics Covered

- Code review skills
- Security awareness (OWASP top 10 concepts)
- Performance analysis
- SOLID principles recognition
- Error handling best practices
- Test coverage planning
