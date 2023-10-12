using Application.Common.DTOs.User;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Wrappers;
using AutoMapper;

namespace Application.Queries.Users;

public record GetAllUsersQuery : IRequestWrapper<IReadOnlyList<UserDto>>;

internal sealed class GetAllUserQueryHandler : IHandlerWrapper<GetAllUsersQuery, IReadOnlyList<UserDto>>
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public GetAllUserQueryHandler(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<IResponse<IReadOnlyList<UserDto>>> Handle(
        GetAllUsersQuery request,
        CancellationToken cancellationToken)
    {
        var users = await _userService.GetAllUsersAsync();
        return Response.Success(_mapper.Map<IReadOnlyList<UserDto>>(users));
    }
}