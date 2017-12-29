using System.Collections.Generic;

namespace CyDar.ServiceLocator.Tests.Interfaces
{
    public interface IDumbLog
    {
        List<string> LogMessages { get; }

        void Log(string message);
    }
}
