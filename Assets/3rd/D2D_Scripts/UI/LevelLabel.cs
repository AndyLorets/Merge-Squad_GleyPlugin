using D2D.Databases;
using D2D.Utilities;

namespace D2D.UI
{
    public class LevelLabel : LabelBase
    {
        protected override float UpdateRate => 999f;

        protected override float StartRate => .1f;

        protected override string GetText() => 
            $"{this.FindLazy<GameProgressionDatabase>().PassedLevels.Value + 1}";
    }
} 