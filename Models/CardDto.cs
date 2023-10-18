using System;

namespace smartlocker.software.api.Models
{
    public class CardDto
    {
        public string Name { get; set; }
        public string Number { get; set; }
        public int ExpirationMonth { get; set; }
        public int ExpirationYear { get; set; }
        public string SecurityCode { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public long Amount { get; set; }

    }
}