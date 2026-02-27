using System.ComponentModel;

namespace Puush.Persistence.Models.Enums;

public enum AccountType
{
    [Description("Free Account")]
    Free = 0,
    [Description("Pro Account")]
    Pro = 1,
    [Description("Pro Tester")]
    ProTester = 2,
    [Description("Haxor!")]
    Haxor = 9
}
