using ContactosWEB.Models;
using ContactosWEB.Models.Contexto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ContactosWEB.Controllers
{
    public class ContactosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHost;

        public ContactosController(ApplicationDbContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Contactos.ToListAsync());
        }
        [HttpGet]
        public IActionResult CrearContacto()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CrearContacto(Contacto contacto, IFormFile foto)
        {
            if (ModelState.IsValid)
            {
                if (foto != null)
                {
                    string directorio = Path.Combine(_webHost.WebRootPath, "img");
                    string nombreFoto = Guid.NewGuid().ToString() + foto.FileName;

                    using (FileStream fileStream = new FileStream(Path.Combine(directorio, nombreFoto), FileMode.Create))
                    {
                        await foto.CopyToAsync(fileStream);
                        contacto.Foto = "~/img/" + nombreFoto;
                    }
                }

                else
                {
                    contacto.Foto = "~/img/usuario.png";
                }
                _context.Add(contacto);
                await _context.SaveChangesAsync();
                TempData["NuevoContacto"] = $"Contacto con el nombre {contacto.Nombre} agregado correctamente";
                return RedirectToAction(nameof(Index));
            }

            return View(contacto);

        }
        [HttpGet]
        public async Task<IActionResult> ActualizarContacto(int contactoId)
        {
            Contacto contacto = await _context.Contactos.FindAsync(contactoId);

            if(contacto == null)
            {
                return NotFound();
            }
            TempData["Foto"] = contacto.Foto;
            return View(contacto);
        }
        [HttpPost]
        public async Task<IActionResult> ActualizarContacto(Contacto contacto, IFormFile foto)
        {
            if (ModelState.IsValid)
            {
                if(foto != null)
                {
                    string directorio = Path.Combine(_webHost.WebRootPath, "img");
                    string nombreFoto = Guid.NewGuid().ToString() + foto.FileName;

                    using(FileStream fileStream = new FileStream(Path.Combine(directorio, nombreFoto), FileMode.Create))
                    {
                        await foto.CopyToAsync(fileStream);
                        contacto.Foto = "~/img/" + nombreFoto;
                        TempData["Foto"] = null;
                    }
                }
                else
                {
                    contacto.Foto = TempData["Foto"].ToString();
                }

                _context.Update(contacto);
                await _context.SaveChangesAsync();
                TempData["ContactoActualizado"] = $"Contacto con el nombre {contacto.Nombre} ha sido actualizado!";
                return RedirectToAction(nameof(Index));
            }
            return View(contacto);
        }
        [HttpPost]
        public async Task<JsonResult> EliminarContacto(int contactoId)
        {
            Contacto contacto = await _context.Contactos.FindAsync(contactoId);
            _context.Contactos.Remove(contacto);
            await _context.SaveChangesAsync();
            TempData["ContactoEliminado"] = $"Contacto {contacto.Nombre} elimiado!";
            return Json(true);
        }
    }
}
