using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Projeto.Data.Contracts;
using Projeto.Presentation.Models;

namespace Projeto.Presentation.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model,
                                   [FromServices] IColaboradorRepository colaboradorRepository)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!colaboradorRepository.PerfilExiste(model.Usuario))
                        throw new Exception("Permissão de acesso negado.");

                    if (colaboradorRepository.PasswordExiste(model.Usuario))
                    {
                        var usuario = colaboradorRepository.Consultar(model.Usuario, model.Senha);

                        if (usuario != null)
                        {
                            UserModel userModel = new UserModel();

                            userModel.Chapa = usuario.Chapa;
                            userModel.Nome = usuario.Nome;
                            userModel.Perfil = usuario.Perfil.Descricao;
                            userModel.Acesso = string.Format("{0:dd/MM/yyyy HH:mm} | {1}", DateTime.Now, Request.Host);

                            //criando a identificação de acesso..
                            var identity = new ClaimsIdentity(
                                new[]
                                { 
                                //gravando os dados da model em formato JSON no cookie
                                new Claim(ClaimTypes.Name, JsonConvert.SerializeObject(userModel)),
                                new Claim(ClaimTypes.Role, userModel.Perfil) //perfil
                                },
                                CookieAuthenticationDefaults.AuthenticationScheme);

                            //gravar em cookie
                            var principal = new ClaimsPrincipal(identity);
                            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                            //redirecionamento
                            return RedirectToAction("Index", "Home", new { area = "AreaRestrita" });
                        }
                        else
                        {
                            throw new Exception("Acesso negado");
                        }                       
                    }
                    else
                    {
                        //Validar active directory 
                    }

                }
                catch (Exception ex)
                {
                    TempData["MensagemErro"] = ex.Message;
                }
            }

            return View();
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Logout()
        {
            //destruir o cookie de autenticação do usuário
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //redirecionando..
            return RedirectToAction("Login");
        }
    }
}