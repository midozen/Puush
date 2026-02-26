using System.ComponentModel;

namespace Puush.Models;

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