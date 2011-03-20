using HomeSen.Helpers.Interfaces;

namespace HomeSen.Helpers.Proxies
{
    class Registry : IRegistry
    {
        #region IRegistry Members

        public IRegistryKey CurrentUser
        {
            get { return (RegistryKey)Microsoft.Win32.Registry.CurrentUser; }
        }

        public IRegistryKey LocalMachine
        {
            get { return (RegistryKey)Microsoft.Win32.Registry.LocalMachine; }
        }

        public IRegistryKey ClassesRoot
        {
            get { return (RegistryKey)Microsoft.Win32.Registry.ClassesRoot; }
        }

        public IRegistryKey Users
        {
            get { return (RegistryKey)Microsoft.Win32.Registry.Users; }
        }

        public IRegistryKey PerformanceData
        {
            get { return (RegistryKey)Microsoft.Win32.Registry.PerformanceData; }
        }

        public IRegistryKey CurrentConfig
        {
            get { return (RegistryKey)Microsoft.Win32.Registry.CurrentConfig; }
        }

        public IRegistryKey DynData
        {
            get { return (RegistryKey)Microsoft.Win32.Registry.DynData; }
        }

        public object GetValue(string keyName, string valueName, object defaultValue)
        {
            return Microsoft.Win32.Registry.GetValue(keyName, valueName, defaultValue);
        }

        public void SetValue(string keyName, string valueName, object value)
        {
            Microsoft.Win32.Registry.SetValue(keyName, valueName, value);
        }

        public void SetValue(string keyName, string valueName, object value, Microsoft.Win32.RegistryValueKind valueKind)
        {
            Microsoft.Win32.Registry.SetValue(keyName, valueName, value, valueKind);
        }

        #endregion
    }
}
