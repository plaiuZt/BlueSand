using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;

namespace Git.Framework.ORM
{
    public interface IDbHelper<T> : IDisposable where T: BaseEntity
    {
        int Add(T entity);
        int Add(List<T> list);
        int Add(T entity, bool isOpenTrans);
        int Add(List<T> list, bool isOpenTrans);
        int Delete(IEnumerable<int> ids);
        int Delete(int id);
        int Delete(object value);
        int Delete(T entity);
        int Delete(IEnumerable<int> ids, bool isOpenTrans);
        int Delete(int id, bool isOpenTrans);
        int Delete(object value, bool isOpenTrans);
        int Delete(T entity, bool isOpenTrans);
        int DeleteBatch(IEnumerable<T> list, bool isOpenTrans);
        int GetCount();
        int GetCount(bool isOpenTrans);
        int GetCount(T entity);
        int GetCount(T entity, bool isOpenTrans);
        List<T> GetList();
        List<T> GetList(bool isOpenTrans);
        List<T> GetList(T entity);
        List<V> GetList<V>(T entity) where V: class, new();
        List<T> GetList(T entity, bool isOpenTrans);
        List<V> GetList<V>(T entity, bool isOpenTrans) where V: class, new();
        List<T> GetList(T entity, int pageSize, int pageIndex, out int rowCount);
        List<V> GetList<V>(T entity, int pageSize, int pageIndex, out int rowCount) where V: class, new();
        List<T> GetList(T entity, int pageSize, int pageIndex, out int rowCount, bool isOpenTrans);
        List<V> GetList<V>(T entity, int pageSize, int pageIndex, out int rowCount, bool isOpenTrans) where V: class, new();
        T GetSingle(int id);
        T GetSingle(object value);
        T GetSingle(T entity);
        V GetSingle<V>(T entity) where V: class, new();
        T GetSingle(int id, bool isOpenTrans);
        T GetSingle(T entity, bool isOpenTrans);
        V GetSingle<V>(T entity, bool isOpenTrans) where V: class, new();
        DataTable GetTable();
        DataTable GetTable(bool isOpenTrans);
        DataTable GetTable(T entity);
        DataTable GetTable(T entity, bool isOpenTrans);
        DataTable Group(T entity);
        DataTable Group(T entity, bool isOpenTrans);
        IEnumerable<IGrouping<TKey, T>> Group<TKey>(T entity, Func<T, TKey> keySelector);
        V Max<V>(T entity);
        V Max<V>(T entity, bool isOpenTrans);
        V Min<V>(T entity);
        V Min<V>(T entity, bool isOpenTrans);
        V Sum<V>(T entity);
        V Sum<V>(T entity, bool isOpenTrans);
        List<T> Top(T entity, int pageSize);
        List<V> Top<V>(T entity, int pageSize) where V: class, new();
        List<T> Top(T entity, int pageSize, bool isOpenTrans);
        List<V> Top<V>(T entity, int pageSize, bool isOpenTrans) where V: class, new();
        List<T> Top(T entity, int skipSize, int pageSize);
        List<V> Top<V>(T entity, int skipSize, int pageSize) where V: class, new();
        List<T> Top(T entity, int skipSize, int pageSize, bool isOpenTrans);
        List<V> Top<V>(T entity, int skipSize, int pageSize, bool isOpenTrans) where V: class, new();
        int Update(T entity);
        int Update(List<T> list);
        int Update(T entity, bool isOpenTrans);
        int Update(List<T> list, bool isOpenTrans);
    }
}

