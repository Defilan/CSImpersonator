using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace CSImpersonator
{
    public class CookieJar :
        IDisposable
    {
        private WindowsImpersonationContext _impersonationContext;

        public CookieJar(string username, string domainname, string password)
        {
            ImpersonateUser(username, domainname, password);
        }

        public void Dispose()
        {
            UndoImpersonation();
        }

        private void ImpersonateUser(string username, string domainname, string password)
        {
            var token = IntPtr.Zero;
            var tokenDuplicate = IntPtr.Zero;
            try
            {
                if (RevertToSelf())
                {
                    if (
                        LogonUser(username, domainname, password, Logon32LogonInteractive, Logon32ProviderDefault,
                            ref token) != 0)
                    {
                        if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                        {
                            var tempIdentity = new WindowsIdentity(tokenDuplicate);
                            _impersonationContext = tempIdentity.Impersonate();
                        }
                        else
                        {
                            throw new Win32Exception(Marshal.GetLastWin32Error());
                        }
                    }

                    else
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    }
                }
                else
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
            finally
            {
                if (token != IntPtr.Zero)
                {
                    CloseHandle(token);
                }
                if (tokenDuplicate != IntPtr.Zero)
                {
                    CloseHandle(tokenDuplicate);
                }
            }
        }

        private void UndoImpersonation()
        {
            _impersonationContext?.Undo();
        }

        #region DLLImports,

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int LogonUser(
            string lpszUserName,
            string lpszDomain,
            string lpszPassword,
            int dwLogonType,
            int dwLogonProvider,
            ref IntPtr phToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int DuplicateToken(
            IntPtr hToken,
            int impersonationLevel,
            ref IntPtr hNewToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool RevertToSelf();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool CloseHandle(
            IntPtr handle);

        private const int Logon32LogonInteractive = 2;
        private const int Logon32ProviderDefault = 0;

        #endregion
    }
}