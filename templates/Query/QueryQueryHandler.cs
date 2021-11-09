using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

namespace Query
{
    public class QueryQueryHandler : IRequestHandler<QueryQuery, QueryQueryResult>
    {
        private readonly IConfigurationProvider _mapping;

        public QueryQueryHandler(IConfigurationProvider mapping)
        {
            _mapping = mapping;
        }

        public Task<QueryQueryResult> Handle(QueryQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}