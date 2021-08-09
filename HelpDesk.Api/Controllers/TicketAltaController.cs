using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using HelpDesk.Api.Entities.Dtos;
using HelpDesk.Api.Entities.Models;
using HelpDesk.Api.Model.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace HelpDesk.Api.Controllers
{
    [EnableCors("_MyPoliciy")]
    [Route("api/[controller]")]
    [ApiController]
    public class TicketAltaController : ControllerBase
    {
        private readonly IAltaTicket repository;
        private readonly IMapper mapper;
        private readonly LinkGenerator generator;
        private readonly IEmailSender emailSender;
        private IWebHostEnvironment hostingEnvironment;
        public TicketAltaController(IAltaTicket repository, 
                                    IMapper mapper, IWebHostEnvironment environment,
                                    IEmailSender emailSender, LinkGenerator generator)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.generator = generator;
            this.emailSender = emailSender;
            this.hostingEnvironment = environment;
        }


        [HttpPost]
        public async Task<ActionResult<TicketAltaDto>> PostAltaTicket([FromForm] TicketAltaFileDto ticketDto)
        {
            try
            {
                var ticketAlta = JsonConvert.DeserializeObject<Ticket>(ticketDto.TicketAlta);
                var ticket = mapper.Map<Ticket>(ticketAlta);
                ticket.FechaAlta = DateTime.Now;
                ticket.EstadoTicket = 2;
                ticket.AsignadoA = "Soporte Telintec";
                ticket.FechaAsignado = DateTime.Now;
                repository.Alta(ticket);

                string fullPath = string.Empty;

                if (await repository.GuardarCambiosAsync())
                {
                    var file = ticketDto.File;

                    if (file != null)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var folderName = Path.Combine("Resources", "Images");
                        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                        fullPath = Path.Combine(pathToSave, $"ticket_{ticket.TicketId}_{file.FileName}");
                        using (Stream fileStream = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                            ticket.ImagenUrl = fullPath;
                            await repository.GuardarCambiosAsync();
                        }
                    }

                    /*await emailSender
                    .SendEmailAsync($"Nuevo Ticket Creado #{ticket.TicketId}", await obtenerHtmlTicket(ticket), fullPath)
                    .ConfigureAwait(false);*/
                    ticketDto.TicketId = ticket.TicketId;
                    return Ok(ticketDto);
                }
                else
                {
                    ticketDto.Errores = "Ocurrio un error al querer agregar el ticket";
                    return BadRequest(ticketDto);
                }
            }
            catch (Exception err)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, err.Message);
            }


        }

        [HttpPost]
        [Route("ActualizarTicket")]
        public async Task<ActionResult<TicketAltaDto>> PostActualizarTicket([FromForm] TicketUpdateFileDto ticketDto)
        {
            try
            {
                var ticketUpdate = JsonConvert.DeserializeObject<TicketUpdateDto>(ticketDto.TicketUpdate);
                var ticketOld = await repository.ObtenerTicketPorId(ticketUpdate.Id);
           
                if (ticketOld == null)
                {
                    return BadRequest($"No se encontro el ticket numero : {ticketUpdate.Id} en la base de datos");
                }
                if (ticketDto.File != null)
                {
                    var file = ticketDto.File;
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var folderName = Path.Combine("Resources", "Images");
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    var fullPath = Path.Combine(pathToSave, $"ticket_{ticketUpdate.Id}_{file.FileName}");
                    using (Stream fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        ticketOld.ImagenUrl = fullPath;
                        await file.CopyToAsync(fileStream);
                    }
                }
                else
                {
                    ticketOld.ImagenUrl = String.Empty;        
                }

                ticketOld.ComentarioAlta = ticketUpdate.ComentarioAlta;
                ticketOld.NombreReporta = ticketUpdate.NombreReporta;
                ticketOld.PlantaId = ticketUpdate.PlantaId;
                ticketOld.TipoReporte = ticketUpdate.TipoReporte;

                repository.Update(ticketOld);

                await repository.GuardarCambiosAsync();
                {
                    return Ok(ticketDto);
                }
                //else
                //{
                //    ticketDto.Errores = "Ocurrio un error al querer agregar el ticket";
                //    return BadRequest(ticketDto);
                //}
            }
            catch (Exception err)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, err.Message);
            }
        }


        [HttpPost]
        [Route("AsignarResponsable")]
        public async Task<ActionResult<TicketResponsableDto>> PostAsignarResponsableTicket([FromBody] TicketResponsableDto ticketDto)
        {
            try
            {
                var ticketOld = await repository.ObtenerTicketPorId(ticketDto.Id);
                if (ticketOld == null)
                {
                    return BadRequest($"No se encontro el ticket numero : {ticketDto.Id} en la base de datos");
                }
                ticketOld.FechaAsignado = DateTime.Now;
                ticketOld.EstadoTicket = 2;
                ticketOld.AsignadoA = ticketDto.AsignadoA;

                repository.Update(ticketOld);

                if (await repository.GuardarCambiosAsync())
                {
                    return Ok(ticketDto);
                }
                else
                {
                    ticketDto.Errores = "Ocurrio un error al querer agregar el ticket";
                    return BadRequest(ticketDto);
                }
            }
            catch (Exception err)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, err.Message);
            }
        }


        [HttpPost]
        [Route("CerrarTicket")]
        public async Task<ActionResult<TicketResponsableDto>> PostCerrarTicket([FromBody] TicketCerrarDto ticketDto)
        {
            try
            {
                var ticketOld = await repository.ObtenerTicketPorId(ticketDto.Id);
                if (ticketOld == null)
                {
                    return BadRequest($"No se encontro el ticket numero : {ticketDto.Id} en la base de datos");
                }

                if (ticketDto.CerrarTicket)
                {
                    ticketOld.FechaTermino = DateTime.Now;
                    ticketOld.EstadoTicket = 3;
                }

                repository.Update(ticketOld);

                if (await repository.GuardarCambiosAsync())
                {
                    return Ok(ticketDto);
                }
                else
                {
                    ticketDto.Errores = "Ocurrio un error al querer agregar el ticket";
                    return BadRequest(ticketDto);
                }
            }
            catch (Exception err)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, err.Message);
            }
        }


        [HttpDelete]
        [Route("{ticketId}")]
        public async Task<ActionResult<TicketResponsableDto>> DeleteTicket(int ticketId)
        {
            try
            {
                var ticketOld = await repository.ObtenerTicketPorId(ticketId);
                if (ticketOld == null)
                {
                    return BadRequest($"No se encontro el ticket numero : {ticketId} en la base de datos");
                }
                repository.Eliminar(ticketOld);
                await repository.GuardarCambiosAsync();
                return Ok();

            }
            catch (Exception err)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, err.Message);
            }
        }


        [HttpGet]
        public async Task<ActionResult<Ticket[]>> GetTickets()
        {
            try
            {
                var results = await repository.ObtenerTickets();
                if (results == null) return NotFound();
                return Ok(results);
            }
            catch (Exception err)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, err.Message);
            }
        }


        [HttpGet]
        [Route("Plantas")]
        public async Task<ActionResult<Planta[]>> GetPlantas()
        {
            try
            {
                var results = await repository.ObtenerPlantas();
                if (results == null) return NotFound();
                return Ok(results);
            }
            catch (Exception err)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, err.Message);
            }
        }

        [HttpGet]
        [Route("{ticketId}")]
        public async Task<ActionResult<Ticket>> GetTicketPorId(int ticketId)
        {
            try
            {
                var result = await repository.ObtenerTicketPorId(ticketId);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception err)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, err.Message);
            }
        }
        
        private async Task<string> obtenerHtmlTicket(Ticket ticket)
        {

            var results = await repository.ObtenerPlantas();

            var planta = (from p in results
                          where p.PlantaId == ticket.PlantaId
                          select p).First().PlantaNombre;

            return $" <table> <tr>  <td> Ticket #: </td> <td>{ ticket.TicketId} </td>   <tr>" +
                $"   <tr>   <td> Creado Por: </td> <td> {ticket.NombreReporta}</td> </tr>  " +
                $"   <tr>   <td>Planta: </td> <td> {planta}</td> </tr>  " +
                $"  <tr>   <td> Fecha : </td> <td> {ticket.FechaAlta.ToString("dd/MM/yyyy HH:mm:ss")}</td> </tr> " +
                $"  <tr>   <td> Tipo : </td> <td> { obtenerTipoTicket(ticket.TipoReporte)}</td> </tr> " +
                $"  <tr>   <td> Comentario Alta : </td> <td> {  ticket.ComentarioAlta}</td> </tr> " +
                $"  </table>";
        }

        [AllowAnonymous]
        [Route("getTicketImage/{ticketId}")]        
        [HttpGet]
        public async Task<ActionResult<TicketAltaDto>> getProductImage(int ticketId)
        {
            var result = await repository.ObtenerTicketPorId(ticketId);
            var ticket = mapper.Map<TicketAltaDto>(result);
            if (!String.IsNullOrEmpty(result.ImagenUrl))
            {
                char _char = (char)92;
                byte[] imageByteData = System.IO.File.ReadAllBytes(result.ImagenUrl);
                ticket.Imagen = result.ImagenUrl.Split(_char)[result.ImagenUrl.Split(_char).Length - 1];
                ticket.ImagenData = Convert.ToBase64String(imageByteData);
            }
            return Ok(ticket);          
        }

        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }


        private string obtenerTipoTicket(int tipoTicket)
        {   
            switch(tipoTicket)
            {
                case 1: return "Error en Sistema";
                case 2: return "Falla en Equipo";
                case 3: return "Mantenimiento";
                default: return "otro";
            }

        }
    }
}
