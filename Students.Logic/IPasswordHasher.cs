namespace Students.Logic
{
  public interface IPasswordHasher
  {
    string Hash(string password);
    bool Compare(string hash, string password);
  }
}
