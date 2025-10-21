using Valoriza.Domain.Entities;


namespace Valoriza.Application.DTOs;


public record TransactionCreateDto(Guid UserId, decimal Amount, string Description, Category Category, DateTime? Date);
public record TransactionUpdateDto(decimal Amount, string Description, Category Category, DateTime? Date, bool IsRisky);
public record TransactionViewDto(Guid Id, Guid UserId, decimal Amount, string Description, Category Category, DateTime Date, bool IsRisky);