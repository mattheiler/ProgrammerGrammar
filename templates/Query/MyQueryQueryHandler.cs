using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

namespace Query
{
    public class MyQueryQueryHandler : IRequestHandler<MyQueryQuery, MyQueryQueryResult>
    {
        private readonly IConfigurationProvider _mapping;

        public MyQueryQueryHandler(IConfigurationProvider mapping)
        {
            _mapping = mapping;
        }

        public Task<MyQueryQueryResult> Handle(MyQueryQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}