namespace Checklist_API.Features.Common.Interfaces;

public interface IMapper<TEntity, TDto>
{
    TEntity MapToEntity(TDto dto);
    TDto MapToDTO(TEntity entity);
}
