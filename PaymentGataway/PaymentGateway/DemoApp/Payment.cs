using System;
using System.Collections.Generic;
using System.Text;

namespace DemoApp
{
    public class Payment
    {
        public int OrderId { get; set; }
        public string CardNumber { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string CVV { get; set; }
        public string CardholderName { get; set; }
        public uint AmountKop { get; set; }
    }
}
