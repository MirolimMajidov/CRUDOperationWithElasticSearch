using Microsoft.AspNetCore.Mvc;
using MyUser.Models;
using MyUser.Services;

namespace MyUser.Controllers
{
    [Route("[controller]")]
    public class UserController : BaseController<User>
    {
        public UserController(ILogger<UserController> logger, IEntityRepository<User> repository) : base(logger, repository) { }
    }
}