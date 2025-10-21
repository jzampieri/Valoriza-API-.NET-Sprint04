using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Valoriza.Application.DTOs;
using Valoriza.Domain.Entities;
using Valoriza.Infrastructure;

namespace Valoriza.Application.Services
{
    public class TransactionService
    {
        private readonly ValorizaDbContext _db;
        private readonly IMapper _mapper;

        public TransactionService(ValorizaDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<TransactionViewDto> CreateAsync(TransactionCreateDto dto)
        {
            var entity = _mapper.Map<TransactionRecord>(dto);
            entity.IsRisky = entity.Category == Category.Gambling || entity.Amount >= 500m;
            _db.Transactions.Add(entity);
            await _db.SaveChangesAsync();
            return _mapper.Map<TransactionViewDto>(entity);
        }

        public async Task<TransactionViewDto?> GetAsync(Guid id) =>
            await _db.Transactions
                .AsNoTracking()
                .Where(t => t.Id == id)
                .ProjectTo<TransactionViewDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

        public async Task<IReadOnlyList<TransactionViewDto>> ListByUserAsync(Guid userId) =>
            await _db.Transactions
                .AsNoTracking()
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.Date)
                .ProjectTo<TransactionViewDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _db.Transactions.FindAsync(id);
            if (entity is null) return false;
            _db.Transactions.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<TransactionViewDto?> UpdateAsync(Guid id, TransactionUpdateDto dto)
        {
            var entity = await _db.Transactions.FindAsync(id);
            if (entity is null) return null;
            entity.Amount = dto.Amount;
            entity.Description = dto.Description;
            entity.Category = dto.Category;
            entity.Date = dto.Date ?? DateTime.UtcNow;
            entity.IsRisky = dto.IsRisky;
            await _db.SaveChangesAsync();
            return _mapper.Map<TransactionViewDto>(entity);
        }

        public async Task<IReadOnlyList<TransactionViewDto>> SearchAsync(
            Guid userId, DateTime? start, DateTime? end, Category? category, decimal? min, decimal? max, bool? risky)
        {
            var q = _db.Transactions.AsNoTracking().Where(t => t.UserId == userId);
            if (start.HasValue) q = q.Where(t => t.Date >= start.Value);
            if (end.HasValue) q = q.Where(t => t.Date <= end.Value);
            if (category.HasValue) q = q.Where(t => t.Category == category.Value);
            if (min.HasValue) q = q.Where(t => t.Amount >= min.Value);
            if (max.HasValue) q = q.Where(t => t.Amount <= max.Value);
            if (risky.HasValue) q = q.Where(t => t.IsRisky == risky.Value);

            return await q.OrderByDescending(t => t.Amount)
                          .Take(10)
                          .ProjectTo<TransactionViewDto>(_mapper.ConfigurationProvider)
                          .ToListAsync();
        }
    }
}
