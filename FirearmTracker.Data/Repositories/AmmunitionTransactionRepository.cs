using FirearmTracker.Core.Interfaces;
using FirearmTracker.Core.Models;
using FirearmTracker.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FirearmTracker.Data.Repositories
{
    public class AmmunitionTransactionRepository : IAmmunitionTransactionRepository
    {
        private readonly FirearmTrackerContext _context;

        public AmmunitionTransactionRepository(FirearmTrackerContext context)
        {
            _context = context;
        }

        public async Task<List<AmmunitionTransaction>> GetAllAsync()
        {
            return await _context.AmmunitionTransactions
                .Include(t => t.Ammunition)
                .Include(t => t.Activity)
                .IgnoreQueryFilters()
                .ToListAsync();
        }

        public async Task<AmmunitionTransaction?> GetByIdAsync(int id)
        {
            return await _context.AmmunitionTransactions
                .Include(t => t.Ammunition)
                .Include(t => t.Activity)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<AmmunitionTransaction>> GetByAmmunitionIdAsync(int ammunitionId)
        {
            return await _context.AmmunitionTransactions
                .Include(t => t.Activity)
                .Where(t => t.AmmunitionId == ammunitionId)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<List<AmmunitionTransaction>> GetByActivityIdAsync(int activityId)
        {
            return await _context.AmmunitionTransactions
                .Include(t => t.Ammunition)
                .Where(t => t.ActivityId == activityId)
                .ToListAsync();
        }

        public async Task<AmmunitionTransaction> AddAsync(AmmunitionTransaction transaction)
        {
            _context.AmmunitionTransactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task UpdateAsync(AmmunitionTransaction transaction)
        {
            _context.AmmunitionTransactions.Update(transaction);
            await _context.SaveChangesAsync();
        }
    }
}