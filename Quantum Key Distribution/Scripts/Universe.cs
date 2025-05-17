using System.Text.RegularExpressions;

public partial class Universe
{
        public static int verification = 10;
        public static string unencodedKey = "10011001110101";
        public static int amountOfParticles = 30;
    static void Main(string[] args)
    {
        // Setup for the given experiment
        EntangledPair[] particleArray = GetEntangledArray(amountOfParticles);
        Axis[] axes = GetRandomAxes(amountOfParticles);

        Console.WriteLine("-----------------------------------Initial State-----------------------------------");
        Console.WriteLine("Verification Length: " + verification);
        Console.WriteLine("Unencoded Key: " + unencodedKey);
        Console.WriteLine();
        // Step 1: Alice measures particles in random directions and stores the measurements
        AliceMeasurements(ref particleArray, axes);
        // Step 2: Alice encodes the message given the results of her measurements
        string encodedMessage = EncodeMessage(verification, unencodedKey, particleArray);

        SimulateNormalConversation(particleArray, encodedMessage);
        //SimulateInterceptedConversation(particleArray, encodedMessage);
    }

    private static void SimulateNormalConversation(EntangledPair[] particles, string encodedMessage)
    {
        EntangledPair[] particlesCopy = particles;
        Console.WriteLine("-----------------------------------Expected Outcome-----------------------------------");

        string[] seperatedMessage = DecodeMessage(encodedMessage, out int parseLength);

        // Step 3: Bob gets the particles and the message and attempts decryption 
        BobMeasurements(ref particlesCopy, seperatedMessage, parseLength);
    }

    private static void SimulateInterceptedConversation(EntangledPair[] particles, string encodedMessage)
    {
        EntangledPair[] particlesCopy = particles;
        Console.WriteLine("-----------------------------------Simulated Outcome-----------------------------------");

        // Step 3: Eve intercepts the message and has to randomly choose a direction to measure spin
        EveMeasurements(ref particlesCopy);

        // Step 4: Eve attempts to decode the message
        //DecodeMessage(encodedMessage);
        string[] seperatedMessage = DecodeMessage(encodedMessage, out int parseLength);

        // Step 5: Bob gets the particles and measures them in the basis Alice provides
        BobMeasurements(ref particlesCopy, seperatedMessage, parseLength);

        // Step 6: Bob attempts to decode the message
    }

    private static string EncodeMessage(int verification, string key, EntangledPair[] particles)
    {
        Random random = new Random();
        string trustVerification = "" + verification + "|";
        for (int index = 0; index < verification; index++)
        {
            trustVerification += GetEnumString(particles[index].p1.axis) + (int)particles[index].p1.spin;
        }
        List<EntangledPair> particleList = particles.ToList(), spinUp = new List<EntangledPair>(), spinDown = new List<EntangledPair>();
        foreach (EntangledPair pair in particles)
        {
            switch (pair.p1.spin)
            {
                case Spin.Up:
                    spinUp.Add(pair);
                    break;
                case Spin.Down:
                    spinDown.Add(pair);
                    break;
                default: throw new Exception("Error!");
            }
        }
        string encodedKey = "";
        for (int index = 0; index < key.Length; index++)
        {
            int particleIndex;
            switch (key[index])
            {
                case '0':
                    particleIndex = particleList.IndexOf(spinDown[random.Next() % spinDown.Count]);
                    encodedKey += GetEnumString(particles[particleIndex].p1.axis) + particleIndex;
                    break;
                case '1':
                    particleIndex = particleList.IndexOf(spinUp[random.Next() % spinUp.Count]);
                    encodedKey += GetEnumString(particles[particleIndex].p1.axis) + particleIndex;
                    break;
                default: throw new Exception("Error!");
            }
        }
        Console.WriteLine("Verification: " + trustVerification);
        Console.WriteLine("Key: " + encodedKey);
        Console.WriteLine();
        return trustVerification + encodedKey;
    }

    private static string[] DecodeMessage(string message, out int verificationLength)
    {
        if (!int.TryParse(message.Split("|")[0], out verificationLength)) throw new Exception("Error!");

        MatchCollection matchCollection;
        Regex verifyParser = ParserRegex();
        matchCollection = verifyParser.Matches(message);
        int parseLength = matchCollection.Count;
        string[] array = new string[parseLength];
        for (int index = 0; index < parseLength; index++){
            array[index] = matchCollection[index].ToString();
        }
        return array;
    }

    private static Spin[] AliceMeasurements(ref EntangledPair[] particles, Axis[] axesMeasured)
    {
        string axisOutput = "";
        string output = "";
        Spin[] AliceMeasurements = new Spin[particles.Length];
        for (int index = 0; index < particles.Length; index++)
        {
            EntangledPair pair = particles[index];
            pair.MeasureSpin(axesMeasured[index], "p1");
            AliceMeasurements[index] = pair.p1.spin;
            axisOutput += GetEnumString(axesMeasured[index]) + "  ";
            output += (int)AliceMeasurements[index] + "  ";
        }
        Console.WriteLine("Measured Basis: " + axisOutput);
        Console.WriteLine("Alice Measures: " + output);
        Console.WriteLine();
        return AliceMeasurements;
    }

    private static Spin[] EveMeasurements(ref EntangledPair[] particles)
    {
        string axisOutput = "";
        string output = "";
        Axis measuredAxis = GetRandomAxes(1)[0];
        Spin[] EveMeasurements = new Spin[particles.Length];
        for (int index = 0; index < particles.Length; index++)
        {
            EntangledPair pair = particles[index];
            EveMeasurements[index] = pair.p2.MeasureSpin(measuredAxis);
            axisOutput += GetEnumString(measuredAxis) + "  ";
            output += (int)EveMeasurements[index] + "  ";
        }
        Console.WriteLine("Measured Basis: " + axisOutput);
        Console.WriteLine("Eve Measures:   " + output);
        Console.WriteLine();
        return EveMeasurements;
    }

    private static void BobMeasurements(ref EntangledPair[] particles, string[] actions, int verificationLength)
    {
        Axis[] axisOutput = new Axis[particles.Length];
        Spin[] spinOutput = new Spin[particles.Length];

        for (int index = 0; index < particles.Length; index++){
            axisOutput[index] = Axis.Undefined;
            spinOutput[index] = Spin.Undefined;
        }

        bool keyCorrect = true;
        for (int index = 0; index < verificationLength; index++){
            Qubit pair = particles[index].p2;
            if (!int.TryParse(actions[index][1].ToString(), out int spin)) throw new Exception("Error!");
            Spin value = pair.MeasureSpin(GetEnumFromString(actions[index][0]));
            axisOutput[index] = pair.axis;
            spinOutput[index] = value;
            keyCorrect = value == Qubit.GetOppositeSpin((Spin)spin);
        }

        string key = "";
        for (int index = verificationLength; index < actions.Length; index++){
            Axis measuredAxis = GetEnumFromString(actions[index][0]);
            if (!int.TryParse(actions[index].Substring(1), out int particleIndex)) throw new Exception("Error!");
            Spin value = particles[particleIndex].p2.MeasureSpin(measuredAxis);
            key += (int)Qubit.GetOppositeSpin(value);
            axisOutput[particleIndex] = measuredAxis;
            spinOutput[particleIndex] = value;
        }

        string stringAxisOutput = "", stringSpinOutput = "";
        for (int index = 0; index < particles.Length; index++){
            if (axisOutput[index] != Axis.Undefined){
                stringAxisOutput += GetEnumString(axisOutput[index]) + "  ";
            } else {
                stringAxisOutput += "-  ";
            }
            if (spinOutput[index] != Spin.Undefined){
                stringSpinOutput += (int)spinOutput[index] + "  ";
            } else {
                stringSpinOutput += "?  ";
            }
        }
        Console.WriteLine("Measured Basis: " + stringAxisOutput);
        Console.WriteLine("Bob Measures:   " + stringSpinOutput); 
        Console.WriteLine();
        Console.WriteLine("Is Verified: " + keyCorrect);
        Console.WriteLine("Is Successfully Decoded: " + (key == unencodedKey));
        Console.WriteLine("Decoded Key: " + key);
    }

    private static string GetEnumString(Axis axis)
    {
        switch (axis)
        {
            case Axis.Z: return "Z";
            case Axis.Y: return "Y";
            case Axis.X: return "X";
            default: throw new Exception("Error!");
        }
    }

    private static Axis GetEnumFromString(char s){
        switch (s){
            case 'X': return Axis.X;
            case 'Y': return Axis.Y;
            case 'Z': return Axis.Z;
            default: throw new Exception("Error!");
        }
    }

    // Returns a array of random entangled pairs for a predefined size
    private static EntangledPair[] GetEntangledArray(int size)
    {
        EntangledPair[] array = new EntangledPair[size];
        for (int i = 0; i < size; i++)
        {
            array[i] = new EntangledPair();
        }
        return array;
    }

    // Returns a random axis
    private static Axis[] GetRandomAxes(int amount)
    {
        Axis[] axes = new Axis[amount];
        for (int index = 0; index < amount; index++)
        {
            Random random = new Random();
            axes[index] = (Axis)(random.NextInt64() % 3);
        }
        return axes;
    }

    [GeneratedRegex("([X-Z][01])|([X-Z][0-9]*)")]
    private static partial Regex ParserRegex();
}