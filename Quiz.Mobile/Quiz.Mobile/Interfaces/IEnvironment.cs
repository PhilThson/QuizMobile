using System;
using System.Drawing;
using System.Threading.Tasks;

namespace Quiz.Mobile.Interfaces
{
	public interface IEnvironment
	{
        Task SetStatusBarColorAsync(Color color, bool darkStatusBarTint);
    }
}

