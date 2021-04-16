using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SisPlanWebCoreApi.Dtos
{
    public class ClienteCreateDto
    {
        [Required]
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }

    }
}
