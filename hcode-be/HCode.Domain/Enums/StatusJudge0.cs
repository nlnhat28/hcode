
namespace HCode.Domain
{
    /// <summary>
    /// Các trạng thái của submission judge0
    /// </summary>
    public enum StatusJudge0
    {
        Error = 0,
        InQueue = 1,
        Processing = 2,
        Accepted = 3,
        WrongAnswer = 4,
        TimeLimitExceeded = 5,
        CompilationError = 6,
        RuntimeErrorSIGSEGV = 7,
        RuntimeErrorSIGXFSZ = 8,
        RuntimeErrorSIGFPE = 9,
        RuntimeErrorSIGABRT = 10,
        RuntimeErrorNZEC = 11,
        RuntimeErrorOther = 12,
        InternalError = 13,
        ExecFormatError = 14,
    }
}
