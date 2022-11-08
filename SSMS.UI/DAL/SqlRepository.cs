using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text.RegularExpressions;

using SSMS.UI.Models;

namespace SSMS.UI.DAL;
public class SqlRepository : IRepository {
  private const string SelectDatabases = "SELECT name As Name FROM sys.databases";
  private const string SelectEntities = "SELECT TABLE_SCHEMA AS [Schema], TABLE_NAME AS Name FROM {0}.INFORMATION_SCHEMA.{1}S";
  //private const string SelectProcedures = "SELECT SPECIFIC_NAME as Name, ROUTINE_DEFINITION as Definition FROM {0}.INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE = 'PROCEDURE'";

  //private const string SelectColumns = "SELECT COLUMN_NAME as Name, DATA_TYPE as DataType FROM {0}.INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{1}'";
  //private const string SelectProcedureParameters = "SELECT PARAMETER_NAME as Name, PARAMETER_MODE as Mode, DATA_TYPE as DataType FROM {0}.INFORMATION_SCHEMA.PARAMETERS WHERE SPECIFIC_NAME='{1}'";
  //private const string SelectQuery = "SELECT * FROM {0}.{1}.{2}";

  private LoginInformation? _loginInformation;

  private string GetConnectionString(string? database = null) => 
    database is not null 
      ? $"Server={_loginInformation?.ServerName};Database={database};Uid={_loginInformation?.Credentials.Login};Pwd={_loginInformation?.Credentials.Password}"
      : $"Server={_loginInformation?.ServerName};Uid={_loginInformation?.Credentials.Login};Pwd={_loginInformation?.Credentials.Password}";

  public async Task LoginAsync(LoginInformation loginInformation) {
    try {
      _loginInformation = loginInformation;
      using var connection = new SqlConnection(GetConnectionString());
      await connection.OpenAsync();
    } catch {
      _loginInformation = null;
      throw;
    }
  }

  public void Logout() => _loginInformation = null;

  public async IAsyncEnumerable<Database> GetDatabasesAsync() {
    using var connection = new SqlConnection(GetConnectionString());
    await connection.OpenAsync();

    using SqlCommand? cmd = connection.CreateCommand();
    cmd.CommandText = SelectDatabases;
    cmd.CommandType = CommandType.Text;

    using SqlDataReader dr = await cmd.ExecuteReaderAsync();

    while (await dr.ReadAsync())
      yield return new Database(await dr.GetFieldValueAsync<string>(dr.GetOrdinal(nameof(Database.Name))));
  }

  public async IAsyncEnumerable<DbEntity> GetEntities(Database database, DbEntity.DbEntityType entityType) {
    using var connection = new SqlConnection(GetConnectionString());
    await connection.OpenAsync();

    using SqlCommand? cmd = connection.CreateCommand();
    cmd.CommandText = string.Format(SelectEntities, database.Name, entityType.ToString().ToUpper());
    cmd.CommandType = CommandType.Text;

    using SqlDataReader dr = await cmd.ExecuteReaderAsync();

    while (await dr.ReadAsync())
      yield return new DbEntity {
        Name = await dr.GetFieldValueAsync<string>(dr.GetOrdinal(nameof(DbEntity.Name))),
        Schema = await dr.GetFieldValueAsync<string>(dr.GetOrdinal(nameof(DbEntity.Schema))),
      };
  }

  public async Task<DataTable> ExecuteQueryAsync(string database, string statement) {
    using var connection = new SqlConnection(GetConnectionString(database));
    await connection.OpenAsync();

    using SqlCommand? cmd = connection.CreateCommand();
    cmd.CommandText = statement;
    cmd.CommandType = CommandType.Text;

    using SqlDataReader dr = await cmd.ExecuteReaderAsync();
    var dt = new DataTable();
    dt.Load(dr);
    return dt;
  }

  public async Task<int> ExecuteNonQueryAsync(string database, string statement) {
    using var connection = new SqlConnection(GetConnectionString(database));
    await connection.OpenAsync();

    using SqlCommand? cmd = connection.CreateCommand();
    cmd.CommandText = statement;
    cmd.CommandType = CommandType.Text;

    return await cmd.ExecuteNonQueryAsync();
  }
}
