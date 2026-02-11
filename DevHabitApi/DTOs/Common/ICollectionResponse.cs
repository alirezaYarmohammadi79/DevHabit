namespace DevHabitApi.DTOs.Common;

public interface ICollectionResponse<T>
{
    List<T> Data { get; init; }
}

public interface ILinkResponse
{
    List<LinkDto> Links { get; set; }
}
