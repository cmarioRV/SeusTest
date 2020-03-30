
namespace demoseusapp
{
    public interface ICryptography
    {
        string GenerateRandomValue(int lenght);
        string EncryptValue(string value);
    }
}
