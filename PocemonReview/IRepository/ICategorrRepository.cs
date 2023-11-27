using PocemonReview.Models;

namespace PocemonReview.IRepository
{
    public interface ICategorrRepository
    {
        ICollection<Category> GetAllCategories();
        Category GetCategory(int id);
        ICollection<Pokemon> GetPokemonByCategoryId(int categoryId);
        bool CategoryExist(int id);
        bool CreateCategory(Category category);
        bool UpdateCategory(Category category); 
        bool Save();
    }
}
