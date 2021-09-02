using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Projeto.Data.Contracts;
using Projeto.Data.Dtos;
using Projeto.Data.Entities;
using Projeto.Presentation.Areas.AreaRestrita.Models;

namespace Projeto.Presentation.Areas.AreaRestrita.Controllers
{
    [Area("AreaRestrita")]
    public class PermissaoController : Controller
    {
        private readonly IPermissaoRepository permissaoRepository;
        private readonly IPerfilRepository perfilRepository;
        private readonly IPerfilPermissaoRepository perfilpermissaoRepository;

        public PermissaoController(IPermissaoRepository permissaoRepository, IPerfilPermissaoRepository perfilpermissaoRepository, IPerfilRepository perfilRepository)
        {
            this.permissaoRepository = permissaoRepository;
            this.perfilpermissaoRepository = perfilpermissaoRepository;
            this.perfilRepository = perfilRepository;
        }

        public IActionResult Cadastro()
        {
            var model = new PermissaoCadastroModel();
            model.Perfis = new List<SelectListItem>();

            try
            {
                model.Perfis = ListarPerfil(perfilRepository.Consultar());
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro: {ex.Message}";
            }

            return View(model);
        }

        public IActionResult Edicao(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Cadastro");
            else
            {
                var model = new PermissaoEdicaoModel();

                model.Perfis = new List<SelectListItem>();
                model.PermissoesMenu = new List<PermissaoMenu>();

                try
                {
                    model.IdPerfil = int.Parse(id);
                    model.Perfis = ListarPerfil(perfilRepository.Consultar());

                    foreach (var item in permissaoRepository.ObterPai())
                    {
                        var perfilpermissao = perfilpermissaoRepository.PermissaoAutorizada(int.Parse(id), item.IdPermissao);

                        var registro = new PermissaoMenu
                        {
                            //Id = perfilpermissao == null ? (int?)null : perfilpermissao.Id,
                            IdPermissao = item.IdPermissao,
                            Descricao = item.Descricao,
                            Check = perfilpermissao == null ? false : true,
                            IdPai = item.IdPai
                        };
                       
                        foreach (var subitem in item.Permissoes)
                        {
                            var perfilpermissaosub = perfilpermissaoRepository.PermissaoAutorizada(int.Parse(id), subitem.IdPermissao);

                            registro.PermissaoSubMenu.Add(
                                                            new PermissaoMenu
                                                            {
                                                                //Id = perfilpermissaosub == null ? (int?)null : perfilpermissaosub.Id,
                                                                IdPermissao = subitem.IdPermissao,
                                                                Descricao = subitem.Descricao,
                                                                Check = perfilpermissaosub == null ? false : true,
                                                                IdPai = subitem.IdPai
                                                            });
                        }


                        model.PermissoesMenu.Add(registro);
                    }

                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }

                return View(model);
            }
        }

        [HttpPost]
        public IActionResult Edicao(PermissaoEdicaoModel model)
        {
            try
            {
                model.Perfis = new List<SelectListItem>();
                model.Perfis = ListarPerfil(perfilRepository.Consultar());

                foreach (var item in model.PermissoesMenu)
                {
                    AtualizarPermissao(model.IdPerfil, item.IdPermissao, item.Check);

                    foreach (var subitem in item.PermissaoSubMenu)
                    {
                        AtualizarPermissao(model.IdPerfil, subitem.IdPermissao, subitem.Check);
                    }
                }

                TempData["MensagemSucesso"] = "Perfil atualizado com sucesso.";

                // return RedirectToAction("Edicao", new { id = model.IdPerfil });
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
            }

            return View(model);
        }

        private List<SelectListItem> ListarPerfil(List<Perfil> lista)
        {
            var registros = new List<SelectListItem>();

            registros.Add(new SelectListItem() { Text = "", Value = null });

            foreach (var item in lista)
            {
                var opcao = new SelectListItem();
                opcao.Value = item.IdPerfil.ToString();
                opcao.Text = item.Descricao;

                registros.Add(opcao);
            }

            return registros.OrderBy(x => x.Text).ToList();
        }

        private void AtualizarPermissao(int idperfil, int idpermissao, bool check)
        {
            var registro = perfilpermissaoRepository.PermissaoAutorizada(idperfil, idpermissao);

            if (check == true && registro == null)
            {
                var perfilPermissao = new PerfilPermissao
                {
                    IdPerfil = idperfil,
                    IdPermissao = idpermissao
                };

                perfilpermissaoRepository.Inserir(perfilPermissao);
            }
            else if (check == false && registro != null)
            {
                perfilpermissaoRepository.Excluir(registro);
            }
        }
    }
}