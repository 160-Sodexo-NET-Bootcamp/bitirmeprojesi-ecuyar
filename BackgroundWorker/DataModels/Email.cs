using System;

namespace BackgroundWorker.DataModels
{
    public class Email
    {
        public int Id { get; set; }
        public string EmailTo { get; set; }
        public string DynamicData { get; set; }
        public bool IsSent { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
