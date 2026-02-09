# Tasks & waiting
Implement the RunSimpleTask method in such a way that inside, it runs a task and waits for its result.

This task should execute a loop 3 times, and for each iteration, it should:

- stop for 1 second

- print the "Iteration number X" to the console, where X will be 1, 2, and then 3

Then we want to wait for the task completion, so the "The task has finished." gets printed to the console last.