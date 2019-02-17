using System.Collections.Generic;
using TvApi.Models;

namespace TvApi.Core
{
    public interface IShowRepository
    {
        IEnumerable<Show> GetAll();
        void Insert(Show item);
    }
}