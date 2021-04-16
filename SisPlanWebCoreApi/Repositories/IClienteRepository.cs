using System;
using System.Collections.Generic;
using System.Linq;
using SisPlanWebCoreApi.Entities;
using SisPlanWebCoreApi.Models;

namespace SisPlanWebCoreApi.Repositories
{
    public interface IClienteRepository
    {
        ClienteEntity GetSingle(System.Guid id);
        void Add(ClienteEntity item);
        void Delete(Guid id);
        ClienteEntity Update(Guid id, ClienteEntity item);
        IQueryable<ClienteEntity> GetAll(QueryParameters queryParameters);

        int Count();

        bool Save();
    }
}
