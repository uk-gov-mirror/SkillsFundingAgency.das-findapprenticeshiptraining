namespace SFA.DAS.FAT.Domain.Interfaces
{
    public interface ICookieStorageService<T>
    {
        void Create(T item, string cookieName, int expiryDays = 1);
        T Get(string cookieName);
        void Delete(string cookieName);
        void Update(string cookieName, T item, int expiryDays = 1);
    }
}