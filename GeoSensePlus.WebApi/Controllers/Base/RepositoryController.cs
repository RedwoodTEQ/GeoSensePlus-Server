using GeoSensePlus.Data.DatabaseModels.Base;
using Microsoft.AspNetCore.Mvc;
using NetCoreUtils.Database;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeoSensePlus.WebApi.Controllers.Base
{
    public interface IRepositoryController<TEntity> where TEntity : class, IIdAvailable<int>
    {
        ActionResult Delete(int id);
        IEnumerable<TEntity> Get();
        ActionResult<TEntity> Get(int id);
        CreatedAtActionResult Post(TEntity entity);
        ActionResult Put(TEntity entity);
    }

    public class RepositoryController<TEntity> : ControllerBase, IRepositoryController<TEntity> where TEntity : class, IIdAvailable<int>
    {
        private readonly IRepository<TEntity> _repo;

        public RepositoryController(IRepository<TEntity> repo)
        {
            _repo = repo;
        }

        public IEnumerable<TEntity> Get()
        {
            return _repo.GetAllNoTracking();
        }

        public ActionResult<TEntity> Get(int id)
        {
            var result = _repo.Get(id);
            if (result is null)
                return NotFound();
            return result;
        }

        public CreatedAtActionResult Post(TEntity entity)
        {
            _repo.Add(entity);
            _repo.Commit();

            return CreatedAtAction(nameof(Get), new { id = entity.GetId() }, entity);
        }

        public ActionResult Put(TEntity entity)
        {
            if (_repo.GetByIdNoTracking(entity.GetId()) is null)
                return NotFound();

            _repo.Update(entity);
            _repo.Commit();

            return NoContent();
        }

        public ActionResult Delete(int id)
        {
            var target = _repo.Get(id);
            if (target is null)
                return NotFound();

            _repo.Remove(target);
            _repo.Commit();

            return NoContent();
        }
    }
}
