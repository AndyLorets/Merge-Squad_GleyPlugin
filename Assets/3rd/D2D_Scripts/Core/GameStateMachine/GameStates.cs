using System.Linq;

namespace D2D.Core
{
    /// <summary>
    /// Determines some type of game state
    /// </summary>
    public abstract class GameState
    {
        public virtual bool CanRepeat => false;
        
        public override string ToString() => 
            GetType().FullName.Split('.').Last();

        public bool Is<T>() where T : GameState =>
            GetType() == typeof(T);
        
        /// <summary>
        /// Happens when we switch to this state.
        /// </summary>
        public virtual void Action() { }
    }
    
    // ------------------------------------------
    
    public class RunningState : GameState { }

    public class PauseState : GameState { }
    
    public class LoseState : GameState { }
    
    public class WinState : GameState { }

    public class SceneLoading : GameState { }
    
    // ------------------------------------------
    
    public class CustomStateA : GameState { }
    
    public class CustomStateB : GameState { }

    public class CustomStateC : GameState
    {
        public override bool CanRepeat => true;
    }
}