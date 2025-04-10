﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MovieTicketingSystem.Application.Commands.Auth;
using MovieTicketingSystem.Domain.Contracts.Repository;
using MovieTicketingSystem.Domain.Entities;
using AutoMapper;
using FluentValidation;

namespace MovieTicketingSystem.Application.Handlers.Auth
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<RegisterUserCommand> _validator;
        private readonly IMapper _mapper;
        
        public RegisterUserCommandHandler(IUserRepository userRepository, IValidator<RegisterUserCommand> validator, IMapper mapper) 
        {
            _userRepository = userRepository;
            _validator = validator;
            _mapper = mapper;
        }
        
        public async Task<bool> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return false;

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = _mapper.Map<User>(request);
            return await _userRepository.RegisterUser(user);
        }
    }
}
