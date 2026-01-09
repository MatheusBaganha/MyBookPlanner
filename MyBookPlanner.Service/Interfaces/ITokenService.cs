using System;
using System.Collections.Generic;
using System.Text;
using MyBookPlanner.Domain.Models;

namespace MyBookPlanner.Service.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(User user);
    }
}
