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
using MovieTicketingSystem.Domain.DTOs.Auth;

namespace MovieTicketingSystem.Application.Handlers.Auth
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenResponse?>
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<RefreshTokenCommand> _validator;
        private readonly IMapper _mapper;

        public RefreshTokenCommandHandler(IUserRepository userRepository, IValidator<RefreshTokenCommand> validator, IMapper mapper)
        {
            _userRepository = userRepository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<TokenResponse?> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            if (request == null || request.RefreshToken == null || request.Email == null)
                return null;

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            return await _userRepository.RefreshToken(request.RefreshToken,request.Email);
        }
    }
}
