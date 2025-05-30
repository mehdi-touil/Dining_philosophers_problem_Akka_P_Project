machine Fork {
    var owner: machine;  // Generic machine reference
    
    start state Available {
        on eRequestFork do (philosopher: machine) {
            owner = philosopher;
            send owner, eGrantFork;
            goto Taken;
        }
    }
    
    state Taken {
        defer eRequestFork;
        
        on eReleaseFork do (philosopher: machine) {
            if (philosopher == owner) {
                goto Available;
            }
        }
    }
}