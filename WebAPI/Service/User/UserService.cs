using Microsoft.EntityFrameworkCore;
using WebAPI.Infrastructure;
using WebAPI.Infrastructure.Tabels;
using WebAPI.Model;
using WebAPI.Utilities;
using WebAPI.Utilities.Jwt;

namespace WebAPI.Service.User
{
    public class UserService 
    {
        public UserService(IJwtAuthManager jwtAuthManager, IConfig config )
        {
            _jwtAuthManager = jwtAuthManager;
           
            m_config = config;
        }
        readonly IConfig m_config;
        readonly IJwtAuthManager _jwtAuthManager;
        



       
      
        
    }
}
