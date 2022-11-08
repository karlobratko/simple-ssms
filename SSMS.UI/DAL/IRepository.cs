using System.Data;

using SSMS.UI.Models;

namespace SSMS.UI.DAL;
public interface IRepository {
  Task LoginAsync(LoginInformation loginInformation);
  void Logout();
  IAsyncEnumerable<Database> GetDatabasesAsync();
  IAsyncEnumerable<DbEntity> GetEntities(Database database, DbEntity.DbEntityType entityType);
  Task<DataTable> ExecuteQueryAsync(string database, string statement);
  Task<int> ExecuteNonQueryAsync(string database, string statement);
}
