using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKDelivery.Courier.Model
{
    public class UserSession
    {

        public bool IsSessionActive { get; set; }
        public string DeniedScopes { get; set; }
        public string GrantedScopes { get; set; }
        public DateTime TokenExpires { get; set; }
        public string AccessToken { get; set; }
        public double UserId { get; set; } //Unique for app
    }
}
