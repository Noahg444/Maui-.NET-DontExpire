using System;
using SQLite;

namespace DontExpireFinal
{

    [Table("Food")]
    public class Food
    {
        public Food()
        {
        }
        public enum ExpireDuration { Expired, ExpiringSoon, DoesNotExpire };
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime UseByDate { get; set; }
        public string Location { get; set; }
        public string DurationTime { get; set; }
        public ExpireDuration Duration { get; set; }
        public string Serving { get; set; }
        public string Category { get; set; }
        public bool IsChecked { get; set; }
        public string ImageFile { get; set; }

        public string selectDuration
        {
            get
            {
                if (System.DateTime.Now >= UseByDate) return "Expired";
                if (System.DateTime.Now < UseByDate) return "ExpiringSoon";
                return "DoesNotExpire";
            }
            set
            {
                if (System.DateTime.Now >= UseByDate) DurationTime = "Expired";
                else if (System.DateTime.Now < UseByDate) DurationTime = "ExpiringSoon";
                else DurationTime = "DoesNotExpire";
            }
        }
    }
}

