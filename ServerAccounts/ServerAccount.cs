using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cat.ServerAccounts
{
    public class ServerAccount
    {
        /***************************Misc**************************************/
        public ulong ID { get; set; }
        public string ServerName { get; set; }
        public string Prefix { get; set; }

        /***************************Welcome msg strings**************************/
        public bool WelcomeMsg { get; set; }

        /***************************Welcome msg strings**************************/
        public string WelcomeMsgDescriptionString { get; set; }
        public string WelcomeMsgTitleString { get; set; }
        public string WelcomeMsgImageUrlString { get; set; }
        public string WelcomeMsgTumbnailImageUrlString { get; set; }
        public string WelcomeMsgFooterString { get; set; }
        public string WelcomeMsgInlineField1String { get; set; }
        public string WelcomeMsgInlineField2String { get; set; }
        public string WelcomeMsgInlineField3String { get; set; }
        public string WelcomeMsgInlineField4String { get; set; }
        public string WelcomeMsgField1String { get; set; }
        public string WelcomeMsgField2String { get; set; }
        public string WelcomeMsgField3String { get; set; }
        public string WelcomeMsgField4String { get; set; }
        public string WelcomeMsgField5String { get; set; }
        public string WelcomeMsgField6String { get; set; }
        public string WelcomeMsgInlineField1TitleString { get; set; }
        public string WelcomeMsgInlineField2TitleString { get; set; }
        public string WelcomeMsgInlineField3TitleString { get; set; }
        public string WelcomeMsgInlineField4TitleString { get; set; }
        public string WelcomeMsgField1TitleString { get; set; }
        public string WelcomeMsgField2TitleString { get; set; }
        public string WelcomeMsgField3TitleString { get; set; }
        public string WelcomeMsgField4TitleString { get; set; }
        public string WelcomeMsgField5TitleString { get; set; }
        public string WelcomeMsgField6TitleString { get; set; }

        /*************************Join Dm Bools********************************/
        public bool JoinDmBool { get; set; }

        /***************************Join Message Strings**************************/

        public string JoinDmDescriptionString { get; set; }
        public string JoinDmTitleString { get; set; }
        public string JoinDmImageUrlString { get; set; }
        public string JoinDmTumbnailImageUrlString { get; set; }
        public string JoinDmFooterString { get; set; }
        public string JoinDmInlineField1String { get; set; }
        public string JoinDmInlineField2String { get; set; }
        public string JoinDmInlineField3String { get; set; }
        public string JoinDmInlineField4String { get; set; }
        public string JoinDmField1String { get; set; }
        public string JoinDmField2String { get; set; }
        public string JoinDmField3String { get; set; }
        public string JoinDmField4String { get; set; }
        public string JoinDmField5String { get; set; }
        public string JoinDmField6String { get; set; }
        public string JoinDmInlineField1TitleString { get; set; }
        public string JoinDmInlineField2TitleString { get; set; }
        public string JoinDmInlineField3TitleString { get; set; }
        public string JoinDmInlineField4TitleString { get; set; }
        public string JoinDmField1TitleString { get; set; }
        public string JoinDmField2TitleString { get; set; }
        public string JoinDmField3TitleString { get; set; }
        public string JoinDmField4TitleString { get; set; }
        public string JoinDmField5TitleString { get; set; }
        public string JoinDmField6TitleString { get; set; }

        /***************************Assign role to new user**************************/

        public bool NewUserJoinedRoleBool { get; set; }
        public string NewUserJoinedRoleString { get; set; }

        /***************************Level up setting**************************/

        public bool LevelupMsgBool { get; set; }
        public ulong LevelupMsgChannel { get; set; }
        public bool WeeklyTopBool { get; set; }
        public bool MonthlyTopBool { get; set; }
        public bool YearlyTopBool { get; set; }
    }
}
