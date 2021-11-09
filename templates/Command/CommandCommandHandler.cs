using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

namespace Command
{
    public class CommandCommandHandler : AsyncRequestHandler<CommandCommand>
    {
        private readonly IMapper _mapper;

        public CommandCommandHandler(IMapper mapper)
        {
            _mapper = mapper;
        }

        protected override Task Handle(CommandCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}