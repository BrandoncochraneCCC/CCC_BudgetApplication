using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.Controllers
{
    public class CompareObjectsController<t> : ObjectInstanceController
    {
        private t obj;
        private t other;

        public CompareObjectsController() { }
        public CompareObjectsController(t obj, t other)
        {
            this.obj = obj;
            this.other = other;
        }

        public void setObjects(t obj, t other)
        {
            setObj(obj);
            setOther(other);
        }

        public t getObj() { return obj; }
        public void setObj(t obj) { this.obj = obj; }
        public t getOther() { return other; }
        public void setOther(t other) { this.other = other; }

        // GET: CompareObjects
        public bool compareObject(t obj, t other)
        {
            setObjects(obj, other);

            return compareObject();
        }

        public bool compareObject(t other)
        {
            setOther(other);

            return compareObject();
        }

        public bool compareObject()
        {
            string s1 = obj.ToString();
            string s2 = other.ToString();

            return compareString(s1, s2);
        }

        public bool compareString(string s1, string s2)
        {
            var value1 = s1.ToLower();
            value1 = value1.Replace(" ", string.Empty);

            var value2 = s2.ToLower();
            value2 = value2.Replace(" ", string.Empty);

            return value1.Equals(value2);
        }
    }
}
