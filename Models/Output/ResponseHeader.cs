using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Models.Output
{
    public class ResponseHeader
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public object Content { get; set; }

        public int Page { get; set; }
        public int PerPage { get; set; }
        public int TotalElement { get; set; }

        public ResponseHeader(object Content)
        {
            this.Content = Content;
        }

        public ResponseHeader(string status, string Message)
        {
            this.Status = status;
            this.Message = Message;
        }

        public ResponseHeader(string Status, string Message, object Content)
        {
            this.Status = Status;
            this.Message = Message;
            this.Content = Content;
        }

        public ResponseHeader(string Status, string Message, object Content, int Page, int PerPage, int TotalElement)
        {
            this.Status = Status;
            this.Message = Message;
            this.Content = Content;
            this.Page = Page;
            this.PerPage = PerPage;
            this.TotalElement = TotalElement;
        }

        public ResponseHeader() { }



    }
}
