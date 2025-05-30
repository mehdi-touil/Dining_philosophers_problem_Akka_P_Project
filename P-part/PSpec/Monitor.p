spec Monitor observes eGrantFork, eReleaseFork, eStartEating, eStopEating {
    var forksInUse: int;
    var philosophersEating: set[machine];  // Use generic machine type
    
    // Helper function to check deadlock condition
    fun CheckDeadlockCondition() {
        if (sizeof(philosophersEating) == 0 && forksInUse == 5) {
            assert false, "Deadlock detected: All forks held but no one eating";
        }
    }
    
    start state Init {
        entry {
            forksInUse = 0;
            philosophersEating = default(set[machine]);
            CheckDeadlockCondition();
        }
        
        // No payload types needed
        on eGrantFork do {
            forksInUse = forksInUse + 1;
            CheckDeadlockCondition();
        }
        
        on eReleaseFork do {
            forksInUse = forksInUse - 1;
            CheckDeadlockCondition();
        }
        
        on eStartEating do (philosopher: machine) {
            philosophersEating += (philosopher);
            CheckDeadlockCondition();
        }
        
        on eStopEating do (philosopher: machine) {
            if (philosopher in philosophersEating) {
                philosophersEating -= (philosopher);
            }
            CheckDeadlockCondition();
        }
    }
}