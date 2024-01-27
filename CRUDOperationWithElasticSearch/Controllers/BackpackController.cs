using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyUser.Models;
using MyUser.Services;

namespace MyUser.Controllers
{
    [Route("[controller]")]
    public class BackpackController : BaseController<Backpack>
    {
        public BackpackController(ILogger<BackpackController> logger, IEntityRepository<Backpack> repository) : base(logger, repository) { }
    }
}