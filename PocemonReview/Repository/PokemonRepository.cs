using PocemonReview.Data;
using PocemonReview.IRepository;
using PocemonReview.Models;

namespace PocemonReview.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext _dataContext;
        public PokemonRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var owner = _dataContext.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
            var category = _dataContext.Categories.Where(c => c.Id == categoryId).FirstOrDefault();

            var pokemnonOwner = new PokemonOwner
            {
                Owner = owner,
                Pokemon = pokemon
            };
            _dataContext.Add(pokemnonOwner);

            var pokemonCategory = new PokemonCategory
            {
                Category = category,
                Pokemon = pokemon
            };
            _dataContext.Add(pokemonCategory);

            _dataContext.Add(pokemon);
            return Save();
        }

        public ICollection<Pokemon> GetAllPokemons()
        {
            return _dataContext.Pokemons.OrderBy(p => p.Id).ToList();
        }

        public Pokemon GetPokemonById(int id)
        {
            return _dataContext.Pokemons.Where(p => p.Id == id).FirstOrDefault();
        }

        public Pokemon GetPokemonByName(string name)
        {
            return _dataContext.Pokemons.Where(p => p.Name == name).FirstOrDefault();
        }

        public decimal GetPokemonRating(int pokemonId)
        {
            var review = _dataContext.Reviews.Where(p => p.Pokemon.Id == pokemonId);
            if (review.Count() <= 0)
                return 0;
            return (decimal)(review.Average(r => r.Rating));
        }

        public bool PokemonExists(int pokemonId)
        {
            return _dataContext.Pokemons.Any(p => p.Id == pokemonId);
        }

        public bool Save()
        {
            var saved = _dataContext.SaveChanges();
            return saved > 0;
        }
    }
}
