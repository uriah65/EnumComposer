using EnumComposer;
using IEnumComposer;
using System;
using System.Collections.Generic;

namespace TestComposer
{
    public class Fake_DbReader : IEnumDbReader
    {
        public void ReadEnumeration(EnumModel model)
        {
            if (string.IsNullOrWhiteSpace(model.SqlSelect))
            {
                throw new ArgumentException("SQL is blank.");
            }

            Dictionary<int, string> source = null;
            string sql = model.SqlSelect;
            if (sql.Contains("T_Simple"))
            {
                source = T_Simple;
            }
            else if (sql.Contains("T_AwayType"))
            {
                source = T_AwayType;
            }
            else if (sql.Contains("T_MeetingType"))
            {
                source = T_MeetingType;
            }
            else if (sql.Contains("T_AwaySystem"))
            {
                source = T_AwaySystem;
            }
            else if (sql.Contains("T_Location"))
            {
                source = T_Location;
            }

            foreach (var entry in source)
            {
                model.FillFromDb(entry.Key, entry.Value);
            }
        }

        private Dictionary<int, string> T_Simple = new Dictionary<int, string>
        {
            [5] = "in Db 1",
            [6] = "in Db 2",
            [7] = "in Db New"
        };

        private Dictionary<int, string> T_AwayType = new Dictionary<int, string>
        {
            [2] = "Mediation Face To Face",
            [3] = "Mediation Phone",
            [4] = "Administrative",
            [5] = "Vacation",
            [6] = "Writing Day",
            [22] = "Training",
            [23] = "Seminar",
            [24] = "Not Available",
            [39] = "Sick Days",
            [42] = "Holiday",
            [43] = "External Event",
            [41] = "Old Holiday",
            [46] = "Religious Holiday",
        };

        private Dictionary<int, string> T_MeetingType = new Dictionary<int, string>
        {
            [7] = "Pre Hearing",
            [8] = "Hearing",
            [5] = "SD",
        };

        private Dictionary<int, string> T_AwaySystem = new Dictionary<int, string>
        {
            [98000] = "Default",
            [98001] = "Mediation",
            [98002] = "Arbitration",
            [98003] = "Appeals",
        };

        private Dictionary<int, string> T_Location = new Dictionary<int, string>
        {
            [129] = "Written Submissions",
            [100] = "To Be Scheduled Arb",
        };
    }
}