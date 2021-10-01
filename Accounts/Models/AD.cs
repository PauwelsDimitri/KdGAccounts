using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.Models
{
    public class AD
    {

        private DirectoryEntry GetDirectoryEntry(string Domain)
        { var path = "LDAP://admin.kdg.be:389/DC=admin,DC=kdg,DC=be";
            if (Domain == "STUDENT")
            {
                path = "LDAP://student.kdg.be:389/DC=student,DC=kdg,DC=be";
            }
            if (Domain == "KDG")
            {
                path = "LDAP://kdg.be:389/DC=kdg,DC=be";
            }
          
            DirectoryEntry oDE = new DirectoryEntry(path);
            oDE.AuthenticationType = AuthenticationTypes.Secure;
            return oDE;
        }
        private DateTime GetDateTime(long LDate)
        {
            return DateTime.FromFileTime(LDate);
        }

        private SearchResult GetGroup(string AccountName, string Domain)
        {
            DirectoryEntry de = GetDirectoryEntry(Domain);
            DirectorySearcher ds = new DirectorySearcher();
            ds.SearchRoot = de;

            ds.Filter = "(&(objectClass=group)(SAMAccountName=" + AccountName + "))";
            ds.SearchScope = SearchScope.Subtree;
            ds.PropertiesToLoad.Add("cn");
            ds.PropertiesToLoad.Add("SAMAccountName");
            ds.PropertiesToLoad.Add("name");
            SearchResult result = ds.FindOne();
            return result;
        }

        private SearchResultCollection GetGroups(string term, string Domain, int size)
        {
            DirectoryEntry de = GetDirectoryEntry(Domain);
            DirectorySearcher ds = new DirectorySearcher();
            ds.SearchRoot = de;

            ds.Filter = "(&(objectClass=group)(SAMAccountName=*" + term + "*))";
            ds.SearchScope = SearchScope.Subtree;
            ds.SizeLimit = size;
            ds.PropertiesToLoad.Add("cn");
            ds.PropertiesToLoad.Add("SAMAccountName");
            ds.PropertiesToLoad.Add("name");
            ds.Sort.Direction = SortDirection.Ascending;
            ds.Sort.PropertyName = "cn";
            SearchResultCollection results = ds.FindAll();

            return results;
        }

        private SearchResult GetUser(string UserName, string Domain)
        {
            DirectoryEntry de = GetDirectoryEntry(Domain);
            DirectorySearcher ds = new DirectorySearcher();
            ds.SearchRoot = de;

            ds.Filter = "(&(objectClass=user)(SAMAccountName=" + UserName + "))";
            ds.SearchScope = SearchScope.Subtree;
            ds.PropertiesToLoad.Add("cn");
            ds.PropertiesToLoad.Add("SAMAccountName");
            ds.PropertiesToLoad.Add("description");
            ds.PropertiesToLoad.Add("accountExpires");
            ds.PropertiesToLoad.Add("lastLogonTimestamp");
            SearchResult result = ds.FindOne();
            return result;
        }

        private SearchResultCollection GetUsers(string Domain, int size)
        {
            DirectoryEntry de = GetDirectoryEntry(Domain);
            DirectorySearcher ds = new DirectorySearcher();
            ds.SearchRoot = de;

            ds.Filter = "(&(objectClass=user))";
            ds.SearchScope = SearchScope.Subtree;
            ds.SizeLimit = size;
            ds.PropertiesToLoad.Add("cn");
            ds.PropertiesToLoad.Add("SAMAccountName");
            ds.PropertiesToLoad.Add("description");
            ds.PropertiesToLoad.Add("accountExpires");
            ds.Sort.Direction = SortDirection.Ascending;
            ds.Sort.PropertyName = "cn";
            SearchResultCollection results = ds.FindAll();

            return results;
        }

        public bool ADGroupExist(string SamAccountName, string Domain)
        {
            bool Exist = false;
            SearchResult result = GetGroup(SamAccountName, Domain);
            if (result != null && result.Path != null)
            {
                Exist = true;
            }
            return Exist;
        }

        public ADGroup GetADGroup(string SamAccountName, string Domain)
        {
            String Account = "";
            String cn = "";
            String Name = "";

            SearchResult result = GetGroup(SamAccountName, Domain);
            if (result != null)
            {
                Account = result.Properties["SAMAccountName"][0].ToString();
                Name = result.Properties["Name"][0].ToString();
                cn = result.Properties["cn"][0].ToString();
            }
            
            ADGroup aDGroup = new ADGroup(cn, Name, SamAccountName);
            return aDGroup;
        }

        public List<ADGroup> GetADGroups(string term, string Domain, int size)
        {
            SearchResultCollection results = new AD().GetGroups(term, Domain, size);
            List<ADGroup> aDGroups = new List<ADGroup>();
            foreach (SearchResult result in results)
            {
                String Account;
                String cn;
                String Name;

                Account = result.Properties["SAMAccountName"][0].ToString();
                Name = result.Properties["Name"][0].ToString();
                cn = result.Properties["cn"][0].ToString();
                aDGroups.Add(new ADGroup(cn, Name, Account));
            }
            return aDGroups;
        }

        public ADUser GetADUser(string SamAccountName, string Domain)
        {
            bool Expired = true;
            DateTime AccountExpires = new DateTime();
            DateTime dtLastLogon = new DateTime();
            String Account ="";
            String Description = "";
            String Name = "";

            SearchResult result = GetUser(SamAccountName,Domain);

            if (result != null)
            {
                if (result.Properties["SAMAccountName"] != null && result.Properties["SAMAccountName"].Count > 0)
                {
                    Account = result.Properties["SAMAccountName"][0].ToString();
                }
                if (result.Properties["description"] != null && result.Properties["description"].Count > 0)
                {
                    Description = result.Properties["description"][0].ToString();
                }
                if (result.Properties["cn"] != null && result.Properties["cn"].Count > 0)
                {
                    Name = result.Properties["cn"][0].ToString();
                }
                if (result.Properties["accountExpires"] != null && result.Properties["accountExpires"].Count > 0)
                {
                    if ((long)result.Properties["accountExpires"][0] != 9223372036854775807)
                    {
                        AccountExpires = GetDateTime((long)result.Properties["accountExpires"][0]);
                    }
                }
                if (result.Properties["lastLogonTimestamp"] != null && result.Properties["lastLogonTimestamp"].Count > 0)
                {
                    dtLastLogon = GetDateTime((long)result.Properties["lastLogonTimestamp"][0]);
                }
                if (AccountExpires < DateTime.Now)
                {
                    Expired = true;
                }
                else
                {
                    Expired = false;
                }
            }

            ADUser aDUser = new ADUser(Name, Account, Description, dtLastLogon, AccountExpires, Expired);
            return aDUser;
        }

        public List<ADUser> GetADUsers(string Domain, int size)
        {
            SearchResultCollection results = new AD().GetUsers(Domain,size);
            List<ADUser> aDUsers = new List<ADUser>();
            foreach (SearchResult result in results)
            {
                bool Expired;
                DateTime AccountExpires = new DateTime();
                DateTime dtLastLogon = new DateTime();
                String Account;
                String Description;
                String Name;

                AccountExpires = GetDateTime((long)result.Properties["accountExpires"][0]);
                Account = result.Properties["SAMAccountName"][0].ToString();
                Description = result.Properties["description"][0].ToString();
                Name = result.Properties["cn"][0].ToString();
                if (AccountExpires < DateTime.Now)
                {
                    Expired = true;
                }
                else
                {
                    Expired = false;
                }
                if (result.Properties["lastLogonTimestamp"] != null && result.Properties["lastLogonTimestamp"].Count > 0)
                {
                    long lastLogon = (long)result.Properties["lastLogonTimestamp"][0];
                    dtLastLogon = DateTime.FromFileTime(lastLogon);
                }
                aDUsers.Add(new ADUser(Name, Account, Description,dtLastLogon, AccountExpires, Expired));
            }

            return aDUsers;

        }
    }
}
