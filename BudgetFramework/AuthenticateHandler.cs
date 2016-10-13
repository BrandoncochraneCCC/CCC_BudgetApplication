using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Security.Principal;


namespace BudgetFramework
{
    public class AuthenticateHandler : IHttpModule
    {

        public void Dispose()
        {
            //throw new NotImplementedException();
        }


        public void Init(HttpApplication context)
        {
            context.PostAuthenticateRequest += new EventHandler(Application_PostAuthenticateRequest);
        } 


        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.Cookies.Get("authCookie") != null)
            {
                return;
            }



            string strUserIdentity = HttpContext.Current.User.Identity.Name;

            string adPath = System.Configuration.ConfigurationManager.AppSettings.Get("LDAP"); //Path to your LDAP directory server
            string domain = System.Configuration.ConfigurationManager.AppSettings.Get("adDomain");
            string username = System.Configuration.ConfigurationManager.AppSettings.Get("adTestUsername");
            string password = System.Configuration.ConfigurationManager.AppSettings.Get("adTestUserPassword");
            //string adPath = "LDAP://uldc"; 
            LdapAuthentication adAuth = new LdapAuthentication();

            //if (true == adAuth.IsAuthenticated(e.Identity))

            bool isAuthenticated = false;

            string useUNPW = System.Configuration.ConfigurationManager.AppSettings.Get("useADTestUNPW");

            if (useUNPW.ToLower() == "true")
            {
                isAuthenticated = adAuth.IsAuthenticated(domain, username, password);
            }
            else
            {
                isAuthenticated = adAuth.IsAuthenticated(HttpContext.Current.User.Identity);
                username = LdapAuthentication.GetCurrentUserName(HttpContext.Current);//HttpContext.Current.User.Identity.Name;

            }


            //if(true == adAuth.IsAuthenticated(domain, username, password))
            if (isAuthenticated)
            {
                string groups = adAuth.GetGroups();

                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1,
                            username, DateTime.Now, DateTime.Now.AddMinutes(60), false, groups);

                //Encrypt the ticket.
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                //Create a cookie, and then add the encrypted ticket to the cookie as data.
                HttpCookie authCookie = new HttpCookie("authCookie", encryptedTicket);
                //authCookie.Expires = DateTime.Now.AddMinutes(60);
                //Add the cookie to the outgoing cookies collection.
                HttpContext.Current.Response.Cookies.Add(authCookie);


                GenericIdentity genIdent = new GenericIdentity(authTicket.Name);
                string delim = "|";
                string[] strRoles = authTicket.UserData.Split(delim.ToCharArray());
                GenericPrincipal principal = new GenericPrincipal(genIdent, strRoles);
                HttpContext.Current.User = principal;

            }
        } 

    }
}
