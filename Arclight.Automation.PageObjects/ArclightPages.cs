using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arclight.Automation.PageObjects
{
    public static class ArclightPages
    {
        public static BoxArtListPage BoxArtListPage
        {
            get
            {
                var boxArtListPage = new BoxArtListPage();
                return boxArtListPage;
            }
        }

        public static CreateBoxArtPage CreateBoxArtPage
        {
            get
            {
                var createBoxArtPage = new CreateBoxArtPage();
                return createBoxArtPage;
            }
        }

        public static DashboardPage DashboardPage
        {
            get
            {
                var dashboardPage = new DashboardPage();
                return dashboardPage;
            }
        }

        public static LoginPage LoginPage
        {
            get
            {
                var loginPage = new LoginPage();
                return loginPage;
            }
        }


    }
}
