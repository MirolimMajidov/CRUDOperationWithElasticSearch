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
        public virtual async Task<IEnumerable<TEntity>> Get(int? from = null, int? size = null)
        {
            return await _repository.GetAllAsync(from, size);
        }

        [HttpGet("GetById")]
        [AllowAnonymous]
        public async Task<ActionResult> GetById(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return NotFound();

            return Ok(entity);
        }

        [HttpPost]
        public async Task<ActionResult> Create(TEntity user)
        {
            var entity = await _repository.CreateAsync(user);
            if (entity == null)
                return BadRequest();

            return Ok(entity);
        }

        [HttpPut]
        public async Task<ActionResult> Update(Guid userId, TEntity user)
        {
            var _user = await _repository.UpdateAsync(user.Id, user);
            if (_user == null)
                return BadRequest();

            return Ok(_user);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(Guid userId)
        {
            var response = await _repository.DeleteAsync(userId);
            if (response)
                BadRequest();

            return Ok();
        }
    }
}