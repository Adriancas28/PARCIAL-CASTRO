
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PARCIAL_CASTRO.Data;
using PARCIAL_CASTRO.Models;


namespace PARCIAL_CASTRO.Controllers
{
    [Route("[controller]")]
    public class TransaccionesController : Controller
    {
        private readonly ILogger<TransaccionesController> _logger;
        private readonly ApplicationDbContext _context; // Agregamos el contexto de la base de datos

        public TransaccionesController(ILogger<TransaccionesController> logger,ApplicationDbContext context)
        {
            _logger = logger;
            _context = context; // Inyectamos el contexto
        }

        // Acción para mostrar el listado de bancos
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var transacciones = await _context.DataTransacciones.ToListAsync();
            return View(transacciones); // Pasamos la lista de bancos a la vista
        }

        // Acción para mostrar el formulario de registro
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]    
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Transacciones transacciones)
        {
            if (ModelState.IsValid)
            {
                // Calcular MontoFin antes de guardar
                transacciones.MontoFin = transacciones.MontoEnv * transacciones.TasaCam;

                // Agregar la transacción al contexto
                _context.Add(transacciones);
                await _context.SaveChangesAsync();

                // Redirigir al listado después de guardar
                return RedirectToAction(nameof(Index));
            }

            // En caso de que el modelo no sea válido, regresar la vista con el modelo
            return View(transacciones);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}