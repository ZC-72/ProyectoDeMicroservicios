using Application.Common.DTOs.User;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Wrappers;
using AutoMapper;
using Domain.Exceptions;

namespace Application.Queries.Users;

public record GetUserQuery(string userId) : IRequestWrapper<UserDto>;

internal sealed class GetUserQueryHandler : IHandlerWrapper<GetUserQuery, UserDto>
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public GetUserQueryHandler(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<IResponse<UserDto>> Handle(
        GetUserQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByIdAsync(request.userId) ??
        throw new UserNotFoundException(request.userId);

        return Response.Success(_mapper.Map<UserDto>(user));
    }
}