using DesafioVsoft.Domain.Entities;
using DesafioVsoft.Domain.Repositories;
using DesafioVsoft.Repository.Data;

namespace DesafioVsoft.Repository.Repositories;


/// <summary>
/// Repositório concreto para tarefas
/// </summary>
public class TaskRepository : BaseRepository<TaskItem>, ITaskRepository
{
    public TaskRepository(AppDbContext context) : base(context)
    {
    }

}
