using System.ComponentModel.DataAnnotations;

namespace FamilyFeet.Api.Dtos.Pagination;

/// <summary>
/// Base class for every paginated GET request. Inherit from it to add filters.
/// </summary>
public class PaginationDto
{
    /// <summary>Page to retrieve (starts at 1).</summary>
    [Range(1, int.MaxValue, ErrorMessage = "PageNumber must be greater than or equal to 1.")]
    public int PageNumber { get; set; } = 1;

    /// <summary>Records per page (1 to 100).</summary>
    [Range(1, 100, ErrorMessage = "PageSize must be between 1 and 100.")]
    public int PageSize { get; set; } = 10;
}
