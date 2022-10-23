using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

namespace Command
{
    public class MyCommandCommandHandler : AsyncRequestHandler<MyCommandCommand>
    {
        private readonly IMapper _mapper;

        public MyCommandCommandHandler(IMapper mapper)
        {
            _mapper = mapper;
        }

        protected override Task Handle(MyCommandCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}