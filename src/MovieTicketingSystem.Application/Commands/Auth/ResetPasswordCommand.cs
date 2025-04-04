﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace MovieTicketingSystem.Application.Commands.Auth
{
    public class ResetPasswordCommand : IRequest<bool>
    {
        public string? Token { get; set; }
        public string? Email { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}
