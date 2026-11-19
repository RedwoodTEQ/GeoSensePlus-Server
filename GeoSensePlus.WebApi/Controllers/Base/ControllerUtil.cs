using GeoSensePlus.Data.DatabaseModels;
using GeoSensePlus.Data.DatabaseModels.Location;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NetCoreUtils.Database;
using System.Collections.Generic;

namespace GeoSensePlus.WebApi.Controllers.Base
{
    /// <summary>
    /// Usage: 
    /// 1. use IControllerUtil<T> as DI is preferred, as the type used in post body may not be the
    ///    same as the entity type, e.g. AreaEntity vs Area:
    /// 
    ///    [HttpPost]
    ///    public ActionResult<AreaEntity> Post([FromBody] AreaEntity value)
    ///    {
    ///        var area = new Area { Name = value.Name, Description = value.Description };
    ///        return _controllerUtil.Post(area);
    ///    }
    /// 
    /// 2. or use ControllerUtil<T> as a base class for an API controller if just want have a temporary
    ///    quick and dirty implementation with no DI
    /// </summary>
    /// <typeparam name="TEntity">An EF core entity class</typeparam>
    public interface IControllerUtil<TEntity> where TEntity : class
    {
        ActionResult Delete(int id);
        IEnumerable<TEntity> Get();
        ActionResult<TEntity> Get(int id);
        CreatedAtActionResult Post(TEntity entity);
        ActionResult Put(TEntity entity);
    }

    public class ControllerUtil<TEntity> : ControllerBase, IControllerUtil<TEntity> where TEntity : BaseEntity<int>
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

        [HttpGet]
        public IEnumerable<TEntity> Get()
        {
            return _repo.GetAllNoTracking();
        }

        [HttpGet("{id}")]
        public ActionResult<TEntity> Get(int id)
        {
            var result = _repo.Get(id);
            if (result is null)
                return NotFound();
            return result;
        }

        [HttpPost]
        public CreatedAtActionResult Post(TEntity entity)
        {
            _repo.Add(entity);
            _repo.Commit();

            return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
        }

        [HttpPut]
        public ActionResult Put(TEntity entity)
        {
            if (_repo.GetByIdNoTracking(entity.Id) is null)
                return NotFound();

            _repo.Update(entity);
            _repo.Commit();

            return NoContent();
        }

        [HttpDelete("{id}")]
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
