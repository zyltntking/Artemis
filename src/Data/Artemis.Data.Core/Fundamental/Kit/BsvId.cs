namespace Artemis.Data.Core.Fundamental.Kit;

/// <summary>
///     Base Sort Value Id Generator
/// </summary>
public static class BsvId
{
    /// <summary>
    ///     根据ascii码表序列排序的仿base64字符集
    /// </summary>
    private static readonly Dictionary<char, char> SortableBase64CharSet = new()
    {
        { 'A', '$' }, { 'B', '-' }, { 'C', '0' }, { 'D', '1' }, { 'E', '2' }, { 'F', '3' }, { 'G', '4' },
        { 'H', '5' }, { 'I', '6' }, { 'J', '7' }, { 'K', '8' }, { 'L', '9' }, { 'M', 'A' }, { 'N', 'B' },
        { 'O', 'C' }, { 'P', 'D' }, { 'Q', 'E' }, { 'R', 'F' }, { 'S', 'G' }, { 'T', 'H' }, { 'U', 'I' },
        { 'V', 'J' }, { 'W', 'K' }, { 'X', 'L' }, { 'Y', 'M' }, { 'Z', 'N' }, { 'a', 'O' }, { 'b', 'P' },
        { 'c', 'Q' }, { 'd', 'R' }, { 'e', 'S' }, { 'f', 'T' }, { 'g', 'U' }, { 'h', 'V' }, { 'i', 'W' },
        { 'j', 'X' }, { 'k', 'Y' }, { 'l', 'Z' }, { 'm', 'a' }, { 'n', 'b' }, { 'o', 'c' }, { 'p', 'd' },
        { 'q', 'e' }, { 'r', 'f' }, { 's', 'g' }, { 't', 'h' }, { 'u', 'i' }, { 'v', 'j' }, { 'w', 'k' },
        { 'x', 'l' }, { 'y', 'm' }, { 'z', 'n' }, { '0', 'o' }, { '1', 'p' }, { '2', 'q' }, { '3', 'r' },
        { '4', 's' }, { '5', 't' }, { '6', 'u' }, { '7', 'v' }, { '8', 'w' }, { '9', 'x' }, { '+', 'y' },
        { '/', 'z' }, { '=', '!' }
    };

    /// <summary>
    ///     生成新的Id
    /// </summary>
    /// <returns></returns>
    public static string NewId()
    {
        var guid = Guid.NewGuid().ToByteArray();
        var dateValue = DateTime.UtcNow.Ticks;
        var dateBytes = BitConverter.GetBytes(dateValue);
        var bytes = new byte[guid.Length + dateBytes.Length];

        Buffer.BlockCopy(guid, 0, bytes, 0, guid.Length);
        Buffer.BlockCopy(dateBytes, 0, bytes, guid.Length, dateBytes.Length);

        var b64 = Convert.ToBase64String(bytes);

        var sortArray = b64.ToArray();

        for (var i = 0; i < sortArray.Length; i++) sortArray[i] = SortableBase64CharSet[sortArray[i]];

        return new string(sortArray);
    }
}