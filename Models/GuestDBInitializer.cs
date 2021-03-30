using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Kutse_App.Models
{
    public class GuestDBInitializer: CreateDatabaseIfNotExists<GuestContext> //DropCreateDatabaseAlways<GuestContext>
    {
        protected override void Seed(GuestContext db)
        {
            db.Guests.Add(
                new Guest
                {
                    Id=1,
                    Name="Marina Oleinik",
                    Email="marina.oleinik@gmail.com",
                    Phone="+56909474",
                    WillAttend=true

                });
            db.SaveChanges();
            base.Seed(db);
        }
    }
}