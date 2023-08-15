using GeoSensePlus.Data.DatabaseModels.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NetCoreUtils.Database;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeoSensePlus.WebApi.Controllers.Base
{
    public interface IControllerUtil<TEntity> where TEntity : class, IIdAvailable<int>
    {
        ActionResult Delete(int id);
        IEnumerable<TEntity> Get();
        ActionResult<TEntity> Get(int id);
        CreatedAtActionResult Post(TEntity entity);
        ActionResult Put(TEntity entity);
    }

    public class ControllerUtil<TEntity> : ControllerBase, IControllerUtil<TEntity> where TEntity : class, IIdAvailable<int>
    {
        private readonly IRepository<TEntity> _repo;

        /** About HttpContext.RequestServices.GetService<T>():
         * Not recommended as it will make the dependencies inexplicit, better
         * to use constructor injection instead
         */
        public T GetService<T>()
        {
            return HttpContext.RequestServices.GetService<T>();
        }

        public ControllerUtil(IRepository<TEntity> repo)
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

    public static class RepositoryControllerExt
    {
        public static void AddControllerUtil(this IServiceCollection services)
        {
            services.AddTransient(typeof(IControllerUtil<>), typeof(ControllerUtil<>));
        }
    }
}
