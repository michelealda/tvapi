using System;
using System.Linq;
using TvApi.Infrastructure.Dto;
using TvApi.Models;

namespace TvApi.Infrastructure.Mappers
{
    public static class TvShowMapper
    {
        public static Show MapToShow(this TvShowDto dto)
            => new Show
            {
                Id = dto.Id,
                Name = dto.Name,
                Cast = dto._embedded
                    .Cast?
                    .Select(c => c.Person)
                    .OrderBy(p => p.Birthday ?? DateTime.MaxValue)
                    .Select(MapToPerson)
                    .ToArray()
            };
        
        public static Person MapToPerson(this PersonDto dto)
            => new Person
            {
                Id = dto.Id,
                Name = dto.Name,
                Birthday = dto.Birthday
            };

    }
}