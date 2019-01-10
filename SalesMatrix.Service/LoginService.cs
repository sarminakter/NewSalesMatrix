using SalesMatrix.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesMatrix.Service
{
    public class LoginService
    {
        private _SalesMatrixDB db = new _SalesMatrixDB();
        CommonService common = new CommonService();
        public bool ValidLogin(User user)
        {
            User gotUser = db.Users.SingleOrDefault(u => u.UserName == user.UserName);
            string encoPass = common.DoEncode(user.Password);
            if (gotUser.UserName == user.UserName && gotUser.Password == encoPass)
            {
                return true;
            }
            else
                return false;
        }

    }
}
