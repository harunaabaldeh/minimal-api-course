namespace Minimal.Api;

public record Person(string FullName);

public class PeopleService
{
    private readonly List<Person> _people = new()
    {
        new Person("Bob"),
        new Person("Sam"),
        new Person("Mike"),
        new Person("Samule")
    };

    public IEnumerable<Person> Search(string searchTerm)
    {
        return _people.Where(x => x.FullName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
    }
}