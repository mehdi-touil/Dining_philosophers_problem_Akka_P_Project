/* Test cases for Dining Philosophers */
test tcDeadlockVariant [main=Setup_Deadlock]:
    assert Monitor in  // Reference spec directly
    (union DeadlockPhilosopher, Fork, { Setup_Deadlock });

test tcDeadlockFreeVariant [main=Setup_DeadlockFree]:
    assert Monitor in  // Reference spec directly
    (union DeadlockFreePhilosopher, Fork, { Setup_DeadlockFree });