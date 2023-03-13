using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using TransportWebApplication.Models;

namespace TransportWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly TransportContext _context;

        public ChartController(TransportContext context)
        {
            _context = context;
        }

        [HttpGet("ModelsData")]
        public JsonResult ModelsData()
        {
            var models = _context.Models.ToList();
            var autos = _context.Autos;

            List<object> data = new()
            {
                new object[] { "Модель", "Кількість авто" }
            };
            foreach (var model in models)
            {
                data.Add(new object[] { model.Name, autos.Where(auto => auto.ModelId == model.Id).ToList().Count /*model.Autos.Count*/});
            }
            return new JsonResult(data);
        }
        [HttpGet("ColorsData")]
        public JsonResult ColorsData()
        {
            var autos = _context.Autos.ToList();

            HashSet<string> colors = new();

            autos.ForEach(auto =>
            {
                colors.Add(auto.Color);
            });

            var colorCounts = colors.Select(color =>
                new
                {
                    Color = color,
                    Count = autos.Count(auto => auto.Color == color)
                }
            );

            List<object> data = new()
            {
                new object[] { "Колір", "Кількість авто" }
            };

            foreach (var color in colorCounts)
            {
                data.Add(new object[] { color.Color, color.Count});
            }

            return new JsonResult(data);
        }
    }
}
