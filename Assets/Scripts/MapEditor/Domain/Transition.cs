public class Transition
{
    public Spawn Start { get; }
    public Spawn Destination { get; }

    public Transition(Spawn start, Spawn destination)
    {
        Start = start;
        Destination = destination;
    }
}