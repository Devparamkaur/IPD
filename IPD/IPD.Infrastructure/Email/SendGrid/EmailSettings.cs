﻿namespace IPD.Infrastructure.Emails.SendGrid
{
    public class EmailSettings
    {
        public string ApiKey { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
    }
}