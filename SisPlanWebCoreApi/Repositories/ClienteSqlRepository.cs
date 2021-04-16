using SisPlanWebCoreApi.Entities;
using SisPlanWebCoreApi.Helpers;
using SisPlanWebCoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace SisPlanWebCoreApi.Repositories
{
    public class ClienteSqlRepository : IClienteRepository
    {
        private readonly ClienteDbContext _clienteDbContext;
        public ClienteSqlRepository(ClienteDbContext clienteDbContext)
        {
            _clienteDbContext = clienteDbContext;
        }
        public void Add(ClienteEntity item)
        {
            _clienteDbContext.ClienteItens.Add(item);
        }

        public int Count()
        {
            return _clienteDbContext.ClienteItens.Count();
        }

        public void Delete(Guid id)
        {
            ClienteEntity foodItem = GetSingle(id);
            _clienteDbContext.ClienteItens.Remove(foodItem);
        }

        public IQueryable<ClienteEntity> GetAll(QueryParameters queryParameters)
        {
            IQueryable<ClienteEntity> _allItems = _clienteDbContext.ClienteItens.OrderBy(queryParameters.OrderBy,
           queryParameters.IsDescending());

             if (queryParameters.HasQuery()){
                 _allItems = _allItems
                    .Where(x => x.Nome.ToLower().Contains(queryParameters.Nome.ToLower()));
             }

        
            return _allItems
               .Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);
        }

       

        public ClienteEntity GetSingle(Guid id)
        {
            return _clienteDbContext.ClienteItens.FirstOrDefault(x => x.Id == id);
        }

        public bool Save()
        {
            return (_clienteDbContext.SaveChanges() >= 0);
        }

        public ClienteEntity Update(Guid id, ClienteEntity item)
        {
            _clienteDbContext.ClienteItens.Update(item);
            return item;
        }
    }
}
