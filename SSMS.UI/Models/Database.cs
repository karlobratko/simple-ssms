using Nito.AsyncEx;

using SSMS.UI.DAL;

namespace SSMS.UI.Models;
public sealed class Database {
  public readonly AsyncLazy<IList<DbEntity>> _tables;
  public readonly AsyncLazy<IList<DbEntity>> _views;

  public Database(string name) {
    Name = name;

    _tables = new(async () => await RepositoryFactory.Repository.GetEntities(this, DbEntity.DbEntityType.Table).ToListAsync());
    _views = new(async () => await RepositoryFactory.Repository.GetEntities(this, DbEntity.DbEntityType.View).ToListAsync());
  }

  public string Name { get; init; }
  public Task<IList<DbEntity>> Tables => _tables.Task;
  public Task<IList<DbEntity>> Views => _views.Task;

  

  public override string ToString() => $"{Name}";
}
