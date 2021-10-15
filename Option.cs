
using System.Collections.Generic;

struct AbilityOption
{
	public string Name { get; set; }
	public string Probability { get; set; }
}

struct ValueOption
{
	public string Value { get; set; }
	public string Probability { get; set; }
}

struct AbilityValueOption
{
	public string Name { get; set; }
	
	public List<ValueOption> ValueOptions { get; set; }
}
