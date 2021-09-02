using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Projeto.CrossCutting.Messages.Contracts;
using Projeto.Data.Contracts;
using Projeto.Data.Entities;
using Projeto.Presentation.Areas.AreaRestrita.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;

namespace Projeto.Presentation.Areas.AreaRestrita.Controllers
{
    [Authorize]
    [Area("AreaRestrita")]
    public class SalaController : Controller
    {
        private readonly ISalaRepository salaRepository;
        private readonly IPerfilPermissaoRepository perfilpermissaoRepository;
        private readonly ISqlServerException sqlServerException;

        public SalaController(ISalaRepository salaRepository, IPerfilPermissaoRepository perfilpermissaoRepository, ISqlServerException sqlServerException)
        {
            this.salaRepository = salaRepository;
            this.perfilpermissaoRepository = perfilpermissaoRepository;
            this.sqlServerException = sqlServerException;
        }

        public IActionResult Cadastro([FromServices] ILocalidadeRepository localidadeRepository)
        {
            if (perfilpermissaoRepository.PermissaoAutorizada(User.FindFirst(ClaimTypes.Role).Value, 14))
            {
                var model = new SalaCadastroModel();
                model.Localidades = new List<SelectListItem>();

                try
                {
                    model.Localidades = ListarLocalidade(localidadeRepository.ConsultarAtivo());
                }
                catch (Exception ex)
                {
                    TempData["MensagemErro"] = $"Erro: {ex.Message}";
                }

                return View(model);
            }
            else
                return RedirectToAction("Consulta");
        }

        [HttpPost]
        public IActionResult Cadastro(SalaCadastroModel model,
                                      [FromServices] ILocalidadeRepository localidadeRepository)
        {
            try
            {
                model.Localidades = new List<SelectListItem>();
                model.Localidades = ListarLocalidade(localidadeRepository.ConsultarAtivo());

                if (ModelState.IsValid)
                {
                    try
                    {
                        if (salaRepository.ObterIdPorCriterio(model.Nome, (int)model.IdLocalidade) != null)
                            throw new Exception("Sala já cadastrada.");

                        var sala = new Sala();

                        sala.Nome = model.Nome;
                        sala.IdLocalidade = (int)model.IdLocalidade;
                        sala.Desativar = model.Desativar ? "x" : null;

                        salaRepository.Inserir(sala);

                        TempData["MensagemSucesso"] = $"{model.Nome}, cadastrado com sucesso.";

                        return RedirectToAction("Cadastro");

                    }
                    catch (Exception ex)
                    {
                        TempData["MensagemErro"] = $"Erro: {ex.Message}";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro: {ex.Message}";
            }

            return View(model);
        }

        public IActionResult Consulta(SalaConsultaModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Salas = salaRepository.ConsultarTodos();
                    model.PerfisPermissoes = perfilpermissaoRepository.Consultar(User.FindFirst(ClaimTypes.Role).Value, 13);
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = $"Erro ao consultar area(s): {e.Message}.";
                }
            }

            return View(model);
        }

        public JsonResult Exclusao(SalaConsultaModel model)
        {
            try
            {
                var sala = salaRepository.ObterPorId(model.IdSala);

                if (sala != null)
                    salaRepository.Excluir(sala);

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

        public IActionResult Edicao(int id,
                                    [FromServices] ILocalidadeRepository localidadeRepository)
        {

            var model = new SalaEdicaoModel();
            model.Localidades = new List<SelectListItem>();

            try
            {
                if (perfilpermissaoRepository.PermissaoAutorizada(User.FindFirst(ClaimTypes.Role).Value, 15))
                {
                    model.Localidades = ListarLocalidade(localidadeRepository.ConsultarAtivo());

                    var area = salaRepository.ObterPorId(Convert.ToInt32(id));

                    model.IdSala = area.IdSala;
                    model.Nome = area.Nome;
                    model.Desativar = string.IsNullOrEmpty(area.Desativar) ? false : true;
                    model.IdLocalidade = area.IdLocalidade;
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
        public IActionResult Edicao(SalaEdicaoModel model,
                                    [FromServices] ILocalidadeRepository localidadeRepository)
        {
            try
            {
                model.Localidades = new List<SelectListItem>();
                model.Localidades = ListarLocalidade(localidadeRepository.ConsultarAtivo());

                if (ModelState.IsValid)
                {
                    try
                    {
                        var registro = salaRepository.ObterPorId(Convert.ToInt32(model.IdSala));

                        if (registro != null)
                        {
                            var idsala = salaRepository.ObterIdPorCriterio(model.Nome, (int)model.IdLocalidade);

                            if (idsala == null || idsala == registro.IdSala)
                            {
                                registro.Nome = model.Nome;
                                registro.Desativar = model.Desativar == true ? "x" : null;
                                registro.IdLocalidade = (int)model.IdLocalidade;

                                salaRepository.Alterar(registro);
                            }
                            else
                                throw new Exception("Sala já cadastrada.");
                        }
                        else
                            throw new Exception("Sala não encontrada.");

                        TempData["MensagemSucesso"] = $"{model.Nome}, atualizado com sucesso.";

                    }
                    catch (Exception ex)
                    {
                        TempData["MensagemErro"] = $"Erro: {ex.Message}.";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro: {ex.Message}.";
            }

            return View(model);
        }

        private List<SelectListItem> ListarLocalidade(List<Localidade> lista)
        {
            var registros = new List<SelectListItem>();

            //registros.Add(new SelectListItem() { Text = "", Value = null });
            foreach (var item in lista)
            {
                var opcao = new SelectListItem();
                opcao.Value = item.IdLocalidade.ToString();
                opcao.Text = item.Nome;

                registros.Add(opcao);
            }

            return registros.OrderBy(x => x.Text).ToList();
        }
    }
}