# QuantumExamples – Grover’s Algorithm (5-bit Demo)

This project demonstrates **Grover’s search algorithm** implemented in **Q#** with a **C# host program**.  
It compares the performance of a classical linear search with a quantum Grover search on a small database of 5-bit values (N = 32).

---

## 📖 What It Does

- **Classical search**: checks values one by one until the target is found.
- **Quantum search (Grover)**: uses superposition and amplitude amplification to find the target in far fewer steps (≈ √N oracle calls instead of N/2 checks on average).

Example output:

```
CLASSICAL (linear) over N=32
Target: 18 (bin 10010)
Found:  18  (bin 10010)
Checks: 19   (avg ≈ 16, worst = 32)

QUANTUM (Grover) over N=32
Target:         18 (bin 10010)
Measured:       18 (bin 10010)
Iterations:     4
Oracle checks:  4   (≈ √N)
```

---

## 🛠️ Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download)  
- [Microsoft Quantum Development Kit templates](https://learn.microsoft.com/azure/quantum/install-command-line-qdk) (`Microsoft.Quantum.ProjectTemplates`)  

Install the templates if not already installed:

```powershell
dotnet new install Microsoft.Quantum.ProjectTemplates
```

---

## 📂 Project Structure

```
QuantumExamples/
│── Program.cs        # C# host program
│── Program.qs        # Q# quantum operations
│── QuantumExamples.csproj
│── README.md
```

- `Program.cs`: runs both the classical search and Grover’s quantum search.  
- `Program.qs`: defines the oracle, diffusion operator, and Grover’s algorithm.  
- `QuantumExamples.csproj`: project file using `Microsoft.Quantum.Sdk`.  

---

## 🚀 Run the Project

### Option 1 – .NET CLI
```powershell
dotnet restore
dotnet run -c Release
```

### Option 2 – Visual Studio 2022
1. Open `QuantumExamples.csproj` in Visual Studio.  
2. Ensure files `Program.cs` and `Program.qs` are included.  
3. Right-click the project → **Set as Startup Project**.  
4. Press **F5** to run.  

> ⚠️ You may see a **QS6202 warning** ("no entry point in Q# project") – this is safe to ignore because the entry point is the C# `Main` method.

---

## ⚡ Key Concepts

- **Superposition**: all states explored simultaneously.  
- **Oracle**: flips the phase of the target state.  
- **Diffusion operator**: amplifies the probability of the target.  
- **Iteration count**: optimal ≈ (π/4)·√N.  

---

## 🔮 Next Steps

- Increase qubits (e.g., 6 bits → N=64) and see scaling.  
- Experiment with fewer/more iterations and observe probability of success.  
- Try running on real hardware via [Azure Quantum](https://azure.microsoft.com/services/quantum/).  

---

## 📜 License

MIT License – feel free to use and modify.
