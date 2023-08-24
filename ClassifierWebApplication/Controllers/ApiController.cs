using ClassifierWebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClassifierWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        [Route("/Classify/")]
        [HttpPost]
        public async Task<IActionResult> Classify(IFormFile file)
        {
            try
            {
                var uploadLocation = Path.Combine("wwwroot", "Images");
                var fileName = Guid.NewGuid().ToString();

                if (file.Length > 0)
                {
                    using (var stream = new FileStream(Path.Combine(uploadLocation, fileName), FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    Classifier classifier = new Classifier();
                    return Ok(classifier.GetStyle(Path.Combine(uploadLocation, fileName)).ToString());
                }

                return BadRequest();

            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }



        [HttpGet]
        public ClassifyViewModel GetConnection()
        {
            ClassifyViewModel model = new ClassifyViewModel();
            model.Style = Style.ArtInformel.ToString();
            model.Descriptors = new float[] { 1f, 2f, 3f };
            return model;
        }

    }
}
