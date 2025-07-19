using DesafioVsoft.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioVsoft.Domain.Commons;

public class Pagination<T> where T : class
{
    public int TotalRecords { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public List<T> Items { get; set; } = new List<T>();
}
