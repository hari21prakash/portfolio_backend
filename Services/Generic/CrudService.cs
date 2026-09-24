using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;

namespace PortfolioApi.Services.Generic;

// A small, generic data-access helper for entities that are plain CRUD
// (Skill, Experience, Education, Certification, Achievement, Service, SocialLink, Technology).
// Entity <-> DTO mapping stays in each controller's dedicated mapping extensions,
// so this class only ever touches EF — it has no knowledge of DTOs.
public class CrudService<TEntity> where TEntity : class
{
    private readonly ApplicationDbContext _db;
    private readonly DbSet<TEntity> _set;

    public CrudService(ApplicationDbContext db)
    {
        _db = db;
        _set = db.Set<TEntity>();
    }

    public IQueryable<TEntity> Query() => _set.AsQueryable();

    public Task<TEntity?> FindAsync(int id) => _set.FindAsync(id).AsTask();

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        _set.Add(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task SaveAsync() => await _db.SaveChangesAsync();

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _set.FindAsync(id);
        if (entity is null) return false;

        _set.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }
}
