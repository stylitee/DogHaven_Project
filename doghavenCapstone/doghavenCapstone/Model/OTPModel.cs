using System;
using System.Collections.Generic;
using System.Text;

namespace doghavenCapstone.Model
{
    public class OTPModel
    {
        public string sender { get; set; }
        public string route { get; set; }
        public string country { get; set; }
        public List<Sms> sms { get; set; } = new List<Sms> { };
    }

    public class Sms
    {
        public string message { get; set; }
        public List<string> to { get; set; }
    }

    public class OTPResponse
    {
        public string message { get; set; }
        public string type { get; set; }
    }
}
