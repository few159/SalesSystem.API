﻿using SalesSystem.Domain.Entities.Snapshot;

namespace SalesSystem.Application.Interfaces.Repositories.Stores;

public interface IProductSnapshotStore
{
    Task<IEnumerable<ProductSnapshot>> GetAllAsync();
    Task<ProductSnapshot?> GetAsync(Guid id);
}