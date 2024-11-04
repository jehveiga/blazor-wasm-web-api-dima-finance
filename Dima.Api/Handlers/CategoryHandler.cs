using Dima.Api.Data;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers
{
    public class CategoryHandler(AppDbContext dbContext) : ICategoryHandler
    {
        public async Task<PagedResponse<List<Category>?>> GetAllAsync(GetAllCategoriesRequest getAllRequest)
        {
            try
            {
                IQueryable<Category> query = dbContext.Categories.AsNoTracking()
                                                 .Where(c => c.UserId == getAllRequest.UserId)
                                                 .OrderBy(c => c.Title);

                int skip = ((getAllRequest.PageNumber - 1) * getAllRequest.PageSize);
                int take = getAllRequest.PageSize;
                List<Category> categoriesDb = await query.Take(skip..take)
                                                         .ToListAsync();

                int count = await query.CountAsync();

                return new PagedResponse<List<Category>?>(data: categoriesDb,
                                                          totalCount: count,
                                                          currentPage: getAllRequest.PageNumber,
                                                          pageSize: getAllRequest.PageSize);
            }
            catch (Exception)
            {
                return new PagedResponse<List<Category>?>(default, code: 500, message: "Não foi possível consultar as categorias");
            }
        }

        public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest getByIdRequest)
        {
            try
            {
                Category? categoryDb = await dbContext.Categories.AsNoTracking()
                                                                 .FirstOrDefaultAsync(c => c.Id == getByIdRequest.Id &&
                                                                                      c.UserId == getByIdRequest.UserId);

                return categoryDb is null
                    ? new Response<Category?>(default, code: 404, message: "Não foi possível encontrar a categoria")
                    : new Response<Category?>(data: categoryDb, code: 200, message: "Categoria encontrada com sucesso");
            }
            catch (Exception)
            {
                return new Response<Category?>(default, 500, "Não foi possível recuperar a categoria");
            }
        }

        public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest createRequest)
        {
            try
            {
                Category category = new()
                {
                    Title = createRequest.Title,
                    Description = createRequest.Description,
                    UserId = createRequest.UserId
                };

                await dbContext.AddAsync(category);
                await dbContext.SaveChangesAsync();

                return new Response<Category?>(category);
            }
            catch (Exception)
            {
                return new Response<Category?>(default, 500, "Não foi possível criar a categoria");
            }
        }

        public async Task<Response<Category?>> UpdateAsync(DeteteCategoryRequest updateRequest)
        {
            try
            {
                Category? categoryDb = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == updateRequest.Id &&
                                                                      c.UserId == updateRequest.UserId);

                if (categoryDb is null)
                    return new Response<Category?>(default, 404, "Não foi possível encontrar a categoria");

                categoryDb.Title = updateRequest.Title;
                categoryDb.Description = updateRequest.Description;
                dbContext.Update(categoryDb);
                await dbContext.SaveChangesAsync();

                return new Response<Category?>(data: categoryDb, code: 201, message: "Categoria criada  com sucesso");
            }
            catch (Exception)
            {
                return new Response<Category?>(default, 500, "Não foi possível alterar a categoria");
            }

        }

        public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest deleteRequest)
        {
            try
            {
                Category? categoryDb = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == deleteRequest.Id &&
                                          c.UserId == deleteRequest.UserId);

                if (categoryDb is null)
                    return new Response<Category?>(default, 404, "Não foi possível encontrar a categoria");

                dbContext.Categories.Remove(categoryDb);
                await dbContext.SaveChangesAsync();

                return new Response<Category?>(default, message: "Categoria excluída com sucesso");
            }
            catch (Exception)
            {
                return new Response<Category?>(default, code: 500, message: "Não foi possível excluir a categoria referida");
            }
        }


    }
}
