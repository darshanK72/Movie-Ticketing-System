using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MovieTicketingSystem.Application.Commands.Auth;
using MovieTicketingSystem.Domain.Contracts.Repository;
using AutoMapper;
using FluentValidation;

namespace MovieTicketingSystem.Application.Handlers.Auth
{
    public class ForgetPasswordCommandHandler : IRequestHandler<ForgetPasswordCommand, string?>
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<ForgetPasswordCommand> _validator;
        private readonly IMapper _mapper;
        
        public ForgetPasswordCommandHandler(IUserRepository userRepository,IValidator<ForgetPasswordCommand> validator ,IMapper mapper)
        {
            _userRepository = userRepository;
            _validator = validator;
            _mapper = mapper;
        }
        
        public async Task<string?> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            if (request == null || request.Email == null)
                return null;

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            return await _userRepository.ForgotPassword(request.Email);
        }
    }
} 