using CyDar.ServiceLocator.Tests.Interfaces;
using System.Collections.Generic;

namespace CyDar.ServiceLocator.Tests.Concretes
{
    class DumbLog : IDumbLog
    {
        public List<string> LogMessages
        {
            get;
            private set;

        }

        public DumbLog()
        {
            LogMessages = new List<string>();
        }

        public void Log(string message)
        {
            LogMessages.Add(message);
        }


    }
}
