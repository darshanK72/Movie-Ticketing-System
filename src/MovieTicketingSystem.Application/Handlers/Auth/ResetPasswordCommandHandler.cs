using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MovieTicketingSystem.Application.Commands.Auth;
using MovieTicketingSystem.Domain.Contracts.Repository;
using AutoMapper;
using FluentValidation;

namespace MovieTicketingSystem.Application.Handlers.Auth
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<ResetPasswordCommand> _validator;
        private readonly IMapper _mapper;
        
        public ResetPasswordCommandHandler(IUserRepository userRepository,IValidator<ResetPasswordCommand> validator, IMapper mapper)
        {
            _userRepository = userRepository;
            _validator = validator;
            _mapper = mapper;
        }
        
        public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            if (request == null || request.Email == null || request.Token == null || request.NewPassword == null)
                return false;

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            return await _userRepository.ResetPassword(request.Email, request.Token, request.NewPassword);
        }
    }
} 