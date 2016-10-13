using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;
using BudgetFramework.Models;
using System.Web;


namespace BudgetFramework
{
    public class LdapAuthentication
    {
        private string _filterAttribute;
        private string _groups;
        public enum SecurityGroup
        {
            User = 1,
            SysAdmin = 2,
            Admin = 3,
            HRView = 4,
        }

        public static List<BudgetUser> LoadUsersInGroup(string groupName)
        {
            List<BudgetUser> users = new List<BudgetUser>();
            string adPath = System.Configuration.ConfigurationManager.AppSettings.Get("LDAP");//path to LDAP directory server
            string domain = System.Configuration.ConfigurationManager.AppSettings.Get("adDomain");
            string adLookupUser = System.Configuration.ConfigurationManager.AppSettings.Get("adLookupUserName");
            string adLookupPassword = System.Configuration.ConfigurationManager.AppSettings.Get("adLookupPassword"); 


            DirectoryEntry searchRoot = new DirectoryEntry(adPath, adLookupUser, adLookupPassword);
            DirectorySearcher search = new DirectorySearcher(searchRoot);

            //sets filter to search AD
            search.Filter = string.Format("(&(objectCategory=person)(objectClass=user)(memberOf=CN={0},CN=Users,DC={1},DC=local))",groupName, domain);
            //property list to search
            search.PropertiesToLoad.Add("cn");
            search.PropertiesToLoad.Add("memberOf");
            search.PropertiesToLoad.Add("whenChanged");
            search.PropertiesToLoad.Add("whenCreated");
            search.PropertiesToLoad.Add("useraccountcontrol");
            search.PropertiesToLoad.Add("sAMAccountName");
            search.PropertiesToLoad.Add("manager");
            search.PropertiesToLoad.Add("initials");
            search.PropertiesToLoad.Add("givenName");
            search.PropertiesToLoad.Add("sn");

            try
            {
                SearchResultCollection results = search.FindAll();

                foreach (SearchResult s in results)
                {
                    BudgetUser u = new BudgetUser();
                    
                    if (s.Properties["sAMAccountName"].Count > 0)
                    {
                        u.UserID = s.Properties["sAMAccountName"][0].ToString();
                    }

                    if (s.Properties["manager"].Count > 0)
                    {
                        //u.ReportsTo = s.Properties["manager"][0].ToString();
                        u.ReportsTo = GetSAMAccountNameFromPath(s.Properties["manager"][0].ToString());
                    }

                    if(s.Properties["cn"].Count > 0)
                    {
                        u.DisplayName = s.Properties["cn"][0].ToString();
                    }

                    if (s.Properties["initials"].Count > 0)
                    {
                        u.Initials = s.Properties["initials"][0].ToString();
                    }

                    if (s.Properties["givenName"].Count > 0)
                    {
                        u.FirstName = s.Properties["givenName"][0].ToString();
                    }
                    if (s.Properties["sn"].Count > 0)
                    {
                        u.LastName = s.Properties["sn"][0].ToString();
                    }


                    int uac = Convert.ToInt32(s.Properties["useraccountcontrol"][0].ToString());
                    u.IsActive = ((uac & 2) != 2 ? true : false); // uac = bitflag value containing the user settings. the 0x2 (or 2) flag is deactivated. the code (uac & 2) != 2 checks that the deactive flag is NOT present.

                    users.Add(u);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error obtaining userlist. " + ex.Message);
            }

            return users;
        }

        public static BudgetUser LoadUser(string username)
        {
            string adPath = System.Configuration.ConfigurationManager.AppSettings.Get("LDAP");
            string domain = System.Configuration.ConfigurationManager.AppSettings.Get("adDomain");
            string adLookupUser = System.Configuration.ConfigurationManager.AppSettings.Get("adLookupUserName");
            string adLookupPassword = System.Configuration.ConfigurationManager.AppSettings.Get("adLookupPassword");

            DirectoryEntry searchRoot = new DirectoryEntry(adPath, adLookupUser, adLookupPassword);

            BudgetUser u = new BudgetUser();

            try
            {
                object obj = searchRoot.NativeObject;

                DirectorySearcher search = new DirectorySearcher(searchRoot);

                int idx = username.IndexOf(@"\");
                if (idx > -1)
                {
                    username = username.Substring(idx);
                    username = username.Replace(@"\", "");
                }

                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                search.PropertiesToLoad.Add("memberOf");
                search.PropertiesToLoad.Add("whenChanged");
                search.PropertiesToLoad.Add("whenCreated");
                search.PropertiesToLoad.Add("useraccountcontrol");
                search.PropertiesToLoad.Add("sAMAccountName");
                search.PropertiesToLoad.Add("manager");
                search.PropertiesToLoad.Add("initials");
                search.PropertiesToLoad.Add("givenName");
                search.PropertiesToLoad.Add("sn");

                SearchResult s = search.FindOne();

                if (s != null)
                {
                    if (s.Properties["sAMAccountName"].Count > 0)
                    {
                        u.UserID = s.Properties["sAMAccountName"][0].ToString();
                    }

                    if (s.Properties["manager"].Count > 0)
                    {
                        //u.ReportsTo = s.Properties["manager"][0].ToString();
                        u.ReportsTo = GetSAMAccountNameFromPath(s.Properties["manager"][0].ToString());
                    }

                    if (s.Properties["cn"].Count > 0)
                    {
                        u.DisplayName = s.Properties["cn"][0].ToString();
                    }

                    if (s.Properties["initials"].Count > 0)
                    {
                        u.Initials = s.Properties["initials"][0].ToString();
                    }

                    if (s.Properties["givenName"].Count > 0)
                    {
                        u.FirstName = s.Properties["givenName"][0].ToString();
                    }
                    if (s.Properties["sn"].Count > 0)
                    {
                        u.LastName = s.Properties["sn"][0].ToString();
                    }


                    int uac = Convert.ToInt32(s.Properties["useraccountcontrol"][0].ToString());
                    u.IsActive = ((uac & 2) != 2 ? true : false); // uac = bitflag value containing the user settings. the 0x2 (or 2) flag is deactivated. the code (uac & 2) != 2 checks that the deactive flag is NOT present.
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error loading user. " + ex.Message);
            }

            return u;
        }

        protected static string LoadUserDN(string username)
        {
            string adPath = System.Configuration.ConfigurationManager.AppSettings.Get("LDAP");
            string domain = System.Configuration.ConfigurationManager.AppSettings.Get("adDomain");
            string adLookupUser = System.Configuration.ConfigurationManager.AppSettings.Get("adLookupUserName");
            string adLookupPassword = System.Configuration.ConfigurationManager.AppSettings.Get("adLookupPassword");

            DirectoryEntry searchRoot = new DirectoryEntry(adPath, adLookupUser, adLookupPassword);
          

            string _dn = "";

            try
            {
                object obj = searchRoot.NativeObject;
                DirectorySearcher search = new DirectorySearcher(searchRoot);

                int idx = username.IndexOf(@"\");
                if (idx > -1)
                {
                    username = username.Substring(idx);
                    username = username.Replace(@"\", "");
                }

                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("distinguishedName");
                search.PropertiesToLoad.Add("sAMAccountName");
                SearchResult s = search.FindOne();

                if (s != null)
                {

                    if (s.Properties["distinguishedName"].Count > 0)
                    {
                        _dn = s.Properties["distinguishedName"][0].ToString();
                    }
                }

            
            }
            catch (Exception ex)
            {
                throw new Exception("Error obtaining user. " + ex.Message);
            }

            return _dn;
        }

        public static List<BudgetUser> LoadUsersForManager(string managerName)
        {

            string _managerDN = LoadUserDN(managerName);

            List<BudgetUser> users = new List<BudgetUser>();

            if (_managerDN == "")
            {
                return users;
            }

            string adPath = System.Configuration.ConfigurationManager.AppSettings.Get("LDAP");
            string domain = System.Configuration.ConfigurationManager.AppSettings.Get("adDomain");
            string adLookupUser = System.Configuration.ConfigurationManager.AppSettings.Get("adLookupUserName");
            string adLookupPassword = System.Configuration.ConfigurationManager.AppSettings.Get("adLookupPassword");


            DirectoryEntry searchRoot = new DirectoryEntry(adPath, adLookupUser, adLookupPassword);
            DirectorySearcher search = new DirectorySearcher(searchRoot);

            search.Filter = string.Format("(&(objectCategory=person)(objectClass=user)(manager={0}))", _managerDN);
            //search.Filter = string.Format("(&(objectCategory=user)(manager={0}))", managerName);

            search.PropertiesToLoad.Add("cn");
            search.PropertiesToLoad.Add("memberOf");
            search.PropertiesToLoad.Add("whenChanged");
            search.PropertiesToLoad.Add("whenCreated");
            search.PropertiesToLoad.Add("useraccountcontrol");
            search.PropertiesToLoad.Add("sAMAccountName");
            search.PropertiesToLoad.Add("manager");
            search.PropertiesToLoad.Add("initials");
            search.PropertiesToLoad.Add("givenName");
            search.PropertiesToLoad.Add("sn");

            try
            {
                SearchResultCollection results = search.FindAll();

                foreach (SearchResult s in results)
                {
                    BudgetUser u = new BudgetUser();

                    if (s.Properties["sAMAccountName"].Count > 0)
                    {
                        u.UserID = s.Properties["sAMAccountName"][0].ToString();
                    }

                    if (s.Properties["manager"].Count > 0)
                    {
                        //u.ReportsTo = s.Properties["manager"][0].ToString();
                        u.ReportsTo = GetSAMAccountNameFromPath(s.Properties["manager"][0].ToString());
                    }

                    if (s.Properties["cn"].Count > 0)
                    {
                        u.DisplayName = s.Properties["cn"][0].ToString();
                    }

                    if (s.Properties["initials"].Count > 0)
                    {
                        u.Initials = s.Properties["initials"][0].ToString();
                    }

                    if (s.Properties["givenName"].Count > 0)
                    {
                        u.FirstName = s.Properties["givenName"][0].ToString();
                    }
                    if (s.Properties["sn"].Count > 0)
                    {
                        u.LastName = s.Properties["sn"][0].ToString();
                    }


                    int uac = Convert.ToInt32(s.Properties["useraccountcontrol"][0].ToString());
                    u.IsActive = ((uac & 2) != 2 ? true : false); // uac = bitflag value containing the user settings. the 0x2 (or 2) flag is deactivated. the code (uac & 2) != 2 checks that the deactive flag is NOT present.

                    users.Add(u);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error obtaining userlist. " + ex.Message);
            }

            return users;
        }

        public static string GetSAMAccountNameFromPath(string UserPath)
        {
            string adPath = System.Configuration.ConfigurationManager.AppSettings.Get("LDAP");
            string domain = System.Configuration.ConfigurationManager.AppSettings.Get("adDomain");
            string adLookupUser = System.Configuration.ConfigurationManager.AppSettings.Get("adLookupUserName");
            string adLookupPassword = System.Configuration.ConfigurationManager.AppSettings.Get("adLookupPassword");

            DirectoryEntry searchRoot = new DirectoryEntry(adPath, adLookupUser, adLookupPassword);
            DirectorySearcher search = new DirectorySearcher(searchRoot);

            search.Filter = string.Format("(&(objectCategory=person)(objectClass=user)(distinguishedName={0}))", UserPath);

            search.PropertiesToLoad.Add("samaccountname");

            string name = "";

            try
            {
                SearchResult s = search.FindOne();

                if (s.Properties["sAMAccountName"].Count > 0)
                {
                    name = s.Properties["sAMAccountName"][0].ToString();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error obtaining manager names. " + ex.Message);
            }


            return name;
        }

        public bool IsAuthenticated(string domain, string username, string pwd)
        {

            string domainAndUsername = domain + @"\" + username;
            string adPath = System.Configuration.ConfigurationManager.AppSettings.Get("LDAP");

            DirectoryEntry entry = new DirectoryEntry(adPath, domainAndUsername, pwd);

            bool isvalid = true;

            try
            {
                //Bind to the native AdsObject to force authentication.
                object obj = entry.NativeObject;

                DirectorySearcher search = new DirectorySearcher(entry);

                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                search.PropertiesToLoad.Add("manager");
                SearchResult result = search.FindOne();

                if (null == result)
                {
                    isvalid = false;
                }
                else
                {

                    //Update the new path to the user in the directory.
                    //_path = result.Path;
                    _filterAttribute = (string)result.Properties["cn"][0];
                    LoadGroups(adPath, domainAndUsername, pwd);
                    isvalid = true;
                }
            }
            catch (Exception ex)
            {
                //throw new Exception("Error authenticating user. " + ex.Message);
                isvalid = false;
            }

            return isvalid;
        }

        public bool IsAuthenticated(System.Security.Principal.IIdentity ident)
        {
            string adPath = System.Configuration.ConfigurationManager.AppSettings.Get("LDAP");

            //string domainAndUsername = domain + @"\" + username;
            DirectoryEntry entry = new DirectoryEntry(adPath);
            
            bool isvalid = false;           

            try
            {
                //Bind to the native AdsObject to force authentication.
                object obj = entry.NativeObject;

                DirectorySearcher search = new DirectorySearcher(entry);

                string username = Environment.UserName;
                int idx = username.IndexOf(@"\");
                if (idx > -1)
                {
                    username = username.Substring(idx);
                    username = username.Replace(@"\", "");
                }

                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                search.PropertiesToLoad.Add("manager");
                SearchResult result = search.FindOne();

                if (null == result)
                {
                    isvalid = false;
                }
                else
                {
                    //Update the new path to the user in the directory.
                    //_path = result.Path;
                    _filterAttribute = (string)result.Properties["cn"][0];
                    LoadGroups(adPath);
                    isvalid = true;
                }
            }
            catch (Exception ex)
            {
                //throw new Exception("Error authenticating user. " + ex.Message);
                isvalid = false;
            }

            return isvalid; 
        }

        private void LoadGroups(string path, string domainAndUsername, string pwd)
        {
            DirectoryEntry searchRoot = new DirectoryEntry(path, domainAndUsername, pwd);
            DirectorySearcher search = new DirectorySearcher(searchRoot);
            search.Filter = "(cn=" + _filterAttribute + ")";
            search.PropertiesToLoad.Add("memberOf");
            StringBuilder groupNames = new StringBuilder();

            _groups = "";

            try
            {
                SearchResult result = search.FindOne();
                int propertyCount = result.Properties["memberOf"].Count;
                string dn;
                int equalsIndex, commaIndex;

                for (int propertyCounter = 0; propertyCounter < propertyCount; propertyCounter++)
                {
                    dn = (string)result.Properties["memberOf"][propertyCounter];
                    equalsIndex = dn.IndexOf("=", 1);
                    commaIndex = dn.IndexOf(",", 1);
                    if (-1 == equalsIndex)
                    {
                        return;
                    }
                    groupNames.Append(dn.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1));
                    groupNames.Append("|");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error obtaining group names. " + ex.Message);
            }
            _groups = groupNames.ToString();
        }

        private void LoadGroups(string path)
        {
            DirectoryEntry searchRoot = new DirectoryEntry(path);
            DirectorySearcher search = new DirectorySearcher(searchRoot);
            search.Filter = "(cn=" + _filterAttribute + ")";
            search.PropertiesToLoad.Add("memberOf");
            StringBuilder groupNames = new StringBuilder();

            _groups = "";

            try
            {
                SearchResult result = search.FindOne();
                int propertyCount = result.Properties["memberOf"].Count;
                string dn;
                int equalsIndex, commaIndex;

                for (int propertyCounter = 0; propertyCounter < propertyCount; propertyCounter++)
                {
                    dn = (string)result.Properties["memberOf"][propertyCounter];
                    equalsIndex = dn.IndexOf("=", 1);
                    commaIndex = dn.IndexOf(",", 1);
                    if (-1 == equalsIndex)
                    {
                        return;
                    }
                    groupNames.Append(dn.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1));
                    groupNames.Append("|");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error obtaining group names. " + ex.Message);
            }
            _groups = groupNames.ToString();
        }

        public string GetGroups()
        {
            return _groups;
        }


        public static bool IsCurrentUserInGroup(HttpContext context, SecurityGroup group)
        {
            string groupName = "";

            switch (group)
            {
                case SecurityGroup.User:
                    groupName = System.Configuration.ConfigurationManager.AppSettings.Get("BudgetUsers");
                    break;
                case SecurityGroup.Admin:
                    groupName = System.Configuration.ConfigurationManager.AppSettings.Get("BudgetAdmin");
                    break;
                case SecurityGroup.HRView:
                    groupName = System.Configuration.ConfigurationManager.AppSettings.Get("BudgetHRView");
                    break;
                case SecurityGroup.SysAdmin:
                    groupName = System.Configuration.ConfigurationManager.AppSettings.Get("BudgetSysAdmin");
                    break;

                default:
                    break;
            }

            return context.User.IsInRole(groupName);
        }

        /// <summary>
        /// checks all groups to make sure that the user is in at least one of them.  If the user is not in any Budget related groups, returns false
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool IsCurrentUserAuthenticated(HttpContext context)
        {
            bool isValid = false;

            if(IsCurrentUserInGroup(context, SecurityGroup.User))
            {
                isValid = true;
            }
            else if (IsCurrentUserInGroup(context, SecurityGroup.Admin))
            {
                isValid = true;
            }
            else if (IsCurrentUserInGroup(context, SecurityGroup.SysAdmin))
            {
                isValid = true;
            }
            else if (IsCurrentUserInGroup(context, SecurityGroup.HRView))
            {
                isValid = true;
            }


            return isValid;
        }

        public static string GetCurrentUserName(HttpContext context)
        {
            string domain = System.Configuration.ConfigurationManager.AppSettings.Get("adDomain");

            domain = domain + @"\";

            string username = context.User.Identity.Name;

            if (context.User.Identity.Name.StartsWith(domain))
            {
                return username.Replace(domain, "");
            }
            else
            {
                return username;
            }
        }
    }
}
