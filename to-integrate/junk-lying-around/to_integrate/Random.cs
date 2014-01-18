interface IRandom
{
}
public class FlexRandom : IRandom
{
    public FlexRandom(int? seed = null) 
    {
        SetRand(seed);
    }
    
    void SetRand(int? seed = null) {
        if(seed == null) {
            rand = new Random();
        }
        else {
            rand = new Random(seed.Value);
        }
    }
    
    public int? Seed {
        get { return seed; }
        set { 
            SetSeed(value);
        }
    }
    
    Random rand;
    
    //public Random Rand { get; private set; }
    
}
