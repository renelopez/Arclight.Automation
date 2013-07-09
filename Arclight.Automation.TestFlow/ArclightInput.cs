using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arclight.Automation.Selenium;

namespace Arclight.Automation.TestFlow
{
    public class ArclightInput
    {
        // List that will hold the values that will be used for the test. 
        public static Dictionary<string, string> TestValues;

        // Username to enter into the portal.
        internal static string ArclightUsername
        {
            get { return Browser.GetConfigValue("ARCLIGHT_USERNAME"); }
        }

        // Password to enter into the portal.
        internal static string ArclightPassword
        {
            get { return Browser.GetConfigValue("ARCLIGHT_PASSWORD"); }
        }

        internal static string Filename
        {
            get
            {
                // If the value is not assigned into a key of the list, then it should add it.
                if (!TestValues.ContainsKey("Filename"))
                {
                    TestValues.Add("Filename",Browser.GetConfigValue("FILE_NAME"));
                }

                // If the value was assigned,just return from the list,with its key.
                return TestValues["Filename"];
            }
        }

        internal static string OVP
        {
            get
            {
                // If the value is not assigned into a key of the list, then it should add it.
                if (!TestValues.ContainsKey("OVP"))
                {
                    TestValues.Add("OVP", Browser.GetConfigValue("OVP"));
                }

                // If the value was assigned,just return from the list,with its key.
                return TestValues["OVP"];
            }
        }

    }
}
