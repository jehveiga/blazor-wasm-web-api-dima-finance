using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using System.Net.Http.Json;

namespace Dima.Web.Handlers;

public class CategoryHandler(IHttpClientFactory httpClientFactory) : ICategoryHandler
{
    private readonly HttpClient _client = httpClientFactory.CreateClient(Configuration.HTTPCLIENT_NAME);

    public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest createRequest)
    {
        HttpResponseMessage result = await _client.PostAsJsonAsync("v1/categories", createRequest);
        return await result.Content.ReadFromJsonAsync<Response<Category?>>()
               ?? new Response<Category?>(null, 400, "Falha ao criar a categoria");
    }

    public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest updateRequest)
    {
        HttpResponseMessage result = await _client.PutAsJsonAsync($"v1/categories/{updateRequest.Id}", updateRequest);
        return await result.Content.ReadFromJsonAsync<Response<Category?>>()
               ?? new Response<Category?>(null, 400, "Falha ao atualizar a categoria");
    }

    public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest deleteRequest)
    {
        HttpResponseMessage result = await _client.DeleteAsync($"v1/categories/{deleteRequest.Id}");
        return await result.Content.ReadFromJsonAsync<Response<Category?>>()
               ?? new Response<Category?>(null, 400, "Falha ao excluir a categoria");
    }

    public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest getByIdRequest)
        => await _client.GetFromJsonAsync<Response<Category?>>($"v1/categories/{getByIdRequest.Id}")
           ?? new Response<Category?>(null, 400, "Não foi possível obter a categoria");

    public async Task<PagedResponse<List<Category>?>> GetAllAsync(GetAllCategoriesRequest getAllRequest)
        => await _client.GetFromJsonAsync<PagedResponse<List<Category>?>>("v1/categories")
           ?? new PagedResponse<List<Category>?>(null, 400, "Não foi possível obter as categorias");
}