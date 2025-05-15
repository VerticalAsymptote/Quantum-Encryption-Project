public class Universe{
    static void Main(string[] args){
        EntangledPair[] particleArray = GetEntangledArray(15);

        SimulateNormalConversation(particleArray);
        SimulateInterceptedConversation(particleArray);
    }

    public static void SimulateNormalConversation(EntangledPair[] particles){
        EntangledPair[] particlesCopy = particles;
        Console.WriteLine("-----------------------------------Expected Outcome-----------------------------------");
        Axis[] axesMeasured;
        // Step 1: Alice measures particles in random directions and stores the measurements
        AliceMeasurements(ref particlesCopy, out axesMeasured);

        // Step 2: Bob measures particles in those directions and stores the measurements
        BobMeasurements(ref particlesCopy, axesMeasured);
    }

    public static void SimulateInterceptedConversation(EntangledPair[] particles){
        EntangledPair[] particlesCopy = particles;
        Console.WriteLine("-----------------------------------Simulated Outcome-----------------------------------");
        // Step 1: Alice measures particles in random directions and stores the measurements
        Axis[] axesMeasured;
        AliceMeasurements(ref particlesCopy, out axesMeasured);

        // Step 2: Eve intercepts the message and has to randomly choose a direction to measure spin
        EveMeasurements(ref particlesCopy);

        // Step 3: Bob gets the particles and measures them in the basis Alice provides
        BobMeasurements(ref particlesCopy, axesMeasured);
    }

    private static Spin[] AliceMeasurements(ref EntangledPair[] particles, out Axis[] axesMeasured){
        string axisOutput = "";
        string output = "";
        Spin[] AliceMeasurements = new Spin[particles.Length];
        axesMeasured = new Axis[particles.Length];
        for (int index = 0; index < particles.Length; index++){
            EntangledPair pair = particles[index];
            axesMeasured[index] = GetRandomAxis();
            pair.MeasureSpin(axesMeasured[index], "p1");
            AliceMeasurements[index] = pair.p1.spin;
            axisOutput += GetEnumString(axesMeasured[index]) + "  ";
            output += (int)AliceMeasurements[index] + "  ";
        }
        Console.WriteLine("Measured Basis: " + axisOutput);
        Console.WriteLine("Alice Measures: " + output);
        Console.WriteLine();
        Console.WriteLine();
        return AliceMeasurements; 
    }

    private static Spin[] EveMeasurements(ref EntangledPair[] particles){
        string axisOutput = "";
        string output = "";
        Axis measuredAxis = GetRandomAxis();
        Spin[] EveMeasurements = new Spin[particles.Length];
        for (int index = 0; index < particles.Length; index++){
            EntangledPair pair = particles[index];
            EveMeasurements[index] = pair.p2.MeasureSpin(measuredAxis);
            axisOutput += GetEnumString(measuredAxis) + "  ";
            output += (int)EveMeasurements[index] + "  ";
        }
        Console.WriteLine("Measured Basis: " + axisOutput);
        Console.WriteLine("Eve Measures:   " + output);
        Console.WriteLine();
        Console.WriteLine();
        return EveMeasurements;
    }

    private static Spin[] BobMeasurements(ref EntangledPair[] particles, Axis[] axesMeasured){
        string axisOutput = "";
        string output = "";
        Spin[] BobMeasurements = new Spin[particles.Length];
        for (int index = 0; index < particles.Length; index++){
            EntangledPair pair = particles[index];
            BobMeasurements[index] = pair.p2.MeasureSpin(axesMeasured[index]);
            axisOutput += GetEnumString(axesMeasured[index]) + "  ";
            output += (int)BobMeasurements[index] + "  ";
        }
        Console.WriteLine("Measured Basis: " + axisOutput);
        Console.WriteLine("Bob Measures:   " + output);
        Console.WriteLine();
        Console.WriteLine();
        return BobMeasurements;
    }

    private static string GetEnumString(Axis axis){
        switch(axis){
            case Axis.Z: return "Z";
            case Axis.Y: return "Y";
            case Axis.X: return "X";
            default: throw new Exception("Error!");
        }
    }

    // Returns a array of random entangled pairs for a predefined size
    private static EntangledPair[] GetEntangledArray(int size){
        EntangledPair[] array = new EntangledPair[size];
        for (int i = 0; i < size; i++){
            array[i] = new EntangledPair();
        }
        return array;
    }

    // Returns a random axis
    private static Axis GetRandomAxis(){
        Random random = new Random();
        return (Axis)(random.NextInt64() % 3);
    }
}