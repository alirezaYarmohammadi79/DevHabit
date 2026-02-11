using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using DevHabitApi.Services;

namespace DevHabitApi.DTOs.Common;

public record AcceptHeaderDto
{
    //[FromHeader(Name = "Accept")]
    //public string? Accept { get; set; }

    //public bool IncludeLinks => 
    //    MediaTypeHeaderValue.TryParse(Accept, out MediaTypeHeaderValue? mediaType) &&
    //    mediaType.SubTypeWithoutSuffix.Value &&
    //    mediaType.SubTypeWithoutSuffix.Value.Contains(CustomMediaTypeNames.Application.HateoasJson);
}
