
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
        public async Task<IEnumerable<TEntity>> Get()
        {
            return await _repository.GetAllAsync();
        }

        [HttpGet("GetById")]
        [AllowAnonymous]
        public async Task<TEntity> GetById(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<TEntity> Create(TEntity user)
        {
            await _repository.CreateAsync(user);
            return user;
        }

        [HttpPut]
        public async Task<TEntity> Update(TEntity user)
        {
            var _user = await _repository.GetByIdAsync(user.Id);
            if (_user is not null)
                await _repository.UpdateAsync(user.Id, user);

            return user;
        }

        [HttpDelete]
        public async Task<TEntity> Delete(Guid userId)
        {
            var _user = await _repository.GetByIdAsync(userId);
            if (_user is not null)
                await _repository.DeleteAsync(userId);

            return _user;
        }
    }
}