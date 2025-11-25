using FirearmTracker.Core.Interfaces;
using FirearmTracker.Core.Models;
using FirearmTracker.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FirearmTracker.Data.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly FirearmTrackerContext _context;

        public DocumentRepository(FirearmTrackerContext context)
        {
            _context = context;
        }

        public async Task<List<Document>> GetByActivityIdAsync(int activityId)
        {
            return await _context.Documents
                .Where(d => d.ActivityId == activityId && !d.IsDeleted)
                .OrderBy(d => d.UploadedDate)
                .ToListAsync();
        }

        public async Task<List<Document>> GetByFirearmIdAsync(int firearmId)
        {
            return await _context.Documents
                .Where(d => !d.IsDeleted && (d.FirearmId == firearmId || d.Activity!.FirearmId == firearmId))
                .OrderBy(d => d.UploadedDate)
                .ToListAsync();
        }

        public async Task<Document?> GetByIdAsync(int id)
        {
            return await _context.Documents.FindAsync(id);
        }

        public async Task<Document> AddAsync(Document document)
        {
            document.UploadedDate = DateTime.UtcNow;
            _context.Documents.Add(document);
            await _context.SaveChangesAsync();
            return document;
        }

        public async Task DeleteAsync(int id)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document != null)
            {
                _context.Documents.Remove(document);
                await _context.SaveChangesAsync();
            }
        }
    }
}