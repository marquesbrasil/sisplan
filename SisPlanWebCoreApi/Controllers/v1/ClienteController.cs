using System;
using System.Linq;
using AutoMapper;
using SisPlanWebCoreApi.Dtos;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SisPlanWebCoreApi.Repositories;
using System.Collections.Generic;
using SisPlanWebCoreApi.Entities;
using SisPlanWebCoreApi.Models;
using SisPlanWebCoreApi.Helpers;
using System.Text.Json;

namespace SampleWebApiAspNetCore.v1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IUrlHelper _urlHelper;
        private readonly IMapper _mapper;

        public ClienteController(
            IUrlHelper urlHelper,
            IClienteRepository clienteRepository,
            IMapper mapper)
        {
            _clienteRepository = clienteRepository;
            _mapper = mapper;
            _urlHelper = urlHelper;
        }

        [HttpGet(Name = nameof(GetAllclientes))]
        public ActionResult GetAllclientes(ApiVersion version, [FromQuery] QueryParameters queryParameters)
        {
           
            List<ClienteEntity> ClienteItens = _clienteRepository.GetAll(queryParameters).ToList();

            var allItemCount = _clienteRepository.Count();

            var paginationMetadata = new
            {
                totalCount = allItemCount,
                pageSize = queryParameters.PageCount,
                currentPage = queryParameters.Page,
                totalPages = queryParameters.GetTotalPages(allItemCount)
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var links = CreateLinksForCollection(queryParameters, allItemCount, version);

            var toReturn = ClienteItens.Select(x => ExpandSingleclienteItem(x, version));

            return Ok(new
            {
                value = toReturn,
                links = links
            });
        }

        [HttpGet]
        [Route("{id:int}", Name = nameof(GetSinglecliente))]
        public ActionResult GetSinglecliente(ApiVersion version, Guid id)
        {
            ClienteEntity clienteItem = _clienteRepository.GetSingle(id);

            if (clienteItem == null)
            {
                return NotFound();
            }

            return Ok(ExpandSingleclienteItem(clienteItem, version));
        }

        [HttpPost(Name = nameof(Addcliente))]
        public ActionResult<ClienteDto> Addcliente(ApiVersion version, [FromBody] ClienteCreateDto clienteCreateDto)
        {
            if (clienteCreateDto == null)
            {
                return BadRequest();
            }

            ClienteEntity toAdd = _mapper.Map<ClienteEntity>(clienteCreateDto);

            _clienteRepository.Add(toAdd);

            if (!_clienteRepository.Save())
            {
                throw new Exception("Creating a clienteitem failed on save.");
            }

            ClienteEntity newclienteItem = _clienteRepository.GetSingle(toAdd.Id);

            return CreatedAtRoute(nameof(GetSinglecliente),
                new { version = version.ToString(), id = newclienteItem.Id },
                _mapper.Map<ClienteDto>(newclienteItem));
        }

        [HttpPatch("{id:int}", Name = nameof(PartiallyUpdatecliente))]
        public ActionResult<ClienteDto> PartiallyUpdatecliente(Guid id, [FromBody] JsonPatchDocument<ClienteUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            ClienteEntity existingEntity = _clienteRepository.GetSingle(id);

            if (existingEntity == null)
            {
                return NotFound();
            }

            ClienteUpdateDto ClienteUpdateDto = _mapper.Map<ClienteUpdateDto>(existingEntity);
            patchDoc.ApplyTo(ClienteUpdateDto);

            TryValidateModel(ClienteUpdateDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(ClienteUpdateDto, existingEntity);
            ClienteEntity updated = _clienteRepository.Update(id, existingEntity);

            if (!_clienteRepository.Save())
            {
                throw new Exception("Updating a clienteitem failed on save.");
            }

            return Ok(_mapper.Map<ClienteDto>(updated));
        }

        [HttpDelete]
        [Route("{id:int}", Name = nameof(Removecliente))]
        public ActionResult Removecliente(Guid id)
        {
            ClienteEntity clienteItem = _clienteRepository.GetSingle(id);

            if (clienteItem == null)
            {
                return NotFound();
            }

            _clienteRepository.Delete(id);

            if (!_clienteRepository.Save())
            {
                throw new Exception("Deleting a clienteitem failed on save.");
            }

            return NoContent();
        }

        [HttpPut]
        [Route("{id:int}", Name = nameof(Updatecliente))]
        public ActionResult<ClienteDto> Updatecliente(Guid id, [FromBody] ClienteUpdateDto ClienteUpdateDto)
        {
            if (ClienteUpdateDto == null)
            {
                return BadRequest();
            }

            var existingclienteItem = _clienteRepository.GetSingle(id);

            if (existingclienteItem == null)
            {
                return NotFound();
            }

            _mapper.Map(ClienteUpdateDto, existingclienteItem);

            _clienteRepository.Update(id, existingclienteItem);

            if (!_clienteRepository.Save())
            {
                throw new Exception("Updating a clienteitem failed on save.");
            }

            return Ok(_mapper.Map<ClienteDto>(existingclienteItem));
        }

        private List<LinkDto> CreateLinksForCollection(QueryParameters queryParameters, int totalCount, ApiVersion version)
        {
            var links = new List<LinkDto>();

            // self 
            links.Add(new LinkDto(_urlHelper.Link(nameof(GetAllclientes), new
            {
                pagecount = queryParameters.PageCount,
                page = queryParameters.Page,
                orderby = queryParameters.OrderBy
            }), "self", "GET"));

            links.Add(new LinkDto(_urlHelper.Link(nameof(GetAllclientes), new
            {
                pagecount = queryParameters.PageCount,
                page = 1,
                orderby = queryParameters.OrderBy
            }), "first", "GET"));

            links.Add(new LinkDto(_urlHelper.Link(nameof(GetAllclientes), new
            {
                pagecount = queryParameters.PageCount,
                page = queryParameters.GetTotalPages(totalCount),
                orderby = queryParameters.OrderBy
            }), "last", "GET"));

            if (queryParameters.HasNext(totalCount))
            {
                links.Add(new LinkDto(_urlHelper.Link(nameof(GetAllclientes), new
                {
                    pagecount = queryParameters.PageCount,
                    page = queryParameters.Page + 1,
                    orderby = queryParameters.OrderBy
                }), "next", "GET"));
            }

            if (queryParameters.HasPrevious())
            {
                links.Add(new LinkDto(_urlHelper.Link(nameof(GetAllclientes), new
                {
                    pagecount = queryParameters.PageCount,
                    page = queryParameters.Page - 1,
                    orderby = queryParameters.OrderBy
                }), "previous", "GET"));
            }

            var posturl = _urlHelper.Link(nameof(Addcliente), new { version = version.ToString() });

            links.Add(
               new LinkDto(posturl,
               "create_cliente",
               "POST"));

            return links;
        }

        private dynamic ExpandSingleclienteItem(ClienteEntity clienteItem, ApiVersion version)
        {
            var links = GetLinks(clienteItem.Id, version);
            ClienteDto item = _mapper.Map<ClienteDto>(clienteItem);

            var resourceToReturn = item.ToDynamic() as IDictionary<string, object>;
            resourceToReturn.Add("links", links);

            return resourceToReturn;
        }

        private IEnumerable<LinkDto> GetLinks(Guid id, ApiVersion version)
        {
            var links = new List<LinkDto>();

            var getLink = _urlHelper.Link(nameof(GetSinglecliente), new { version = version.ToString(), id = id });

            links.Add(
              new LinkDto(getLink, "self", "GET"));

            var deleteLink = _urlHelper.Link(nameof(Removecliente), new { version = version.ToString(), id = id });

            links.Add(
              new LinkDto(deleteLink,
              "delete_cliente",
              "DELETE"));

            var createLink = _urlHelper.Link(nameof(Addcliente), new { version = version.ToString() });

            links.Add(
              new LinkDto(createLink,
              "create_cliente",
              "POST"));

            var updateLink = _urlHelper.Link(nameof(Updatecliente), new { version = version.ToString(), id = id });

            links.Add(
               new LinkDto(updateLink,
               "update_cliente",
               "PUT"));

            return links;
        }
    }
}
