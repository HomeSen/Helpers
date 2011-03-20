using System;
using System.Collections.Generic;
using System.Text;
using HomeSen.Helpers.Interfaces.Registry;

namespace HomeSen.Helpers.Proxies
{
    class RegistryKey : IRegistryKey
    {
        #region Operator overrides

        public static explicit operator Microsoft.Win32.RegistryKey(RegistryKey obj)
        {
            return obj._regKey;
        }

        public static implicit operator RegistryKey(Microsoft.Win32.RegistryKey obj)
        {
            return new RegistryKey(obj);
        }

        #endregion
        
        
        #region Fields

        private Microsoft.Win32.RegistryKey _regKey = null;
        
        #endregion


        #region Construtors

        internal RegistryKey(Microsoft.Win32.RegistryKey regKey)
        {
            this._regKey = regKey;
        }

        #endregion


        #region IRegistryKey Members

        public string Name
        {
            get { return _regKey.Name; }
        }

        public int SubKeyCount
        {
            get { return _regKey.SubKeyCount; }
        }

        public int ValueCount
        {
            get { return _regKey.ValueCount; }
        }

        public void Close()
        {
            _regKey.Close();
        }

        public IRegistryKey CreateSubKey(string subkey)
        {
            return (RegistryKey)_regKey.CreateSubKey(subkey);
        }

        public IRegistryKey CreateSubKey(string subkey, Microsoft.Win32.RegistryKeyPermissionCheck permissionCheck)
        {
            return (RegistryKey)_regKey.CreateSubKey(subkey, permissionCheck);
        }

        public IRegistryKey CreateSubKey(string subkey, Microsoft.Win32.RegistryKeyPermissionCheck permissionCheck, System.Security.AccessControl.RegistrySecurity registrySecurity)
        {
            return (RegistryKey)_regKey.CreateSubKey(subkey, permissionCheck, registrySecurity);
        }

        public void DeleteSubKey(string subkey)
        {
            _regKey.DeleteSubKey(subkey);
        }

        public void DeleteSubKey(string subkey, bool throwOnMissingSubKey)
        {
            _regKey.DeleteSubKey(subkey, throwOnMissingSubKey);
        }

        public void DeleteSubKeyTree(string subkey)
        {
            _regKey.DeleteSubKeyTree(subkey);
        }

        public void DeleteValue(string name)
        {
            _regKey.DeleteValue(name);
        }

        public void DeleteValue(string name, bool throwOnMissingValue)
        {
            _regKey.DeleteValue(name, throwOnMissingValue);
        }

        public void Flush()
        {
            _regKey.Flush();
        }

        public System.Security.AccessControl.RegistrySecurity GetAccessControl()
        {
            return _regKey.GetAccessControl();
        }

        public System.Security.AccessControl.RegistrySecurity GetAccessControl(System.Security.AccessControl.AccessControlSections includeSections)
        {
            return _regKey.GetAccessControl(includeSections);
        }

        public string[] GetSubKeyNames()
        {
            return _regKey.GetSubKeyNames();
        }

        public object GetValue(string name)
        {
            return _regKey.GetValue(name);
        }

        public object GetValue(string name, object defaultValue)
        {
            return _regKey.GetValue(name, defaultValue);
        }

        public object GetValue(string name, object defaultValue, Microsoft.Win32.RegistryValueOptions options)
        {
            return _regKey.GetValue(name, defaultValue, options);
        }

        public Microsoft.Win32.RegistryValueKind GetValueKind(string name)
        {
            return _regKey.GetValueKind(name);
        }

        public string[] GetValueNames()
        {
            return _regKey.GetValueNames();
        }

        public IRegistryKey OpenRemoteBaseKey(Microsoft.Win32.RegistryHive hKey, string machineName)
        {
            return (RegistryKey)Microsoft.Win32.RegistryKey.OpenRemoteBaseKey(hKey, machineName);
        }

        public IRegistryKey OpenSubKey(string name)
        {
            return (RegistryKey)_regKey.OpenSubKey(name);
        }

        public IRegistryKey OpenSubKey(string name, bool writable)
        {
            return (RegistryKey)_regKey.OpenSubKey(name, writable);
        }

        public IRegistryKey OpenSubKey(string name, Microsoft.Win32.RegistryKeyPermissionCheck permissionCheck)
        {
            return (RegistryKey)_regKey.OpenSubKey(name, permissionCheck);
        }

        public IRegistryKey OpenSubKey(string name, Microsoft.Win32.RegistryKeyPermissionCheck permissionCheck, System.Security.AccessControl.RegistryRights rights)
        {
            return (RegistryKey)_regKey.OpenSubKey(name, permissionCheck, rights);
        }

        public void SetAccessControl(System.Security.AccessControl.RegistrySecurity registrySecurity)
        {
            _regKey.SetAccessControl(registrySecurity);
        }

        public void SetValue(string name, object value)
        {
            _regKey.SetValue(name, value);
        }

        public void SetValue(string name, object value, Microsoft.Win32.RegistryValueKind valueKind)
        {
            _regKey.SetValue(name, value, valueKind);
        }

        #endregion
    }
}
