using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MovieTicketingSystem.Application.Commands.Auth;
using MovieTicketingSystem.Domain.Contracts.Repository;
using AutoMapper;
using FluentValidation;
using MovieTicketingSystem.Application.Validators.Auth;
using MovieTicketingSystem.Domain.DTOs;

namespace MovieTicketingSystem.Application.Handlers.Auth
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, TokenResponse?>
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<LoginUserCommand> _validator;
        private readonly IMapper _mapper;
        
        public LoginUserCommandHandler(IUserRepository userRepository,IValidator<LoginUserCommand> validator, IMapper mapper)
        {
            _userRepository = userRepository;
            _validator = validator;
            _mapper = mapper;
        }
        
        public async Task<TokenResponse?> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            if (request == null || request.Email == null || request.Password == null)
                return null;

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            return await _userRepository.LoginUser(request.Email, request.Password);
        }
    }
} 