using System;
using System.Data.SqlClient;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projeto.CrossCutting.Messages.Contracts;
using Projeto.Data.Contracts;
using Projeto.Data.Entities;
using Projeto.Presentation.Areas.AreaRestrita.Models;

namespace Projeto.Presentation.Areas.AreaRestrita.Controllers
{
    [Area("AreaRestrita")]
    [Authorize(Policy = "PermissaoEvento")]
    public class EventoController : Controller
    {
        private readonly IEventoRepository eventoRepository;
        private readonly IPerfilPermissaoRepository perfilpermissaoRepository;
        private readonly ISqlServerException sqlServerException;

        public EventoController(IEventoRepository eventoRepository, ISqlServerException sqlServerException, IPerfilPermissaoRepository perfilpermissaoRepository)
        {
            this.eventoRepository = eventoRepository;
            this.sqlServerException = sqlServerException;
            this.perfilpermissaoRepository = perfilpermissaoRepository;
        }

        public IActionResult Cadastro()
        {
            if (perfilpermissaoRepository.PermissaoAutorizada(User.FindFirst(ClaimTypes.Role).Value, 6))
                return View();
            else
                return RedirectToAction("Consulta");
        }

        [HttpPost]
        public IActionResult Cadastro(EventoCadastroModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (eventoRepository.ObterIdPorNome(model.Nome) != null)
                        throw new Exception("Evento já cadastrado");

                    var evento = new Evento();

                    evento.Nome = model.Nome;
                    evento.Descricao = model.Descricao;
                    evento.Desativar = model.Desativar ? "x" : null;

                    eventoRepository.Inserir(evento);

                    TempData["MensagemSucesso"] = $"{model.Nome}, cadastrado com sucesso.";
                    ModelState.Clear();
                }
                catch (Exception ex)
                {
                    TempData["MensagemErro"] = $"Erro: {ex.Message}.";
                }
            }

            return View();
        }

        public IActionResult Consulta(EventoConsultaModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.PerfisPermissoes = perfilpermissaoRepository.Consultar(User.FindFirst(ClaimTypes.Role).Value, 5);
                    model.Eventos = eventoRepository.Consultar();
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = $"Erro ao consultar evento(s): {e.Message}.";
                }
            }

            return View(model);
        }

        public JsonResult Exclusao(EventoConsultaModel model)
        {
            try
            {
                var evento = eventoRepository.ObterPorId(model.IdEvento);

                if (evento != null)
                    eventoRepository.Excluir(evento);

                return Json(new { success = true, responseText = "Excluido com sucesso!!" });

            }
            catch (Exception ex)
            {
                int? ErrorCode = null;

                if (ex.GetBaseException().GetType() == typeof(SqlException))
                {
                    ErrorCode = ((SqlException)ex.InnerException).Number;
                }

                return Json(new
                {
                    success = false,
                    message = (ErrorCode != null ? sqlServerException.AlterarDescricao((int)ErrorCode) :
                                                                                  ex.InnerException?.Message ?? ex.Message)
                                                                                  .Replace("'", @"\'").Replace("\r\n", @"\r\n")
                });
            }
        }

        public IActionResult Edicao(int id)
        {
            var model = new EventoEdicaoModel();

            try
            {
                if (perfilpermissaoRepository.PermissaoAutorizada(User.FindFirst(ClaimTypes.Role).Value, 7))
                {

                    var evento = eventoRepository.ObterPorId(Convert.ToInt32(id));

                    model.IdEvento = evento.IdEvento;
                    model.Nome = evento.Nome;
                    model.Descricao = evento.Descricao;
                    model.Desativar = string.IsNullOrEmpty(evento.Desativar) ? false : true;
                }
                else
                    return RedirectToAction("Consulta");
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = e.Message;
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Edicao(EventoEdicaoModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var registro = eventoRepository.ObterPorId(Convert.ToInt32(model.IdEvento));

                    if (registro != null)
                    {
                        var idevento = eventoRepository.ObterIdPorNome(model.Nome);

                        if (idevento == null || idevento == registro.IdEvento)
                        {
                            registro.Nome = model.Nome;
                            registro.Descricao = model.Descricao;
                            registro.Desativar = model.Desativar == true ? "x" : null;

                            eventoRepository.Alterar(registro);
                        }
                        else
                            throw new Exception("Evento já cadastrado.");
                    }
                    else
                        throw new Exception("Evento não encontrado.");

                    TempData["MensagemSucesso"] = $"{model.Nome}, atualizado com sucesso.";

                }
                catch (Exception ex)
                {
                    TempData["MensagemErro"] = $"Erro: {ex.Message}.";
                }
            }

            return View(model);
        }

    }
}