namespace Git.Framework.ORM
{
    using System;
    using System.Collections.Generic;

    public interface IDbProcHelper<T> : IDisposable where T: BaseEntity
    {
        V ExceuteEntity<V>(T entity) where V: class, new();
        List<V> ExceuteEntityList<V>(T entity) where V: class, new();
        int ExecuteNonQuery(T entity);
        object ExecuteScalar(T entity);
        V ExecuteScalar<V>(T entity);
    }
}

