// Test driver for deadlock variant
machine Setup_Deadlock {
    var forks: seq[Fork];
    var philosophers: seq[Philosopher_Deadlock];
    var i: int;

    start state Init {
        entry {
            i = 0;
            forks = default(seq[Fork]);
            while (i < 5) {
                forks[i] = new Fork();
                i = i + 1;
            }
            
            i = 0;
            philosophers = default(seq[Philosopher_Deadlock]);
            while (i < 5) {
                philosophers[i] = new Philosopher_Deadlock((
                    id = i, 
                    left = forks[i], 
                    right = forks[(i+1) % 5]
                ));
                i = i + 1;
            }
        }
    }
}

// Test driver for deadlock-free variant
machine Setup_DeadlockFree {
    var forks: seq[Fork];
    var philosophers: seq[Philosopher_DeadlockFree];
    var i: int;

    start state Init {
        entry {
            i = 0;
            forks = default(seq[Fork]);
            while (i < 5) {
                forks[i] =  new Fork();
                i = i + 1;
            }
            
            i = 0;
            philosophers = default(seq[Philosopher_DeadlockFree]);
            while (i < 5) {
                philosophers[i] = new Philosopher_DeadlockFree((
                    id = i,
                    left = forks[i],
                    right = forks[(i+1) % 5]
                ));
                i = i + 1;
            }
        }
    }
}