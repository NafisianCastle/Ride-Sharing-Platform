﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RideSharing.Application.Abstractions;
using RideSharing.Domain.Entities;

namespace RideSharing.Infrastructure.Repositories
{
	// TODO: use primary key type generically
	public class BaseRepository<T> : IBaseRepository<T>
		where T : BaseEntity
	{

		#region initializations & declarations

		protected readonly ApplicationDbContext _dbContext;
		protected readonly DapperContext _dapperContext;
		protected DbSet<T> _dbSet;

		public BaseRepository(ApplicationDbContext dbContext, DapperContext dapperContext)
		{
			_dbContext = dbContext;
			_dapperContext = dapperContext;
			_dbSet = dbContext.Set<T>();
		}

		#endregion

		public DbSet<T> DbSet
		{
			get { return _dbSet; }
		}

		#region Transactional helpers

		public virtual async Task<IDbContextTransaction> BeginTransactionAsync()
		{
			var transaction = await _dbContext.Database.BeginTransactionAsync();
			return transaction;
		}

		public virtual async Task CommitTransactionAsync(IDbContextTransaction transaction)
		{
			await transaction.CommitAsync();
		}

		public virtual async Task RollBackTransactionAsync(IDbContextTransaction transaction)
		{
			await transaction.RollbackAsync();
		}

		#endregion

		/// <summary>
		/// Need to be called after certain operations on db: add update delete, otherwise changes will not be saved..
		/// </summary>
		/// <returns></returns>
		public virtual async Task<int> SaveChangesAsync()
		{
			return await _dbContext.SaveChangesAsync();
		}

		// TODO: use generic pagination here
		public virtual async Task<IEnumerable<T>> FindAllAsync()
		{
			return await _dbSet.ToListAsync();
		}

		/// <summary>
		/// Finds an the entity by primary key.
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Returns the entity(T) found or null.</returns>
		public virtual async Task<T> FindByIdAsync(long id) => await _dbSet.FindAsync(id);

		public virtual async Task<T> AddAsync(T item)
		{
			item.Id = 0; // EF Core auto generates Primary Key fields when Id is zero.
			item.SetCreatedAt();

			await _dbSet.AddAsync(item);
			await _dbContext.SaveChangesAsync();
			return item;
		}

		public virtual async Task<T> UpdateByIdAsync(long id, T item)
		{
			if (id != item.Id) throw new Exception("Access restricted!");
			return await UpdateAsync(item);
		}

		public virtual async Task<T> UpdateAsync(T item)
		{
			item.UpdateLastModifiedAt();

			_dbSet.Update(item);
			await _dbContext.SaveChangesAsync();
			return item;
		}

		public virtual async Task<T> DeleteAsync(T item)
		{
			_dbSet.Remove(item);
			await _dbContext.SaveChangesAsync();
			return item;
		}

		public virtual async Task<T> DeleteByIdAsync(long id)
		{
			var itemToBeDeleted = await _dbSet.FindAsync(id);

			if (itemToBeDeleted == null) throw new Exception("Item not found!");

			return await DeleteAsync(itemToBeDeleted);
		}
	}
}
