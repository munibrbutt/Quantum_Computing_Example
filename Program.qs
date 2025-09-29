namespace QuantumExamples {
    open Microsoft.Quantum.Intrinsic;
    open Microsoft.Quantum.Canon;

    // Phase-flip oracle that marks the chosen 5-bit basis state |target⟩.
    // target is MSB->LSB: [b4,b3,b2,b1,b0]
    operation ApplyOracle5(target : Bool[], qs : Qubit[]) : Unit {
        if (Length(target) != 5 or Length(qs) != 5) {
            fail "ApplyOracle5 expects 5 target bits and 5 qubits.";
        }

        // Map |target⟩ -> |11111⟩ using X where target bit is 0
        for i in 0..4 {
            if (not target[i]) { X(qs[i]); }
        }

        // Multi-controlled Z: first 4 as controls, last as target
        Controlled Z(qs[0..3], qs[4]);

        // Uncompute mapping
        for i in 0..4 {
            if (not target[i]) { X(qs[i]); }
        }
    }

    // Grover diffusion operator on 5 qubits.
    operation Diffusion5(qs : Qubit[]) : Unit {
        for i in 0..4 { H(qs[i]); }
        for i in 0..4 { X(qs[i]); }
        Controlled Z(qs[0..3], qs[4]);
        for i in 0..4 { X(qs[i]); }
        for i in 0..4 { H(qs[i]); }
    }

    // Grover search over 5 qubits (N = 32). Returns (measuredBits, oracleCallCount).
    operation GroverSearch5(target : Bool[], iterations : Int) : (Result[], Int) {
        use qs = Qubit[5];

        // 1) Equal superposition over 32 states
        for i in 0..4 { H(qs[i]); }

        mutable calls = 0;

        // 2) Grover iterations: Oracle -> Diffusion
        for _ in 1..iterations {
            ApplyOracle5(target, qs);
            set calls += 1;
            Diffusion5(qs);
        }

        // 3) Measure and reset
        mutable results = [Zero, size = 5];
        for i in 0..4 {
            let r = M(qs[i]);
            set results w/= i <- r;
            if (r == One) { X(qs[i]); } // reset to |0⟩
        }

        return (results, calls);
    }
}
