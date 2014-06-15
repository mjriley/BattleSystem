using System.Resources;
using System.Reflection;

namespace PokeCore {

public static class L18N
{
	static ResourceManager rm;
	static L18N()
	{
		rm = new ResourceManager("Assembly-CSharp", Assembly.GetExecutingAssembly());
	}
	
	public static string GetItemCategory(ItemCategory category)
	{
		return rm.GetString("ITEM_CATEGORY_" + category.ToString());
	}
	
	public static string Get(string key)
	{
		return rm.GetString(ConvertKey(key));
	}
	
	static string ConvertKey(string key)
	{
		return key.Replace(' ', '_').ToUpper();
	}
}

}

