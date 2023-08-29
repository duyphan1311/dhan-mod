public class MotherCanvas
{
	public static MotherCanvas instance;

	public GameCanvas tCanvas;

	public int zoomLevel = 1;

	public Image imgCache;

	private int[] imgRGBCache;

	private int newWidth;

	private int newHeight;

	private int[] output;

	private int OUTPUTSIZE = 20;

	public MotherCanvas()
	{
		checkZoomLevel(getWidth(), getHeight());
	}

	public void checkZoomLevel(int w, int h)
	{
		#region mod
		AssemblyCSharp.Mod.Other.GameEvents.onLoadGraphic();
		#endregion
	}

	public int getWidth()
	{
		return (int)ScaleGUI.WIDTH;
	}

	public int getHeight()
	{
		return (int)ScaleGUI.HEIGHT;
	}

	public void setChildCanvas(GameCanvas tCanvas)
	{
		this.tCanvas = tCanvas;
	}

	protected void paint(mGraphics g)
	{
		tCanvas.paint(g);
	}

	protected void keyPressed(int keyCode)
	{
		tCanvas.keyPressedz(keyCode);
	}

	protected void keyReleased(int keyCode)
	{
		tCanvas.keyReleasedz(keyCode);
	}

	protected void pointerDragged(int x, int y)
	{
		x /= mGraphics.zoomLevel;
		y /= mGraphics.zoomLevel;
		tCanvas.pointerDragged(x, y);
	}

	protected void pointerPressed(int x, int y)
	{
		x /= mGraphics.zoomLevel;
		y /= mGraphics.zoomLevel;
		tCanvas.pointerPressed(x, y);
	}

	protected void pointerReleased(int x, int y)
	{
		x /= mGraphics.zoomLevel;
		y /= mGraphics.zoomLevel;
		tCanvas.pointerReleased(x, y);
	}

	public int getWidthz()
	{
		int width = getWidth();
		return width / mGraphics.zoomLevel + width % mGraphics.zoomLevel;
	}

	public int getHeightz()
	{
		int height = getHeight();
		return height / mGraphics.zoomLevel + height % mGraphics.zoomLevel;
	}
}
