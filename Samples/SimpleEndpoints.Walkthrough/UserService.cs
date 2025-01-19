namespace NocturnalGroup.SimpleEndpoints.Walkthrough;

/// <summary>
/// A user of the application.
/// </summary>
public sealed record User(int Id, string Username);

/// <summary>
/// This is an example of a service you'd have in your app.
/// It's really not important. It's just here so the project compiles.
/// </summary>
public class UserService
{
	private readonly List<User> _users = [new(0, "John Doe"), new(1, "Sam Smith")];

	/// <summary>
	/// Retrieves all users from the "database"
	/// </summary>
	public IEnumerable<User> GetUsers()
	{
		return _users;
	}

	/// <summary>
	/// Retrieves a user from the "database" with the matching id, or null if none are found.
	/// </summary>
	public User? GetUser(int id)
	{
		return _users.FirstOrDefault(u => u.Id == id);
	}
}
