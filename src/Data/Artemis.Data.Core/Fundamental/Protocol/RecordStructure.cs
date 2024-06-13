namespace Artemis.Data.Core.Fundamental.Protocol;

/// <summary>
///     记录
/// </summary>
public class RecordStructure
{
    /// <summary>
    ///     记录长度
    /// </summary>
    public long Length => Properties?.Sum(property => property.Length) ?? 0;

    /// <summary>
    ///     属性结构值
    /// </summary>
    internal List<InternalStructure>? Internal
    {
        get
        {
            if (Properties == null)
                return null;

            var list = new List<InternalStructure>();

            var properties = Properties.OrderBy(property => property.Order).ToList();

            var offset = 0L;

            foreach (var property in properties)
            {
                list.Add(new InternalStructure
                {
                    Order = property.Order,
                    Name = property.Name,
                    Length = property.Length,
                    Type = property.Type,
                    Offset = offset
                });

                offset += property.Length;
            }

            return list;
        }
    }

    /// <summary>
    ///     属性结构
    /// </summary>
    public List<PropertyStructure>? Properties { get; set; }
}

/// <summary>
///     内部属性结构
/// </summary>
internal class InternalStructure : PropertyStructure
{
    /// <summary>
    ///     偏移量
    /// </summary>
    public long Offset { get; init; }
}

/// <summary>
///     属性结构
/// </summary>
public class PropertyStructure
{
    /// <summary>
    ///     属性序列
    /// </summary>
    public required int Order { get; init; }

    /// <summary>
    ///     属性名称
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    ///     属性长度
    /// </summary>
    public required long Length { get; init; }

    /// <summary>
    ///     属性类型
    /// </summary>
    public required string Type { get; init; }
}