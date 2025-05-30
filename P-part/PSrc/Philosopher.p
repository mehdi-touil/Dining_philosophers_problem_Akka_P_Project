type Philosopher = machine ;

// Remove type dependencies - use simple events where possible
event eRequestFork: machine;  // Philosopher machine
event eGrantFork;             // No payload needed
event eReleaseFork: machine;  // Philosopher machine
event eStartEating: machine;  // Philosopher machine
event eStopEating: machine;   // Philosopher machine

// Deadlock-prone philosopher (always left first)
machine Philosopher_Deadlock {
    var id: int;
    var leftFork: Fork;
    var rightFork: Fork;
    var forksHeld: int;

    start state Init {
        entry (payload: (id: int, left: Fork, right: Fork)) {
            id = payload.id;
            leftFork = payload.left;
            rightFork = payload.right;
            forksHeld = 0;
            goto Thinking;
        }
    }

    state Thinking {
        entry {
            goto Hungry;
        }
    }

    state Hungry {
        entry {
            send leftFork, eRequestFork, this;  // this is a machine reference
        }
        
        // No payload type needed
        on eGrantFork do {
            forksHeld = forksHeld + 1;
            if (forksHeld == 1) {
                send rightFork, eRequestFork, this;
            } else if (forksHeld == 2) {
                goto Eating;
            }
        }
    }

    state Eating {
        entry {
            announce eStartEating, this;
            send leftFork, eReleaseFork, this;
            send rightFork, eReleaseFork, this;
            announce eStopEating, this;
            forksHeld = 0;
            goto Thinking;
        }
    }
}

// Deadlock-free philosopher (asymmetric)
machine Philosopher_DeadlockFree {
    var id: int;
    var leftFork: Fork;
    var rightFork: Fork;
    var forksHeld: int;

    start state Init {
        entry (payload: (id: int, left: Fork, right: Fork)) {
            id = payload.id;
            leftFork = payload.left;
            rightFork = payload.right;
            forksHeld = 0;
            goto Thinking;
        }
    }

    state Thinking {
        entry {
            goto Hungry;
        }
    }

    state Hungry {
        entry {
            if (id == 4) {
                send rightFork, eRequestFork, this;
            } else {
                send leftFork, eRequestFork, this;
            }
        }
        
        on eGrantFork do {
            forksHeld = forksHeld + 1;
            if (forksHeld == 1) {
                if (id == 4) {
                    send leftFork, eRequestFork, this;
                } else {
                    send rightFork, eRequestFork, this;
                }
            } else if (forksHeld == 2) {
                goto Eating;
            }
        }
    }

    state Eating {
        entry {
            announce eStartEating, this;
            send leftFork, eReleaseFork, this;
            send rightFork, eReleaseFork, this;
            announce eStopEating, this;
            forksHeld = 0;
            goto Thinking;
        }
    }
}