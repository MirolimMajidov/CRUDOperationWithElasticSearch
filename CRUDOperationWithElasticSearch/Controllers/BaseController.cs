
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyUser.Models;
using MyUser.Services;

namespace MyUser.Controllers
{
    [ApiController]
    [Authorize]
    public abstract class BaseController<TEntity> : ControllerBase where TEntity : BaseEntity
    {
        protected readonly ILogger<ControllerBase> _logger;
        protected readonly IEntityRepository<TEntity> _repository;

        public BaseController(ILogger<ControllerBase> logger, IEntityRepository<TEntity> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<TEntity> Get()
        {
            return _repository.GetAll().ToList();
        }

        [HttpGet("GetById")]
        [AllowAnonymous]
        public TEntity GetById(Guid id)
        {
            return _repository.GetById(id);
        }

        [HttpPost]
        public TEntity Create(TEntity user)
        {
            _repository.Create(user);

            return user;
        }

        [HttpPut]
        public TEntity Update(TEntity user)
        {
            var _user = _repository.GetById(user.Id);
            if (_user is not null)
                _repository.Update(user);

            return user;
        }

        [HttpDelete]
        public TEntity Delete(Guid userId)
        {
            var _user = _repository.GetById(userId);
            if (_user is not null)
                _repository.Remove(_user);

            return _user;
        }
    }
}