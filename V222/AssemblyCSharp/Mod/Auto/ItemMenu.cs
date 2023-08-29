namespace UglyBoy;

public class ItemMenu
{
	public string info;

	public int idAction;

	public string value;

	private IActionListener actionListener;

	public ItemMenu(string info, string value, IActionListener actionListener, int idAction)
	{
		this.info = info;
		this.value = value;
		this.actionListener = actionListener;
		this.idAction = idAction;
	}

	public ItemMenu(string info, bool value, IActionListener actionListener, int idAction)
	{
		this.info = info;
		this.value = (value ? "Bật" : "Tắt");
		this.actionListener = actionListener;
		this.idAction = idAction;
	}

	public void perform()
	{
		if (actionListener != null)
		{
			actionListener.perform(idAction, null);
		}
	}
}
