using System.Linq.Expressions;

namespace RTCodingExercise.Monolithic.DataAccess.Interfaces;

public interface IRepository<T>
{
    IEnumerable<T> Get(
        Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        string includeProperties = "");

    T GetById(object id);
    void Insert(T entity);
    void Delete(object id);
    void Delete(T entityToDelete);
    void Update(T entityToUpdate);
    void Save();
}