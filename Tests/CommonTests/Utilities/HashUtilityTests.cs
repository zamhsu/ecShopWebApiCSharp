using System;
using System.Text;
using Common.Enums;
using Common.Utilities;
using Xunit;

namespace CommonTests.Utilities;

public class HashUtilityTests
{
    [Fact]
    public void CreateGeneralHash_HashSha256_應回傳正確的雜湊()
    {
        // arrange
        var data = Encoding.UTF8.GetBytes("ABC123");
        var hashAlgorithm = GeneralHashAlgorithmEnum.SHA256;
        var expected = "E0BEBD22819993425814866B62701E2919EA26F1370499C1037B53B9D49C2C8A";

        // act
        var actual = HashUtility.CreateGeneralHash(data, hashAlgorithm);

        // assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CreateGeneralHash_HashSha512_應回傳正確的雜湊()
    {
        // arrange
        var data = Encoding.UTF8.GetBytes("ABC123");
        var hashAlgorithm = GeneralHashAlgorithmEnum.SHA512;
        var expected = "8C9333343C6C4222418EDB1D7C9F84D051610526085960A1732C7C3D763FFF64EC7F5220998434C896DDA243AE777D0FB213F36B9B19F7E4A244D5C993B8DFED";

        // act
        var actual = HashUtility.CreateGeneralHash(data, hashAlgorithm);

        // assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void HmacSha256Signature_輸入字串長度6_應拋出例外字串長度不足()
    {
        // arrange
        var signKey = "ABC123";
        var expected = "Must longer than 16 characters";

        // act
        var actual = Assert.Throws<ArgumentException>(() => HashUtility.CreateHmacSha256Signature(signKey));

        // assert
        Assert.Contains(expected, actual.Message);
    }
}