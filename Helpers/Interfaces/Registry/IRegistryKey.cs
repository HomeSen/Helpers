using System;
using System.Collections.Generic;
using System.Text;

namespace HomeSen.Helpers.Interfaces.Registry
{
    public interface IRegistryKey
    {
        #region Properties

        string Name { get; }
        int SubKeyCount { get; }
        int ValueCount { get; }

        #endregion


        #region Methods

        void Close();
        
        IRegistryKey CreateSubKey(string subkey);
        IRegistryKey CreateSubKey(string subkey,
            Microsoft.Win32.RegistryKeyPermissionCheck permissionCheck);
        IRegistryKey CreateSubKey(string subkey,
            Microsoft.Win32.RegistryKeyPermissionCheck permissionCheck,
            System.Security.AccessControl.RegistrySecurity registrySecurity);
        
        void DeleteSubKey(string subkey);
        void DeleteSubKey(string subkey, bool throwOnMissingSubKey);

        void DeleteSubKeyTree(string subkey);

        void DeleteValue(string name);
        void DeleteValue(string name, bool throwOnMissingValue);

        void Flush();

        System.Security.AccessControl.RegistrySecurity GetAccessControl();
        System.Security.AccessControl.RegistrySecurity GetAccessControl(
            System.Security.AccessControl.AccessControlSections includeSections);

        string[] GetSubKeyNames();

        Object GetValue(string name);
        Object GetValue(string name, Object defaultValue);
        Object GetValue(string name, Object defaultValue, Microsoft.Win32.RegistryValueOptions options);

        Microsoft.Win32.RegistryValueKind GetValueKind(string name);

        string[] GetValueNames();

        IRegistryKey OpenRemoteBaseKey(Microsoft.Win32.RegistryHive hKey, string machineName);

        IRegistryKey OpenSubKey(string name);
        IRegistryKey OpenSubKey(string name, bool writable);
        IRegistryKey OpenSubKey(string name, Microsoft.Win32.RegistryKeyPermissionCheck permissionCheck);
        IRegistryKey OpenSubKey(string name,
            Microsoft.Win32.RegistryKeyPermissionCheck permissionCheck,
            System.Security.AccessControl.RegistryRights rights);

        void SetAccessControl(System.Security.AccessControl.RegistrySecurity registrySecurity);

        void SetValue(string name, Object value);
        void SetValue(string name, Object value, Microsoft.Win32.RegistryValueKind valueKind);

        #endregion
    }
}
