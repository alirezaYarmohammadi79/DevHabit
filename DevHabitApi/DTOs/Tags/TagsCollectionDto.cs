using DevHabitApi.DTOs.Common;

namespace DevHabitApi.DTOs.Tags;

public sealed record TagsCollectionDto : ICollectionResponse<TagDto>
{
    public List<TagDto> Data { get; init; }
}
