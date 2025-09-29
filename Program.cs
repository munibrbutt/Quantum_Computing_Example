using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Quantum.Simulation.Core;
using Microsoft.Quantum.Simulation.Simulators;

class Program
{
    // Classical linear search over 0..31
    static (int value, int checks) ClassicalSearch5(int target)
    {
        int checks = 0;
        for (int x = 0; x < 32; x++)
        {
            checks++;
            if (x == target) return (x, checks);
        }
        return (-1, checks);
    }

    // Convert int 0..31 to 5 bits MSB->LSB: [b4,b3,b2,b1,b0]
    static bool[] IntToBool5(int value) => new[]
    {
        ((value >> 4) & 1) == 1,
        ((value >> 3) & 1) == 1,
        ((value >> 2) & 1) == 1,
        ((value >> 1) & 1) == 1,
        ((value >> 0) & 1) == 1
    };

    static async Task Main()
    {
        int target = 0b10010; // 18
        int iterations = 4;   // ≈ round((π/4) * √32)

        // -------- Classical --------
        var (found, classicalChecks) = ClassicalSearch5(target);
        Console.WriteLine("CLASSICAL (linear) over N=32");
        Console.WriteLine($"Target: {target} (bin {Convert.ToString(target, 2).PadLeft(5, '0')})");
        Console.WriteLine($"Found:  {found}  (bin {Convert.ToString(found, 2).PadLeft(5, '0')})");
        Console.WriteLine($"Checks: {classicalChecks}   (avg ≈ 16, worst = 32)");
        Console.WriteLine();

        // -------- Quantum (Grover) --------
        using var sim = new QuantumSimulator();

        // Wrap bool[] for Q# interop
        var targetBits = new QArray<bool>(IntToBool5(target));

        // Call Q# op; result is (IQArray<Result>, long)
        var resultTuple = await QuantumExamples.GroverSearch5.Run(sim, targetBits, iterations);
        IQArray<Result> measuredBits = resultTuple.Item1;
        long oracleCalls = resultTuple.Item2;   // <-- Q# Int -> C# long

        // Convert measured bits to int and string WITHOUT using Count anywhere
        int measuredValue = 0;
        var sb = new StringBuilder();
        foreach (Result r in measuredBits)
        {
            measuredValue = (measuredValue << 1) | (r == Result.One ? 1 : 0);
            sb.Append(r == Result.One ? '1' : '0');
        }
        string measuredBitsString = sb.ToString();

        Console.WriteLine("QUANTUM (Grover) over N=32");
        Console.WriteLine($"Target:         {target} (bin {Convert.ToString(target, 2).PadLeft(5, '0')})");
        Console.WriteLine($"Measured:       {measuredValue} (bin {measuredBitsString})");
        Console.WriteLine($"Iterations:     {iterations}");
        Console.WriteLine($"Oracle checks:  {oracleCalls}   (≈ √N; diffusion not counted as checks)");
        Console.WriteLine();
        Console.WriteLine("Note: Grover is probabilistic—run a few times to see success rate.");
    }
}
