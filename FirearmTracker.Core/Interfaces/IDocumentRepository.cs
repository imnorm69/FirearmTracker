using FirearmTracker.Core.Models;

namespace FirearmTracker.Core.Interfaces
{
    public interface IDocumentRepository
    {
        Task<List<Document>> GetByActivityIdAsync(int activityId);

        Task<List<Document>> GetByFirearmIdAsync(int firearmId);

        Task<Document?> GetByIdAsync(int id);

        Task<Document> AddAsync(Document document);

        Task DeleteAsync(int id);
    }
}