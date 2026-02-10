namespace DevHabitApi.DTOs.Common;

public interface ICollectionResponse<T>
{
    List<T> Data { get; init; }
}
