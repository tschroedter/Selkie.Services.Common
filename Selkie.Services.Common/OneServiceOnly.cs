using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using JetBrains.Annotations;

namespace Selkie.Services.Common
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    internal class OneServiceOnly : IDisposable
    {
        private const int TimeOut = 1000; // 1 second
        public bool HasHandle;
        private Mutex m_Mutex;

        public OneServiceOnly([NotNull] string serviceName)
        {
            InitializeMutex(serviceName);

            try
            {
                HasHandle = m_Mutex.WaitOne(TimeOut,
                                            false);

                if ( HasHandle == false )
                {
                    throw new TimeoutException("Timeout waiting for exclusive access!");
                }
            }
            catch ( AbandonedMutexException )
            {
                HasHandle = true;
            }
        }

        public void Dispose()
        {
            if ( m_Mutex == null )
            {
                return;
            }

            if ( HasHandle )
            {
                m_Mutex.ReleaseMutex();
            }
            m_Mutex.Dispose();
        }

        private void InitializeMutex([NotNull] string serviceName)
        {
            string mutexId = string.Format("Global\\{{{0}}}",
                                           serviceName);

            m_Mutex = new Mutex(false,
                                mutexId);

            MutexAccessRule allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid,
                                                                                           null),
                                                                    MutexRights.FullControl,
                                                                    AccessControlType.Allow);
            MutexSecurity securitySettings = new MutexSecurity();
            securitySettings.AddAccessRule(allowEveryoneRule);
            m_Mutex.SetAccessControl(securitySettings);
        }
    }

    internal interface IOneServiceOnly
    {
    }
}