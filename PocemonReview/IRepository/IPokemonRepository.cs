using PocemonReview.Models;

namespace PocemonReview.IRepository
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon> GetAllPokemons();
        Pokemon GetPokemonById(int id);
        Pokemon GetPokemonByName(string name);
        decimal GetPokemonRating(int pokemonId);
        bool PokemonExists(int pokemonId);
        bool CreatePokemon(int ownerid,int coategoryId,Pokemon pokemon);
        bool Save();
    }
}
