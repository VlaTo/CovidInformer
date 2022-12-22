using System.Threading.Tasks;

namespace CovidInformer.Services
{
    public interface IDatabaseRewriter
    {
        Task RewriteDatabaseAsync();
    }
}