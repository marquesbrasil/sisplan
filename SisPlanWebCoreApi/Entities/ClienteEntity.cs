using System;

namespace SisPlanWebCoreApi.Entities
{
    public class ClienteEntity
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
    }
}
