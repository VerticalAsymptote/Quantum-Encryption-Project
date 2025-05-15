public class Universe{
    static void Main(string[] args){
        // Includes a check on the expected value that 
        string verification = "";
        string key = "";
        string message = verification + key;
        int amountOfParticles = 24;
        EntangledPair[] particleArray = GetEntangledArray(amountOfParticles);
        Axis[] axes = GetRandomAxes(amountOfParticles);

        SimulateNormalConversation(particleArray, axes, message);
        SimulateInterceptedConversation(particleArray, axes, message);
    }

    private static void SimulateNormalConversation(EntangledPair[] particles, Axis[] axes, string unencondedMessage){
        EntangledPair[] particlesCopy = particles;
        Console.WriteLine("-----------------------------------Expected Outcome-----------------------------------");
        // Step 1: Alice measures particles in random directions and stores the measurements
        AliceMeasurements(ref particlesCopy, axes);

        // Step 2: Alice creates a message based on her particles
        string encodedMessage = EncodeMessage(unencondedMessage, particlesCopy);

        // Step 3: Bob measures particles in those directions and stores the measurements
        BobMeasurements(ref particlesCopy, axes);

        // Step 4: Bob gets the particles and measures them in the basis Alice provides
        DecodeMessage(encodedMessage, particlesCopy);
    }

    private static void SimulateInterceptedConversation(EntangledPair[] particles, Axis[] axes, string unencondedMessage){
        EntangledPair[] particlesCopy = particles;
        Console.WriteLine("-----------------------------------Simulated Outcome-----------------------------------");
        // Step 1: Alice measures particles in random directions and stores the measurements
        AliceMeasurements(ref particlesCopy, axes);

        // Step 2: Alice creates a message based on her particles
        string encodedMessage = EncodeMessage(unencondedMessage, particlesCopy);

        // Step 3: Eve intercepts the message and has to randomly choose a direction to measure spin
        EveMeasurements(ref particlesCopy);

        // Step 4: Eve attempts to decode the message
        DecodeMessage(encodedMessage, particlesCopy);

        // Step 5: Bob gets the particles and measures them in the basis Alice provides
        BobMeasurements(ref particlesCopy, axes);

        // Step 6: Bob attempts to decode the message
        DecodeMessage(encodedMessage, particlesCopy);
    }

    private static string EncodeMessage(string message, EntangledPair[] particles){
        return "";
    }

    private static void DecodeMessage(string message, EntangledPair[] particles){

    }

    private static Spin[] AliceMeasurements(ref EntangledPair[] particles, Axis[] axesMeasured){
        string axisOutput = "";
        string output = "";
        Spin[] AliceMeasurements = new Spin[particles.Length];
        for (int index = 0; index < particles.Length; index++){
            EntangledPair pair = particles[index];
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
        Axis measuredAxis = GetRandomAxes(1)[0];
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
    private static Axis[] GetRandomAxes(int amount){
        Axis[] axes = new Axis[amount];
        for (int index = 0; index < amount; index++){
            Random random = new Random();
            axes[index] = (Axis)(random.NextInt64() % 3);
        }
        return axes;
    }
}