using notomyk.DAL;
using notomyk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.Infrastructure
{
    public class BlackListValidator
    {
        private NTMContext _db = new NTMContext();
        private string _urlName;

        public BlackListValidator(string url)
        {
            _urlName = url;
        }

        public bool CheckIfBLocked()
        {
            if (_db.BlackList.Any(b => b.url == _urlName))
            {
                return false;
            }
            return true;
        }
    }
}