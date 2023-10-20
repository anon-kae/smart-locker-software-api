using System;
using System.IO;
using System.Linq;
using System.Timers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SmartLocker.Software.Backend.Entities;

namespace SmartLocker.Software.Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Timer timer = new Timer(TimeSpan.FromSeconds(10).TotalMilliseconds)
            //{
            //    AutoReset = true
            //};
            //timer.Elapsed += new ElapsedEventHandler(CancelBookings);
            //timer.Start();

            //Timer timer2 = new Timer(TimeSpan.FromSeconds(10).TotalMilliseconds)
            //{
            //    AutoReset = true
            //};
            //timer2.Elapsed += new ElapsedEventHandler(CancelBookingsForFInish);
            //timer2.Start();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel();
                    webBuilder.UseUrls("http://*:5000");
                    webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                    webBuilder.UseStartup<Startup>();
                });
        public static void CancelBookings(object sender, ElapsedEventArgs e)
        {
            using var context = new SmartLockerContext();
            var result = context.Booking.Where(c => c.Status == "WP").ToList();
            foreach (Booking booking in result)
            {
                TimeSpan sp = DateTime.Now - booking.CreateDate;
                if (sp.TotalMinutes > 60)
                {
                    booking.Status = "C";
                    booking.UpdateDate = DateTime.Now;
                    context.SaveChanges();
                }
            }
        }

        //public static void CancelBookingsForFInish(object sender, ElapsedEventArgs e)
        //{
        //    using var context = new SmartLockerContext();
        //    var result = context.Booking.Where(c => c.Status == "WF" && c.EndDate != null).ToList();
        //    foreach (Booking booking in result)
        //    {
        //        TimeSpan sp = (TimeSpan)(DateTime.Now - booking.EndDate);
        //        if (sp.TotalMinutes > 15)
        //        {
        //            booking.Status = "P";
        //            booking.UpdateDate = DateTime.Now;
        //            booking.EndDate = null;
        //            context.SaveChanges();
        //        }
        //    }

        //}
    }
}
