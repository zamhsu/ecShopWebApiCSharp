using Common.Enums;
using Service.Implements.Security;
using Xunit;

namespace ServiceTests.Implements.Security;

public class EncryptionServiceTests
{
    private readonly EncryptionService _sut;

    public EncryptionServiceTests()
    {
        _sut = new EncryptionService();
    }
    
    [Fact]
    public void CreatePasswordHash_輸入密碼和鹽_使用Sha256_應回傳正確的雜湊()
    {
        // arrange
        var password = "ABC";
        var saltKey = "123";
        var hashAlgorithm = GeneralHashAlgorithmEnum.SHA256;
        var expected = "E0BEBD22819993425814866B62701E2919EA26F1370499C1037B53B9D49C2C8A";

        // act
        var actual = _sut.CreatePasswordHash(password, saltKey, hashAlgorithm);
        
        // assert
        Assert.Contains(expected, actual);
    }
    
    [Fact]
    public void CreatePasswordHash_輸入密碼和鹽_使用Sha512_應回傳正確的雜湊()
    {
        // arrange
        var password = "ABC";
        var saltKey = "123";
        var hashAlgorithm = GeneralHashAlgorithmEnum.SHA512;
        var expected = "8C9333343C6C4222418EDB1D7C9F84D051610526085960A1732C7C3D763FFF64EC7F5220998434C896DDA243AE777D0FB213F36B9B19F7E4A244D5C993B8DFED";

        // act
        var actual = _sut.CreatePasswordHash(password, saltKey, hashAlgorithm);
        
        // assert
        Assert.Contains(expected, actual);
    }
}