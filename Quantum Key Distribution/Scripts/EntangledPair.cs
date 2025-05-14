public class EntangledPair{
    public Qubit p1;
    public Qubit p2;
    public bool IsEntangled;

    public EntangledPair(){
        p1 = new Qubit();
        p2 = new Qubit();
        IsEntangled = true;
    }

    public void MeasureSpin(Axis axis, string particle){
        if (IsEntangled){
            switch(particle){
                case "p1":
                    p1.MeasureSpin(axis);
                    p2.spin = Qubit.GetOppositeSpin(p1.spin);
                    break;
                case "p2":
                    p2.MeasureSpin(axis);
                    p1.spin = Qubit.GetOppositeSpin(p2.spin);
                    break;
            }
            IsEntangled = false;
        }
    }
}