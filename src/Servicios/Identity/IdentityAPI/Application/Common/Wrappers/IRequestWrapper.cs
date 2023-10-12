using Application.Common.Models;
using MediatR;

namespace Application.Common.Wrappers;

/// <summary>
/// Wrapper Interface para IRequest. Retorna IResponse{T}
/// </summary>
public interface IRequestWrapper<T> : IRequest<IResponse<T>> { }

/// <summary>
/// Wrapper Interface para IRequestHandler{TRequest,TResponse}. To Handle IRequestWrapper{T}
/// </summary>
public interface IHandlerWrapper<in TRequest, TResponse> :
    IRequestHandler<TRequest, IResponse<TResponse>> where TRequest : IRequestWrapper<TResponse>
{ }