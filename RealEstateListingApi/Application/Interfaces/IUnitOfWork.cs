namespace RealEstateListingApi.Application.Interfaces;

public interface IUnitOfWork
{
    /// <summary>
    /// Commits all changes made in this unit of work to the database.
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
