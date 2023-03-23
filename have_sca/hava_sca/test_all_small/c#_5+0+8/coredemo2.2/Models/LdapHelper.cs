using System;
using System.Configuration;
using System.DirectoryServices;

namespace coredemo2._2.Models
{
    public class LdapHelper
    {
        string ldapUrl = string.Empty;
        string ldapUserName = string.Empty;
        string ldapPassword = string.Empty;

        public DirectoryEntry OpenConnection(string domain) {
            this.ldapUrl = ConfigurationManager.ConnectionStrings["LdapHost"].ConnectionString;
            this.ldapUserName = ConfigurationManager.ConnectionStrings["ldapUserName"].ConnectionString;
            this.ldapPassword = ConfigurationManager.ConnectionStrings["ldapPassword"].ConnectionString;
            try
            {
                DirectoryEntry root = new DirectoryEntry(ldapUrl + domain, ldapUserName, ldapPassword, AuthenticationTypes.None);
                string strName = root.Name;//失败，会抛出异常
                return root;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}