

public abstract class AIState
{
    public string Commentary
    {
        get => _commentary;
        protected set => _commentary = value;
    }

    private string _commentary = ""; 
    
    public abstract void OnEnter();
    public abstract void Tick();
    public abstract void OnExit();
}