using System;

namespace HomeSen.Helpers.Interfaces
{
    public interface IRegistry
    {
        #region Properties

        IRegistryKey CurrentUser { get; }
        IRegistryKey LocalMachine { get; }
        IRegistryKey ClassesRoot { get; }
        IRegistryKey Users { get; }
        IRegistryKey PerformanceData { get; }
        IRegistryKey CurrentConfig { get; }
        IRegistryKey DynData { get; }

        #endregion

        #region Methods

        object GetValue(string keyName, string valueName, object defaultValue);

        void SetValue(string keyName, string valueName, Object value);
        void SetValue(string keyName, string valueName, Object value, Microsoft.Win32.RegistryValueKind valueKind);

        #endregion
    }
}
