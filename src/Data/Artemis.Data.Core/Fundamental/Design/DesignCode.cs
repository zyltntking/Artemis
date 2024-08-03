namespace Artemis.Data.Core.Fundamental.Design;

/// <summary>
///     组织机构设计代码
/// </summary>
public class DesignCode
{
    /// <summary>
    ///     生成组织机构设计编码
    /// </summary>
    /// <param name="region"></param>
    /// <param name="serial"></param>
    /// <param name="parentDesignCode"></param>
    /// <returns></returns>
    public static string Organization(string region, int serial, string? parentDesignCode = null)
    {
        var prefix = "ORG";

        if (!string.IsNullOrWhiteSpace(parentDesignCode))
        {
            var length = prefix.Length + region.Length;

            parentDesignCode = parentDesignCode[length..];
        }

        return $"{prefix}{region}{parentDesignCode}{serial:D3}";
    }

    /// <summary>
    ///     生成任务设计编码
    /// </summary>
    /// <param name="organizationDesignCode"></param>
    /// <param name="serial"></param>
    /// <param name="parentDesignCode"></param>
    /// <returns></returns>
    public static string Task(string organizationDesignCode, int serial, string? parentDesignCode = null)
    {
        var prefix = "TA";

        var date = DateTime.Now.ToString("yyMMddHHmmss");

        var segment = "SK";

        if (!string.IsNullOrWhiteSpace(parentDesignCode))
        {
            var length = prefix.Length + date.Length + organizationDesignCode.Length + segment.Length;

            parentDesignCode = parentDesignCode[length..];
        }

        return $"{prefix}{date}{organizationDesignCode}{segment}{parentDesignCode}{serial:D3}";
    }
}