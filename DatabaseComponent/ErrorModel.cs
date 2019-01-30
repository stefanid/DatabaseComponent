using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseComponent
{
    public class ErrorModel
    {
        public DateTime DateTime { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorHeader { get; set; }

        public string ErrorText { get; set; }

        public string ApplicationName { get; set; }

        public string ApplicationVersion { get; set; }

        public string ComponentVersion { get; set; }

        public string ComponentName { get; set; }
    }
}
