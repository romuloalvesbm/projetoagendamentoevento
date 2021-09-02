using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Newtonsoft.Json;
using Projeto.Data.Contracts;
using Projeto.Data.Dtos;
using Projeto.Data.Entities;
using Projeto.Presentation.Areas.AreaRestrita.Models;

namespace Projeto.Presentation.Areas.AreaRestrita.Controllers
{
    [Authorize]
    [Area("AreaRestrita")]
    public class AgendamentoTurmaController : Controller
    {
        private readonly IAgendamentoTurmaRepository agendamentoturmaRepository;
        private readonly IAgendamentoColaboradorRepository agendamentoColaboradorRepository;

        public AgendamentoTurmaController(IAgendamentoTurmaRepository agendamentoturmaRepository, IAgendamentoColaboradorRepository agendamentoColaboradorRepository)
        {
            this.agendamentoturmaRepository = agendamentoturmaRepository;
            this.agendamentoColaboradorRepository = agendamentoColaboradorRepository;
        }

        public IActionResult Cadastro()
        {
            var model = new AgendamentoTurmaCadastroModel();
            model.lstStatus = ListarStatus();

            return View(model);
        }

        [HttpPost]
        public IActionResult Cadastro(AgendamentoTurmaCadastroModel model,
                                      [FromServices] IEventoRepository eventoRepository,
                                      [FromServices] IAreaRepository areaRepository,
                                      [FromServices] ISalaRepository salaRepository)
        {
            model.lstStatus = ListarStatus();

            if (ModelState.IsValid)
            {
                try
                {
                    if (!eventoRepository.EventoAtivo((int)model.IdEvento))
                        throw new Exception("Evento não encontrado.");

                    if (!areaRepository.AreaAtiva((int)model.IdArea))
                        throw new Exception("Área não encontrada.");

                    if (!salaRepository.SalaAtiva((int)model.IdSala))
                        throw new Exception("Sala não encontrada.");

                    //DateTime DataEvento = Projeto.CrossCutting.Validations.Date.Validaticao(model.DataEvento.ToString());

                    if (agendamentoturmaRepository.ConfirmarEvento((int)model.IdSala, (DateTime)model.DataEvento,
                                                                   (TimeSpan)model.HoraInicial.Value.Add(new TimeSpan(0, 1, 0)),
                                                                   (TimeSpan)model.HoraTermino.Value.Subtract(new TimeSpan(0, 1, 0))))
                        throw new Exception("Horário indisponível.");

                    var agendamentoturma = new AgendamentoTurma();

                    agendamentoturma.IdEvento = (int)model.IdEvento;
                    agendamentoturma.IdArea = (int)model.IdArea;
                    agendamentoturma.IdSala = (int)model.IdSala;
                    agendamentoturma.Turma = model.Turma;
                    agendamentoturma.Data = (DateTime)model.DataEvento;
                    agendamentoturma.Hora_Inicio = (TimeSpan)model.HoraInicial;
                    agendamentoturma.Hora_Fim = (TimeSpan)model.HoraTermino;
                    agendamentoturma.Data_Limite = (DateTime)model.DataLimiteInscrição;
                    agendamentoturma.Status = (model.Status == "Aberto" && model.DataLimiteInscrição > DateTime.Today) ? "Finalizado" : model.Status;
                    agendamentoturma.Max_Participante = (int)model.Parcipantes;

                    agendamentoturmaRepository.Inserir(agendamentoturma);

                    TempData["MensagemSucesso"] = $"Evento: {model.NomeEvento}, cadastrado com sucesso.";

                    return RedirectToAction("Cadastro");

                }
                catch (Exception ex)
                {
                    TempData["MensagemErro"] = $"Erro: {ex.Message}";
                }
            }

            return View(model);
        }

        public IActionResult Consulta()
        {
            return View();
        }

        public IActionResult Edicao(string id)
        {

            var model = new AgendamentoTurmaEdicaoModel();
            model.lstStatus = ListarStatus();

            try
            {
                var agendamentoturma = agendamentoturmaRepository.ObterPor_Id(Convert.ToInt32(id));

                model.IdAgendamentoTurma = agendamentoturma.IdAgeTurma;
                model.NomeEvento = agendamentoturma.Evento.Nome;
                model.DescricaoEvento = agendamentoturma.Evento.Descricao;
                model.NomeArea = agendamentoturma.Area.Nome;
                model.NomeSala = agendamentoturma.Sala.Nome + " - " + agendamentoturma.Sala.Localidade.Nome;
                model.Turma = agendamentoturma.Turma;
                model.DataEvento = agendamentoturma.Data.ToString("dd/MM/yyyy");
                model.HoraInicial = agendamentoturma.Hora_Inicio.ToString("hh':'mm");
                model.HoraTermino = agendamentoturma.Hora_Fim.ToString("hh':'mm");
                model.DataLimiteInscricao = agendamentoturma.Data_Limite.ToString("dd/MM/yyyy");
                model.Status = agendamentoturma.Status;
                model.Parcipantes = agendamentoturma.Max_Participante;
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = e.Message;
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Edicao(AgendamentoTurmaEdicaoModel model)
        {

            model.lstStatus = ListarStatus();

            if (ModelState.IsValid)
            {
                try
                {
                    var registro = agendamentoturmaRepository.ObterPor_Id(Convert.ToInt32(model.IdAgendamentoTurma));

                    if (registro != null)
                    {
                        switch (model.Status)
                        {
                            case "Finalizado":

                                registro.Max_Participante = registro.AgendamentosColaboradores.Count();
                                break;

                            case "Aberto":

                                if (model.Parcipantes < registro.AgendamentosColaboradores.Count())
                                    throw new Exception("Qtde participantes inferior à qtde de inscrição.");
                                else
                                {
                                    if (model.Parcipantes > registro.AgendamentosColaboradores.Count())
                                    {
                                        if (registro.Data_Limite < DateTime.Today || model.Parcipantes == registro.AgendamentosColaboradores.Count())
                                            model.Status = "Finalizado";
                                    }
                                }

                                registro.Max_Participante = (int)model.Parcipantes;
                                break;

                        }

                        registro.Status = model.Status;
                        agendamentoturmaRepository.Alterar(registro);

                        TempData["MensagemSucesso"] = $"{model.NomeEvento}, atualizado com sucesso.";

                        return RedirectToAction("Edicao", new { id = model.IdAgendamentoTurma });
                    }
                    else
                        throw new Exception("Agendamento turma não encontrado.");

                }
                catch (Exception ex)
                {
                    TempData["MensagemErro"] = $"Erro: {ex.Message}.";
                }
            }

            return View(model);

        }

        public JsonResult Exclusao(AgendamentoTurmaConsultaModel model)
        {
            try
            {
                var agendamentoturma = agendamentoturmaRepository.ObterPorId(model.IdAgendamentoTurma);

                agendamentoturmaRepository.Excluir(agendamentoturma);

                return Json(new { success = true, responseText = "Excluido com sucesso!!" });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message });
            }
        }

        public JsonResult RemoverColaborador(AgendamentoTurmaConsultaModel model)
        {
            try
            {
                var agendamentoColaborador = agendamentoColaboradorRepository.ObterPorId(model.IdAgeCol);

                agendamentoColaboradorRepository.Excluir(agendamentoColaborador);

                return Json(new { success = true, responseText = "Colaborador excluido com sucesso!!" });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message });
            }
        }

        public JsonResult ListarEvento(string Nome,
                                       [FromServices] IEventoRepository eventoRepository)
        {
            try
            {
                var lista = new EventoConsultaModel();
                lista.Eventos = eventoRepository.ConsultarAtivoPorNome(Nome);

                return Json(JsonConvert.SerializeObject(lista.Eventos));

            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { success = false, message = ex.Message });
            }
        }

        public JsonResult ListarArea(string Nome,
                                     [FromServices] IAreaRepository areaRepository)
        {
            try
            {
                var lista = new AreaConsultaModel();
                lista.Areas = areaRepository.ConsultarAtivoPorNome(Nome);

                return Json(JsonConvert.SerializeObject(lista.Areas));

            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { success = false, message = ex.Message });
            }
        }

        public JsonResult ListarSala(string Nome,
                                     [FromServices] ISalaRepository salaRepository)
        {
            try
            {
                var lista = new SalaConsultaModel();
                lista.Salas = salaRepository.ConsultarAtivoPorNome(Nome);

                return Json(JsonConvert.SerializeObject(lista.Salas, Formatting.None,
                          new JsonSerializerSettings()
                          {
                              ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                          }));
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { success = false, message = ex.Message });
            }
        }

        public JsonResult GetAgendamentoTurma()
        {
            try
            {
                var requestFormData = Request.Form;
                var data = ProcessarDadosForm(requestFormData);

                var response = new 
                {
                    Draw = requestFormData["draw"].ToString(),
                    RecordsFiltered = data.Item2,
                    RecordsTotal = data.Item3,
                    Data = data.Item1,
                };

                return Json(response);

            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { success = false, message = ex.Message });
            }

        }

        public JsonResult GetColaborador(string IdAgendamentoTurma)         
        {
            try
            {
                var requestFormData = Request.Form;
                var data = ProcessarDadosFormColaborador(requestFormData, int.Parse(IdAgendamentoTurma));

                var response = new 
                {
                    Draw = requestFormData["draw"].ToString(),
                    RecordsFiltered = data.Item2,
                    RecordsTotal = data.Item3,
                    Data = data.Item1,
                };

                return Json(response);

            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { success = false, message = ex.Message });
            }

        }

        private List<SelectListItem> ListarStatus()
        {
            return new List<SelectListItem>
            {
                new SelectListItem{ Text="", Value = null },
                new SelectListItem{ Text="Aberto", Value = "Aberto" },
                new SelectListItem{ Text="Finalizado", Value = "Finalizado" },
                new SelectListItem{ Text="Cancelado", Value = "Cancelado" },
            };
        }

        private Tuple<List<AgendamentoTurmaDTO>, int, int> ProcessarDadosForm(IFormCollection requestFormData)
        {
            var search = requestFormData["search[value]"].ToString();
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());

            Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };

            if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
            {
                var columnIndex = requestFormData["order[0][column]"].ToString();
                var sortDirection = requestFormData["order[0][dir]"].ToString();

                tempOrder = new[] { "" };

                if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
                {
                    string columName = requestFormData[$"columns[{columnIndex}][data]"].ToString();
                    string columDate = requestFormData[$"columns[{2}][search][value]"].ToString();
                    string columTotalColaborador = requestFormData[$"columns[{5}][search][value]"].ToString();


                    var filter = new AgendamentoTurmaDTO
                    {
                        Evento = requestFormData[$"columns[{0}][search][value]"].ToString(),
                        Sala = requestFormData[$"columns[{1}][search][value]"].ToString(),
                        TotalColaborador = string.IsNullOrEmpty(columTotalColaborador) ? (int?)null : Convert.ToInt32(columTotalColaborador),
                        Data = string.IsNullOrEmpty(columDate) ? (DateTime?)null : Projeto.CrossCutting.Validations.Date.Validaticao(columDate),
                        Status = requestFormData[$"columns[{6}][search][value]"].ToString()
                    };

                    switch (columName)
                    {
                        case "evento":
                            columName = "evento.nome";
                            break;
                        case "sala":
                            columName = "sala.nome";
                            break;
                        case "totalColaborador":
                            columName = "AgendamentosColaboradores.Count";
                            break;
                    }

                    if (pageSize > 0)
                        return agendamentoturmaRepository.ConsultarTodos(filter, skip, pageSize, columName, sortDirection);
                    else
                        return null;
                }
            }
            return null;
        }

        private Tuple<List<AgendamentoColaboradorDto>, int, int> ProcessarDadosFormColaborador(IFormCollection requestFormData, int idageturma)
        {
            var search = requestFormData["search[value]"].ToString();
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());

            Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };

            if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
            {
                var columnIndex = requestFormData["order[0][column]"].ToString();
                var sortDirection = requestFormData["order[0][dir]"].ToString();

                tempOrder = new[] { "" };

                if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
                {
                    string columName = requestFormData[$"columns[{columnIndex}][data]"].ToString();
                    string columDate = requestFormData[$"columns[{2}][search][value]"].ToString();
                    string columTotalColaborador = requestFormData[$"columns[{5}][search][value]"].ToString();

                    string chapa = requestFormData[$"columns[{0}][search][value]"].ToString();
                    string Nome = requestFormData[$"columns[{1}][search][value]"].ToString();

                    switch (columName)
                    {
                        case "chapa":
                            columName = "colaborador.chapa";
                            break;
                        case "nome":
                            columName = "colaborador.nome";
                            break;
                    }

                    if (pageSize > 0)
                        return agendamentoColaboradorRepository.ListarPorTurma(idageturma, chapa, Nome, skip, pageSize, columName, sortDirection);
                    else
                        return null;
                }
            }
            return null;
        }

        private PropertyInfo getProperty(string name)
        {
            var properties = typeof(AgendamentoTurma).GetProperties();
            PropertyInfo prop = null;
            foreach (var item in properties)
            {
                if (item.Name.ToLower().Equals(name.ToLower()))
                {
                    prop = item;
                    break;
                }
            }
            return prop;
        }
    }
}
