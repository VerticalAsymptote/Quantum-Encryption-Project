public class Universe{
    static void Main(string[] args){
        EntangledPair[] particleArray = GetEntangledArray(15);

        SimulateEncryption(particleArray);
    }

    public static void SimulateEncryption(EntangledPair[] particles){
        Console.WriteLine("-----------------------------------Simulation-----------------------------------");

        // Step 1: Alice measures particles in random directions and stores the measurements
        string output = "";
        Spin[] AliceMeasurements = new Spin[particles.Length];
        for (int index = 0; index < particles.Length; index++){
            EntangledPair pair = particles[index];
            pair.MeasureSpin(GetRandomAxis(), "p1");
            AliceMeasurements[index] = pair.p1.spin;
            output += AliceMeasurements[index] + "  ";
        }
        Console.WriteLine("Alice Measures: " + output);
        Console.WriteLine();

        output = "";
        Spin[] BobMeasurements = new Spin[particles.Length];
        for (int index = 0; index < particles.Length; index++){
            EntangledPair pair = particles[index];
            BobMeasurements[index] = pair.p2.spin;
            output += BobMeasurements[index] + "  ";
        }
        Console.WriteLine("Bob Measures: " + output); 
        Console.WriteLine();

    }

    // Returns a array of random qubits for a predefined size
    public static EntangledPair[] GetEntangledArray(int size){
        EntangledPair[] array = new EntangledPair[size];
        for (int i = 0; i < size; i++){
            array[i] = new EntangledPair();
        }
        return array;
    }

    private static Axis GetRandomAxis(){
        Random random = new Random();
        return (Axis)(random.NextInt64() % 3);
    }
}