using Accounts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Principal;

namespace Accounts.Custom
{
    public class Log
    {
        public void Add(string UniqueId, string App, String Event, KDGIDENTITYContext _context)
        {
            //KDGIDENTITYContext _context = new KDGIDENTITYContext();

            DateTime Logdate = DateTime.Now;
            PeopleLogging peopleLogging = new PeopleLogging();
            peopleLogging.UniqueID = UniqueId;
            peopleLogging.App = App;
            peopleLogging.Event = Event;
            peopleLogging.Logdate = Logdate;

            try
            {
                _context.Add(peopleLogging);
                _context.SaveChanges();
            }
           catch
            {

            }

        }

        public void AddConfig(string App, String Event, KDGIDENTITYContext _context)
        {
            //KDGIDENTITYContext _context = new KDGIDENTITYContext();

            DateTime Logdate = DateTime.Now;
            Logging logging = new Logging();
            logging.App = App;
            logging.Event = Event;
            logging.Logdate = Logdate;
            try
            {
                _context.Add(logging);
                _context.SaveChanges();
            }
            catch
            {

            }

        }

        public void EditAccount(Kdgaccount oldaccount, Kdgaccount newaccount, string EditBy, KDGIDENTITYContext _context)
        {
            if (oldaccount.Aanvrager != newaccount.Aanvrager)
            {
                Add("KDG" + oldaccount.EmployeeId, "Accounts", "Wijziging Aanvrager: " + oldaccount.Aanvrager + " => " + newaccount.Aanvrager + EditBy, _context);
            }
            if (oldaccount.AccountName != newaccount.AccountName)
            {
                Add("KDG" + oldaccount.EmployeeId, "Accounts", "Wijziging AccountName: " + oldaccount.AccountName + " => " + newaccount.AccountName + EditBy, _context);
            }
            if (oldaccount.AccountType != newaccount.AccountType)
            {
                Add("KDG" + oldaccount.EmployeeId, "Accounts", "Wijziging AccountType: " + oldaccount.AccountType + " => " + newaccount.AccountType + EditBy, _context);
            }
            if (oldaccount.Active != newaccount.Active)
            {
                Add("KDG" + oldaccount.EmployeeId, "Accounts", "Wijziging Active: " + oldaccount.Active + " => " + newaccount.Active + EditBy, _context);
            }
            if (oldaccount.Department != newaccount.Department)
            {
                Add("KDG" + oldaccount.EmployeeId, "Accounts", "Wijziging Department: " + oldaccount.Department + " => " + newaccount.Department + EditBy, _context);
            }
            if (oldaccount.DisplayName != newaccount.DisplayName)
            {
                Add("KDG" + oldaccount.EmployeeId, "Accounts", "Wijziging DisplayName: " + oldaccount.DisplayName + " => " + newaccount.DisplayName + EditBy, _context);
            }
            if (oldaccount.Domain != newaccount.Domain)
            {
                Add("KDG" + oldaccount.EmployeeId, "Accounts", "Wijziging Domain: " + oldaccount.Domain + " => " + newaccount.Domain + EditBy, _context);
            }
            if (oldaccount.Email != newaccount.Email)
            {
                Add("KDG" + oldaccount.EmployeeId, "Accounts", "Wijziging Email: " + oldaccount.Email + " => " + newaccount.Email + EditBy, _context);
            }
            if (oldaccount.EndDate != newaccount.EndDate)
            {
                Add("KDG" + oldaccount.EmployeeId, "Accounts", "Wijziging EndDate: " + oldaccount.EndDate + " => " + newaccount.EndDate + EditBy, _context);
            }
            if (oldaccount.Facilities != newaccount.Facilities)
            {
                Add("KDG" + oldaccount.EmployeeId, "Accounts", "Wijziging Facilities: " + oldaccount.Facilities + " => " + newaccount.Facilities + EditBy, _context);
            }
            if (oldaccount.FirstName != newaccount.FirstName)
            {
                Add("KDG" + oldaccount.EmployeeId, "Accounts", "Wijziging FirstName: " + oldaccount.FirstName + " => " + newaccount.FirstName + EditBy, _context);
            }
            if (oldaccount.HideFromAddressLists != newaccount.HideFromAddressLists)
            {
                Add("KDG" + oldaccount.EmployeeId, "Accounts", "Wijziging HideFromAddressLists: " + oldaccount.HideFromAddressLists + " => " + newaccount.HideFromAddressLists + EditBy, _context);
            }
            if (oldaccount.InitialPassword != newaccount.InitialPassword)
            {
                Add("KDG" + oldaccount.EmployeeId, "Accounts", "Wijziging InitialPassword: " + oldaccount.InitialPassword + " => " + newaccount.InitialPassword + EditBy, _context);
            }
            if (oldaccount.LastName != newaccount.LastName)
            {
                Add("KDG" + oldaccount.EmployeeId, "Accounts", "Wijziging LastName: " + oldaccount.LastName + " => " + newaccount.LastName + EditBy, _context);
            }
            if (oldaccount.MifareSerial != newaccount.MifareSerial)
            {
                Add("KDG" + oldaccount.EmployeeId, "Accounts", "Wijziging MifareSerial: " + oldaccount.MifareSerial + " => " + newaccount.MifareSerial + EditBy, _context);
            }
            if (oldaccount.Office != newaccount.Office)
            {
                Add("KDG" + oldaccount.EmployeeId, "Accounts", "Wijziging Office: " + oldaccount.Office + " => " + newaccount.Office + EditBy, _context);
            }
            if (oldaccount.Opmerking != newaccount.Opmerking)
            {
                Add("KDG" + oldaccount.EmployeeId, "Accounts", "Wijziging Opmerking: ", _context);
            }
            if (oldaccount.PrivateMail != newaccount.PrivateMail)
            {
                Add("KDG" + oldaccount.EmployeeId, "Accounts", "Wijziging PrivateMail: " + oldaccount.PrivateMail + " => " + newaccount.PrivateMail + EditBy, _context);
            }
            if (oldaccount.RightsGroup != newaccount.RightsGroup)
            {
                Add("KDG" + oldaccount.EmployeeId, "Accounts", "Wijziging RightsGroup: " + oldaccount.RightsGroup + " => " + newaccount.RightsGroup + EditBy, _context);
            }
            if (oldaccount.StartDate != newaccount.StartDate)
            {
                Add("KDG" + oldaccount.EmployeeId, "Accounts", "Wijziging StartDate: " + oldaccount.StartDate + " => " + newaccount.StartDate + EditBy, _context);
            }
            if (oldaccount.Telephone != newaccount.Telephone)
            {
                Add("KDG" + oldaccount.EmployeeId, "Accounts", "Wijziging Telephone: " + oldaccount.Telephone + " => " + newaccount.Telephone + EditBy, _context);
            }
            if (oldaccount.Ticketnr != newaccount.Ticketnr)
            {
                Add("KDG" + oldaccount.EmployeeId, "Accounts", "Wijziging Ticketnr: " + oldaccount.Ticketnr + " => " + newaccount.Ticketnr + EditBy, _context);
            }

        }

        public void EditAccountType(AccountType oldaccounttype, AccountType newaccounttype, string EditBy, KDGIDENTITYContext _context)
        {
            if (oldaccounttype.AccountExt != newaccounttype.AccountExt)
            {
                AddConfig("Accounts", "Wijziging Accounttype " + oldaccounttype.Id + " AccountExt " + oldaccounttype.AccountExt + " => " + newaccounttype.AccountExt + EditBy, _context);
            }
            if (oldaccounttype.EmailExt != newaccounttype.EmailExt)
            {
                AddConfig("Accounts", "Wijziging Accounttype " + oldaccounttype.Id + " EmailExt " + oldaccounttype.EmailExt + " => " + newaccounttype.EmailExt + EditBy, _context);
            }
            if (oldaccounttype.CreateAllowed != newaccounttype.CreateAllowed)
            {
                AddConfig("Accounts", "Wijziging Accounttype " + oldaccounttype.Id + " CreateAllowed " + oldaccounttype.CreateAllowed + " => " + newaccounttype.CreateAllowed + EditBy, _context);
            }
            if (oldaccounttype.DeleteAfter != newaccounttype.DeleteAfter)
            {
                AddConfig("Accounts", "Wijziging Accounttype " +oldaccounttype.Id + " DeleteAfter " + oldaccounttype.DeleteAfter + " => " + newaccounttype.DeleteAfter + EditBy, _context);
            }
            if (oldaccounttype.Name != newaccounttype.Name)
            {
                AddConfig("Accounts", "Wijziging Accounttype " + oldaccounttype.Id + " Name " + oldaccounttype.Name + " => " + newaccounttype.Name + EditBy, _context);
            }
        }

        public void EditAccountTypeRole(AccountTypeRole oldrole, AccountTypeRole newrole, string EditBy, KDGIDENTITYContext _context)
        {
            if (oldrole.Role != newrole.Role)
            {
                AddConfig("Accounts", "Wijziging AccountTypeRole " + oldrole.Id + " Field " + oldrole.Role + " => " + newrole.Role + EditBy, _context);
            }
        }

        public void EditPropertie(KdgrightsGroupPropertie oldpropertie, KdgrightsGroupPropertie newpropertie, string EditBy, KDGIDENTITYContext _context)
        {
            if (oldpropertie.Field != newpropertie.Field)
            {
                AddConfig("Accounts", "Wijziging RightsGroupPropertie " + oldpropertie.Id + " Field " + oldpropertie.Field + " => " + newpropertie.Field + EditBy, _context);
            }
            if (oldpropertie.Type != newpropertie.Type)
            {
                AddConfig("Accounts", "Wijziging RightsGroupPropertie " + oldpropertie.Id + " Type " + oldpropertie.Type + " => " + newpropertie.Type + EditBy, _context);
            }
            if (oldpropertie.Value != newpropertie.Value)
            {
                AddConfig("Accounts", "Wijziging RightsGroupPropertie " + oldpropertie.Id + " Value " + oldpropertie.Value + " => " + newpropertie.Value + EditBy, _context);
            }
        }

        public void EditRightsGroupGroup(Kdggroup oldgroup, Kdggroup newgroup, string EditBy, KDGIDENTITYContext _context)
        {
            if (oldgroup.AccountName != newgroup.AccountName)
            {
                AddConfig("Accounts", "Wijziging RightsGroupPropertie " + oldgroup.Id + " Field " + oldgroup.AccountName + " => " + newgroup.AccountName + EditBy, _context);
            }
            if (oldgroup.DisplayName != newgroup.DisplayName)
            {
                AddConfig("Accounts", "Wijziging RightsGroupPropertie " + oldgroup.Id + " Type " + oldgroup.DisplayName + " => " + newgroup.DisplayName + EditBy, _context);
            }
            if (oldgroup.Domain != newgroup.Domain)
            {
                AddConfig("Accounts", "Wijziging RightsGroupPropertie " + oldgroup.Id + " Value " + oldgroup.Domain + " => " + newgroup.Domain + EditBy, _context);
            }
        }
    }
}
