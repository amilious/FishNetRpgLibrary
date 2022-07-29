using Amilious.Core.Extensions;

namespace Amilious.Core {
    
    public static class AmiliousCore {

        public const string NO_EXECUTOR = "No Amilious Executor exists in the scene.  Actions will not be invoked!";

        public const string MAIN_CONTEXT_MENU = "Amilious/";

        public const string THREADING_CONTEXT_MENU = MAIN_CONTEXT_MENU + "Threading/";

        public const string INVALID_SUCCESS = "The value property is not available unless state is Success.";

        public const string INVALID_ERROR = "The error property is not available unless state is Error.";

        public const string INVALID_PENDING = "Cannot process a future that isn't in the Pending state.";

        public static string MakeTitle(string title) {
            return title.PadText('#', 60, 10).SetColor("ffff88");
        }
        
    }
    
}