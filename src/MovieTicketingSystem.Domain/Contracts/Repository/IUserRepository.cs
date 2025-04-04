using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MovieTicketingSystem.Domain.Entities;

namespace MovieTicketingSystem.Domain.Contracts.Repository
{
    public interface IUserRepository
    {
        Task<bool> RegisterUser(User user);
    }
}
