using Application.Common.DTOs.Auth;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Wrappers;
using AutoMapper;
using Domain.Exceptions;

namespace Application.Queries.Users;

public record GetRefreshTokenByUserIdQuery(string userId) : IRequestWrapper<IReadOnlyList<RefreshTokenDto>>;

internal sealed class GetRefreshTokenByUserIdQueryHandler :
IHandlerWrapper<GetRefreshTokenByUserIdQuery, IReadOnlyList<RefreshTokenDto>>
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public GetRefreshTokenByUserIdQueryHandler(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<IResponse<IReadOnlyList<RefreshTokenDto>>> Handle(
        GetRefreshTokenByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByIdAsync(request.userId) ??
        throw new UserNotFoundException(request.userId);

        return Response.Success(_mapper.Map<IReadOnlyList<RefreshTokenDto>>(user.RefreshTokens));
    }
}