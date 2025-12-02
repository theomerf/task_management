using Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public record ProjectDtoForUpdate : ProjectDtoForCreation
    {
        public Guid Id { get; init; }
    }
}
