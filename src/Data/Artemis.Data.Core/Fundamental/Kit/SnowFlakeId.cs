namespace Artemis.Data.Core.Fundamental.Kit;

/// <summary>
/// 雪花Id生成器
/// </summary>
public class SnowFlakeId
{
    //起始的时间戳
    private const long StartStamp = 1480166465631L;

    //每一部分占用的位数
    private const int SequenceBit = 12; //序列号占用的位数
    private const int MachineBit = 5;   //机器标识占用的位数
    private const int DatacenterBit = 5;//数据中心占用的位数

    //每一部分的最大值
    private const long MaxDatacenterNum = -1L ^ (-1L << DatacenterBit);
    private const long MaxMachineNum = -1L ^ (-1L << MachineBit);
    private const long MaxSequence = -1L ^ (-1L << SequenceBit);

    //每一部分向左的位移
    private const int MachineLeft = SequenceBit;
    private const int DatacenterLeft = SequenceBit + MachineBit;
    private const int TimestampLeft = DatacenterLeft + DatacenterBit;

    private long _datacenterId;  //数据中心
    private long _machineId;     //机器标识
    private long _sequence; //序列号
    private long _lastStamp = -1L;//上一次时间戳

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="datacenterId">数据中心标识</param>
    /// <param name="machineId">机器标识</param>
    /// <exception cref="Exception"></exception>
    public SnowFlakeId(long datacenterId, long machineId)
    {
        if (datacenterId is > MaxDatacenterNum or < 0)
        {
            throw new Exception("数据中心Id不能大于" + MaxDatacenterNum + "或小于0");
        }
        if (machineId is > MaxMachineNum or < 0)
        {
            throw new Exception("机器Id不能大于" + MaxMachineNum + "或小于0");
        }
        _datacenterId = datacenterId;
        _machineId = machineId;
    }

    /// <summary>
    /// 获取下一个id
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public long NextId()
    {
        lock (this)
        {
            var currStamp = TimeGen();
            if (currStamp < _lastStamp)
            {
                throw new Exception("Clock moved backwards.  Refusing to generate id");
            }

            if (currStamp == _lastStamp)
            {
                _sequence = (_sequence + 1) & MaxSequence;
                if (_sequence == 0)
                {
                    currStamp = GetNextMill();
                }
            }
            else
            {
                _sequence = 0L;
            }

            _lastStamp = currStamp;

            return (currStamp - StartStamp) << TimestampLeft
                   | _datacenterId << DatacenterLeft
                   | _machineId << MachineLeft
                   | _sequence;
        }
    }

    private long GetNextMill()
    {
        var mill = TimeGen();
        while (mill <= _lastStamp)
        {
            mill = TimeGen();
        }
        return mill;
    }

    private long TimeGen()
    {
        return DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }

}