using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.Controllers
{
    public class Disposable<T> where T : Controller
    {
        T db;

        public Disposable(T db)
        {
            this.db = db;
        }       


        protected virtual void Dispose(bool disposing)
        {

            if (disposing)
            {
                // dispose managed resources
                db.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}