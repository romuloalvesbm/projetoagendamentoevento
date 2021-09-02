using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projeto.CrossCutting.Messages.Contracts;
using Projeto.Data.Contracts;
using Projeto.Data.Entities;
using Projeto.Presentation.Areas.AreaRestrita.Models;

namespace Projeto.Presentation.Areas.AreaRestrita.Controllers
{
    [Authorize]
    [Area("AreaRestrita")]
    [Authorize(Policy = "PermissaoLocalidade")]
    public class LocalidadeController : Controller
    {
        private readonly ILocalidadeRepository localidadeRepository;
        private readonly IPerfilPermissaoRepository perfilpermissaoRepository;
        private readonly ISqlServerException sqlServerException;

        public LocalidadeController(ILocalidadeRepository localidadeRepository, IPerfilPermissaoRepository perfilpermissaoRepository, ISqlServerException sqlServerException)
        {
            this.localidadeRepository = localidadeRepository;
            this.perfilpermissaoRepository = perfilpermissaoRepository;
            this.sqlServerException = sqlServerException;
        }

        public IActionResult Cadastro()
        {
            if (perfilpermissaoRepository.PermissaoAutorizada(User.FindFirst(ClaimTypes.Role).Value, 10))
                return View();
            else
                return RedirectToAction("Consulta");
        }

        [HttpPost]
        public IActionResult Cadastro(LocalidadeCadastroModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (localidadeRepository.ObterIdPorNome(model.Nome) != null)
                        throw new Exception("Localidade já cadastrada.");

                    var localidade = new Localidade();

                    localidade.Nome = model.Nome;
                    localidade.Desativar = model.Desativar ? "x" : null;

                    localidadeRepository.Inserir(localidade);

                    TempData["MensagemSucesso"] = $"{model.Nome}, cadastrado com sucesso.";
                    ModelState.Clear();
                }
                catch (Exception ex)
                {
                    TempData["MensagemErro"] = $"Erro: { (ex.InnerException?.Message ?? ex.Message).Replace("'", @"\'").Replace("\r\n", @"\r\n") }";
                }
            }

            return View();
        }

        public IActionResult Consulta(LocalidadeConsultaModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Localidades = localidadeRepository.Consultar();
                    model.PerfisPermissoes = perfilpermissaoRepository.Consultar(User.FindFirst(ClaimTypes.Role).Value, 9);
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = $"Erro ao consultar localidade(s): {e.Message}.";
                }
            }

            return View(model);
        }

        public JsonResult Exclusao(LocalidadeConsultaModel model,
            [FromServices] ISqlServerException sqlserverException)
        {
            try
            {
                var evento = localidadeRepository.ObterPorId(model.IdLocalidade);

                if (evento != null)
                    localidadeRepository.Excluir(evento);

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
            var model = new LocalidadeEdicaoModel();

            try
            {
                if (perfilpermissaoRepository.PermissaoAutorizada(User.FindFirst(ClaimTypes.Role).Value, 11))
                {
                    var localidade = localidadeRepository.ObterPorId(Convert.ToInt32(id));

                    model.IdLocalidade = localidade.IdLocalidade;
                    model.Nome = localidade.Nome;
                    model.Desativar = string.IsNullOrEmpty(localidade.Desativar) ? false : true;
                }
                else
                    return RedirectToAction("Consulta");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = (ex.InnerException?.Message ?? ex.Message).Replace("'", @"\'").Replace("\r\n", @"\r\n");
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Edicao(LocalidadeEdicaoModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var registro = localidadeRepository.ObterPorId(Convert.ToInt32(model.IdLocalidade));

                    if (registro != null)
                    {
                        var idarea = localidadeRepository.ObterIdPorNome(model.Nome);

                        if (idarea == null || idarea == registro.IdLocalidade)
                        {
                            registro.Nome = model.Nome;
                            registro.Desativar = model.Desativar == true ? "x" : null;

                            localidadeRepository.Alterar(registro);
                        }
                        else
                            throw new Exception("Localidade já cadastrada.");
                    }
                    else
                        throw new Exception("Localidade não encontrada.");

                    TempData["MensagemSucesso"] = $"{model.Nome}, atualizado com sucesso.";

                }
                catch (Exception ex)
                {
                    TempData["MensagemErro"] = $"Erro: { (ex.InnerException?.Message ?? ex.Message).Replace("'", @"\'").Replace("\r\n", @"\r\n") }";
                }
            }

            return View(model);
        }
    }
}