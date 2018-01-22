using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using JetBrains.Annotations;

namespace Core2.Selkie.Services.Common
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    internal class OneServiceOnly : IDisposable
    {
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

        private const int TimeOut = 1000; // 1 

        [UsedImplicitly]
        public bool HasHandle;

        private Mutex m_Mutex;

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

            var allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid,
                                                                               null),
                                                        MutexRights.FullControl,
                                                        AccessControlType.Allow);
            var securitySettings = new MutexSecurity();
            securitySettings.AddAccessRule(allowEveryoneRule);
            m_Mutex.SetAccessControl(securitySettings);
        }
    }
}