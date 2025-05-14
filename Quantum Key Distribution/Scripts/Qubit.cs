public class Qubit{
    public Axis axis;
    public Spin spin;

    public Qubit(){
        axis = Axis.Undefined;
        spin = Spin.Undefined;
    }

    public Qubit(Axis axis, Spin spin){
        this.axis = axis;
        this.spin = spin;
    }

    public Spin MeasureSpin(Axis axis){
        if (this.axis == axis){
            return spin;
        } else {
            this.axis = axis;
            GetRandomspin();
            return spin;
        }
    }

    //Helper method
    public static Spin GetOppositeSpin(Spin spin){
        switch(spin){
            case Spin.Up: return Spin.Down;
            case Spin.Down: return Spin.Up;
            default: throw new Exception("Error!");
        }
    }

    private void GetRandomspin(){
        Random random = new Random();
        spin = (Spin)(random.NextInt64() % 2);
    }
}

public enum Axis{
    Z, Y, X, Undefined
}

public enum Spin{
    Up = 1, Down = 0, Undefined = -1
}