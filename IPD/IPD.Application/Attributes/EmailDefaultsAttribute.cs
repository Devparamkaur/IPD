namespace IPD.Application.Attributes
{
    public class EmailDefaultsAttribute : Attribute
    {
        protected string sender;

        #region Properties

        /// <summary>
        /// Holds the EmailSender for a value in an enum.
        /// </summary>
        public string Sender
        {
            get
            {
                if (!string.IsNullOrEmpty(sender))
                {
                    return sender;
                }

                return "notifications@ipd.com";

            }
        }

        // <summary>
        /// Holds the EmailSubject for a value in an enum.
        /// </summary>
        public string Subject { get; protected set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor used to init a EmailSubject Attribute
        /// </summary>
        /// <param name="value"></param>
        public EmailDefaultsAttribute(string sender, string subject)
        {
            this.sender = sender;
            Subject = subject;
        }

        #endregion
    }
}
