using Pbt.Individual.Debugging;

namespace Pbt.Individual;

public class IndividualConsts
{
    public const string LocalizationSourceName = "Individual";

    public const string ConnectionStringName = "Default";

    public const bool MultiTenancyEnabled = false;


    /// <summary>
    /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
    /// </summary>
    public static readonly string DefaultPassPhrase =
        DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "48a2659a3dd94a81be5cdbac97878b7c";
}
