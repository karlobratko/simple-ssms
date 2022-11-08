using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMS.UI.DAL;
public static class RepositoryFactory {
  private static readonly Lazy<IRepository> _repository = new(() => new SqlRepository());

  public static IRepository Repository => _repository.Value;
}
