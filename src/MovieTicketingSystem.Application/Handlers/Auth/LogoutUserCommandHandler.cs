using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using MovieTicketingSystem.Application.Commands.Auth;
using MovieTicketingSystem.Domain.Contracts.Repository;

namespace MovieTicketingSystem.Application.Handlers.Auth
{
    public class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand,bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<LogoutUserCommand> _validator;
        private readonly IMapper _mapper;

        public LogoutUserCommandHandler(IUserRepository userRepository, IValidator<LogoutUserCommand> validator, IMapper mapper)
        {
            _userRepository = userRepository;
            _validator = validator;
            _mapper = mapper;
        }
        public async Task<bool> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
        {
            if (request == null || request.Email == null)
                return false;

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            return await _userRepository.LogoutUser(request.Email);
        }
    }
}
