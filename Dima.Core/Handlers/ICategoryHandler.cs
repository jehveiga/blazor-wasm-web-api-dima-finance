using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;

namespace Dima.Core.Handlers
{
    public interface ICategoryHandler
    {
        Task<PagedResponse<List<Category>?>> GetAllAsync(GetAllCategoriesRequest getAllRequest);
        Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest getByIdRequest);
        Task<Response<Category?>> CreateAsync(CreateCategoryRequest createRequest);
        Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest updateRequest);
        Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest deleteRequest);
    }
}
