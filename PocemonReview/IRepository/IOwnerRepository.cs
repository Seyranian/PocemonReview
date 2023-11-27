using PocemonReview.Models;

namespace PocemonReview.IRepository
{
    public interface IOwnerRepository
    {
        ICollection<Owner> GetAllOwners();
        Owner GetOwner(int id);
        ICollection<Owner> GetOwnerOfAPokemon(int pokemonId);
        ICollection<Pokemon> GetPokemonByOwner(int ownerId);
        bool OwnerExist(int id);
        bool CreateOwner(Owner owner);
        bool UpdateOwner(Owner owner);
        bool Save();
    }
}
