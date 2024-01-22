using System;
using System.IO;
using Avalonia.Media.Imaging;

namespace ServerApp.Models;

public static class BitmapConverter
{
	// ConvertBase64ToBitmap
	public static Bitmap ConvertBase64ToBitmap(string base64)
	{
		byte[] bytes = Convert.FromBase64String(base64);
		return ConvertBase64ToBitmap(bytes);
	} 
	
	// ConvertBase64ToBitmap
	public static Bitmap ConvertBase64ToBitmap(byte[] bytes)
	{
		using MemoryStream stream = new(bytes);
		return new Bitmap(stream);
	}
}