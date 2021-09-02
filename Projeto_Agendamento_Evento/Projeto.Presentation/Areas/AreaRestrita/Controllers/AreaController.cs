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
    [Area("AreaRestrita")]
    [Authorize(Policy = "PermissaoArea")]    
    public class AreaController : Controller
    {
        private readonly IAreaRepository areaRepository;
        private readonly IPerfilPermissaoRepository perfilpermissaoRepository;
        private readonly ISqlServerException sqlServerException;

        public AreaController(IAreaRepository areaRepository, IPerfilPermissaoRepository perfilpermissaoRepository, ISqlServerException sqlServerException)
        {
            this.areaRepository = areaRepository;
            this.perfilpermissaoRepository = perfilpermissaoRepository;
            this.sqlServerException = sqlServerException;
        }

        public IActionResult Cadastro()
        {
            if (perfilpermissaoRepository.PermissaoAutorizada(User.FindFirst(ClaimTypes.Role).Value, 2))
                return View();
            else
                return RedirectToAction("Consulta");
        }

        [HttpPost]
        public IActionResult Cadastro(AreaCadastroModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (areaRepository.ObterIdPorNome(model.Nome) != null)
                        throw new Exception("Área já cadastrada.");

                    var area = new Area();

                    area.Nome = model.Nome;
                    area.Desativar = model.Desativar ? "x" : null;

                    areaRepository.Inserir(area);

                    TempData["MensagemSucesso"] = $"{model.Nome}, cadastrado com sucesso.";
                    ModelState.Clear();
                }
                catch (Exception ex)
                {
                    TempData["MensagemErro"] = $"Erro: {ex.Message}";
                }
            }

            return View();
        }

        public IActionResult Consulta(AreaConsultaModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Areas = areaRepository.Consultar();
                    model.PerfisPermissoes = perfilpermissaoRepository.Consultar(User.FindFirst(ClaimTypes.Role).Value, 1); 
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = $"Erro ao consultar área(s): {e.Message}.";
                }
            }

            return View(model);
        }

        public JsonResult Exclusao(AreaConsultaModel model)
        {
            try
            {
                var area = areaRepository.ObterPorId(model.IdArea);

                if (area != null)
                    areaRepository.Excluir(area);

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
            var model = new AreaEdicaoModel();

            try
            {
                if (perfilpermissaoRepository.PermissaoAutorizada(User.FindFirst(ClaimTypes.Role).Value, 3))
                {
                    var area = areaRepository.ObterPorId(Convert.ToInt32(id));

                    model.IdArea = area.IdArea;
                    model.Nome = area.Nome;
                    model.Desativar = string.IsNullOrEmpty(area.Desativar) ? false : true;
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
        public IActionResult Edicao(AreaEdicaoModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var registro = areaRepository.ObterPorId(Convert.ToInt32(model.IdArea));

                    if (registro != null)
                    {
                        var idarea = areaRepository.ObterIdPorNome(model.Nome);

                        if (idarea == null || idarea == registro.IdArea)
                        {
                            registro.Nome = model.Nome;
                            registro.Desativar = model.Desativar == true ? "x" : null;

                            areaRepository.Alterar(registro);
                        }
                        else
                            throw new Exception("Area já cadastrada.");
                    }
                    else
                        throw new Exception("Area não encontrada.");

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