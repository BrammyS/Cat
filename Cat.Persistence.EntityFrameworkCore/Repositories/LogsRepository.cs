using Cat.Persistence.Domain.Tables;
using Cat.Persistence.EntityFrameworkCore.Models;
using Cat.Persistence.Interfaces.Repositories;


namespace Cat.Persistence.EntityFrameworkCore.Repositories
{

    public class LogsRepository : Repository<Log>, ILogsRepository
    {

        public LogsRepository(CatContext context) : base(context)
        {
        }

    }
}
