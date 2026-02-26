using System.Dynamic;
using System.Linq.Dynamic.Core;
using System.Net.Mime;
using Asp.Versioning;
using DevHabitApi.Database;
using DevHabitApi.DTOs.Common;
using DevHabitApi.DTOs.Habits;
using DevHabitApi.Entities;
using DevHabitApi.Services;
using DevHabitApi.Services.Sorting;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DevHabitApi.Controllers;

[Authorize]
[ApiController]
[Route("habits")]
[ApiVersion(1.0)]
[Produces(
    MediaTypeNames.Application.Json,
    CustomMediaTypeNames.Application.JsonV1,
    CustomMediaTypeNames.Application.JsonV2,
    CustomMediaTypeNames.Application.HateoasJson,
    CustomMediaTypeNames.Application.HateoasJsonV1,
    CustomMediaTypeNames.Application.HateoasJsonV2)]
public sealed class HabitsController(
    ApplicationDbContext dbContext,
    LinkService linkService,
    UserContext userContext) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetHabits(
        [FromQuery] HabitsQueryParameters query,
        SortMappingProvider sortMappingProvider,
        DatashapingService dataShapingService)
    {
        string? userId = await userContext.GetUserIdAsync();

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        if (!sortMappingProvider.ValidateMappings<HabitDto, Habit>(query.Sort))
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                detail: $"The provided sort parameter isn't valid : '{query.Sort}'");
        }

        if (!dataShapingService.Validate<HabitDto>(query.Fields))
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                detail: $"The provided data shaping fields aren't valid : '{query.Fields}'");
        }

        query.Search ??= query.Search?.Trim().ToLower();

        var sortMappings = sortMappingProvider.GetMappings<HabitDto, Habit>();

        IQueryable<HabitDto> habitsQuery = dbContext.Habits
            .Where(h=> h.UserId == userId)
            .Where(h => query.Search == null ||
                        h.Name.ToLower().Contains(query.Search) ||
                        h.Description != null && h.Description.ToLower().Contains(query.Search))
            .Where(h => query.Type == null || h.Type == query.Type)
            .Where(h => query.Status == null || h.Status == query.Status)
            .ApplySort(query.Sort, sortMappings)
            .Select(HabitQueries.ProjectToDto());

        int totalCount = await habitsQuery.CountAsync();

        List<HabitDto> habits = await habitsQuery
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        bool includeLinks = query.Accept == CustomMediaTypeNames.Application.HateoasJson;

        var paginationResult = new PaginationResult<ExpandoObject>
        {
            Data = dataShapingService.ShapeDataCollection(
                habits,
                query.Fields,
                includeLinks ? h => CreateHabitLinks(h.Id, query.Fields) : null),
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = totalCount,
        };

        if (includeLinks)
        {
            paginationResult.Links = CreateHabitLinks(query, paginationResult.HasNextPage, paginationResult.HasPreviousPage);
        }

        return Ok(paginationResult);
    }

    [HttpGet("{id}")]
    [ApiVersion(1.0)]
    public async Task<IActionResult> GetHabit(
        string id,
        string? fields,
        [FromHeader]
        string? accept,
        DatashapingService dataShapingService)
    {
        string? userId = await userContext.GetUserIdAsync();

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        if (!dataShapingService.Validate<HabitWithTagsDto>(fields))
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                detail: $"The provided data shaping fields aren't valid : '{fields}'");
        }

        var habit = await dbContext.Habits
            .Where(h=> h.Id == id && h.UserId == userId)
            .Select(HabitQueries.ProjectToDtoWithTags())
            .FirstOrDefaultAsync();

        if (habit is null)
        {
            return NotFound();
        }

        ExpandoObject shapedHabitDto = dataShapingService.ShapeData(habit, fields);

        if(accept == CustomMediaTypeNames.Application.HateoasJson)
        {
            var links = CreateHabitLinks(id, fields);

            shapedHabitDto.TryAdd("links", links);
        }

        return Ok(shapedHabitDto);
    }

    //[HttpGet("{id}")]
    //[ApiVersion(2.0)]
    //public async Task<IActionResult> GetHabitV2(
    //string id,
    //string? fields,
    //[FromHeader]
    //    string? accept,
    //DatashapingService dataShapingService)
    //{
    //    string? userId = await userContext.GetUserIdAsync();

    //    if (string.IsNullOrWhiteSpace(userId))
    //    {
    //        return Unauthorized();
    //    }

    //    if (!dataShapingService.Validate<HabitWithTagsDtoV2>(fields))
    //    {
    //        return Problem(
    //            statusCode: StatusCodes.Status400BadRequest,
    //            detail: $"The provided data shaping fields aren't valid : '{fields}'");
    //    }

    //    var habit = await dbContext.Habits
    //        .Where(h => h.Id == id && h.UserId == userId)
    //        .Select(HabitQueries.ProjectToDtoWithTagsV2())
    //        .FirstOrDefaultAsync();

    //    if (habit is null)
    //    {
    //        return NotFound();
    //    }

    //    ExpandoObject shapedHabitDto = dataShapingService.ShapeData(habit, fields);

    //    if (accept == CustomMediaTypeNames.Application.HateoasJson)
    //    {
    //        var links = CreateHabitLinks(id, fields);

    //        shapedHabitDto.TryAdd("links", links);
    //    }

    //    return Ok(shapedHabitDto);
    //}

    [HttpPost]
    public async Task<ActionResult<HabitDto>> CreateHabit(
        CreateHabitDto createHabitDto,
        IValidator<CreateHabitDto> validator)
    {
        string? userId = await userContext.GetUserIdAsync();

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        await validator.ValidateAndThrowAsync(createHabitDto);

        var habit = createHabitDto.ToEntity(userId);

        await dbContext.Habits.AddAsync(habit);

        await dbContext.SaveChangesAsync();

        var habitDto = habit.ToDto();
        habitDto.Links = CreateHabitLinks(habitDto.Id, null);

        return CreatedAtAction(nameof(GetHabit), new { id = habitDto.Id }, habitDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateHabit(string id, UpdateHabitDto updateHabitDto)
    {
        string? userId = await userContext.GetUserIdAsync();

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var habit = await dbContext.Habits.FirstOrDefaultAsync(h => h.Id == id && h.UserId == userId);

        if (habit is null)
        {
            return NotFound();
        }

        habit.UpdateFromDto(updateHabitDto);

        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> PatchHabit(string id, JsonPatchDocument<HabitDto> patchDocument)
    {
        string? userId = await userContext.GetUserIdAsync();

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }
        var habit = await dbContext.Habits.FirstOrDefaultAsync(h => h.Id == id && h.UserId == userId);

        if (habit is null)
        {
            return NotFound();
        }

        HabitDto habitDto = habit.ToDto();

        patchDocument.ApplyTo(habitDto, ModelState);

        if (!TryValidateModel(habitDto))
        {
            return ValidationProblem(ModelState);
        }

        habit.Name = habitDto.Name;
        habit.Description = habitDto.Description;
        habit.UpdatedAtUtc = DateTime.UtcNow;

        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteHabit(string id)
    {
        string? userId = await userContext.GetUserIdAsync();

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }
        var habit = await dbContext.Habits.FirstOrDefaultAsync(h => h.Id == id && h.UserId == userId);

        if (habit is null)
        {
            return NotFound();
        }

        dbContext.Habits.Remove(habit);

        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    private List<LinkDto> CreateHabitLinks(
        HabitsQueryParameters query,
        bool hasNextPage,
        bool hasPreviousPage)
    {
        List<LinkDto> links =
        [
            linkService.Create(nameof(GetHabits) , "self" , HttpMethods.Get , new
            {
                page = query.Page,
                pageSize = query.PageSize,
                fields = query.Fields,
                q = query.Search,
                type = query.Type,
                status = query.Status
            }),
            linkService.Create(nameof(CreateHabit) , "create" , HttpMethods.Post)
        ];

        if (hasNextPage)
        {
            links.Add(linkService.Create(nameof(GetHabits), "next-page", HttpMethods.Get, new
            {
                page = query.Page + 1,
                pageSize = query.PageSize,
                fields = query.Fields,
                q = query.Search,
                type = query.Type,
                status = query.Status
            }));
        }

        if (hasPreviousPage)
        {
            links.Add(linkService.Create(nameof(GetHabits), "previous-page", HttpMethods.Get, new
            {
                page = query.Page - 1,
                pageSize = query.PageSize,
                fields = query.Fields,
                q = query.Search,
                type = query.Type,
                status = query.Status
            }));
        }

        return links;
    }

    private List<LinkDto> CreateHabitLinks(string id, string? fields)
    {
        return
        [
            linkService.Create(nameof(GetHabit) , "self" , HttpMethods.Get , new { id , fields }),
            linkService.Create(nameof(UpdateHabit) , "update" , HttpMethods.Put , new { id }),
            linkService.Create(nameof(PatchHabit) , "parital-update" , HttpMethods.Patch , new { id }),
            linkService.Create(nameof(DeleteHabit) , "delete" , HttpMethods.Delete , new { id }),
            linkService.Create(nameof(
                HabitTagsController.UpsertHabitTags),
                "upsert-tags",
                HttpMethods.Put,
                new { habitId = id }, 
                "habitTags")
        ];
    }
}
